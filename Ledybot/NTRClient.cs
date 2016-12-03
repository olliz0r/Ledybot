using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Ledybot
{
    public class NTRClient
    {
        public String host;
        public int port;
        public TcpClient tcp;
        public NetworkStream netStream;
        public Thread packetRecvThread;
        UInt32 lastReadMemSeq;
        private object syncLock = new object();
        public object retValLock = new object();
        int heartbeatSendable;
        int timeout;
        public delegate void logHandler(string msg);
        UInt32 currentSeq;
        public volatile int progress = -1;

        public string retVal;
        public bool retDone;


        int readNetworkStream(NetworkStream stream, byte[] buf, int length)
        {
            int index = 0;
            bool useProgress = false;

            if (length > 100000)
            {
                useProgress = true;
            }
            do
            {
                if (useProgress)
                {
                    progress = (int)(((double)(index) / length) * 100);
                }
                int len = stream.Read(buf, index, length - index);
                if (len == 0)
                {
                    return 0;
                }
                index += len;
            } while (index < length);
            progress = -1;
            return length;
        }

        void packetRecvThreadStart()
        {
            byte[] buf = new byte[84];
            UInt32[] args = new UInt32[16];
            int ret;
            NetworkStream stream = netStream;

            while (true)
            {
                try
                {
                    ret = readNetworkStream(stream, buf, buf.Length);
                    if (ret == 0)
                    {
                        break;
                    }
                    int t = 0;
                    UInt32 magic = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    UInt32 seq = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    UInt32 type = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    UInt32 cmd = BitConverter.ToUInt32(buf, t);
                    for (int i = 0; i < args.Length; i++)
                    {
                        t += 4;
                        args[i] = BitConverter.ToUInt32(buf, t);
                    }
                    t += 4;
                    UInt32 dataLen = BitConverter.ToUInt32(buf, t);

                    if (magic != 0x12345678)
                    {
                        break;
                    }

                    if (cmd == 0)
                    {
                        if (dataLen != 0)
                        {
                            byte[] dataBuf = new byte[dataLen];
                            readNetworkStream(stream, dataBuf, dataBuf.Length);
                            string logMsg = Encoding.UTF8.GetString(dataBuf);
                            //Console.WriteLine(logMsg);
                        }
                        lock (syncLock)
                        {
                            heartbeatSendable = 1;
                        }
                        continue;
                    }
                    if (dataLen != 0)
                    {
                        byte[] dataBuf = new byte[dataLen];
                        readNetworkStream(stream, dataBuf, dataBuf.Length);
                        handlePacket(cmd, seq, dataBuf);
                    }
                }
                catch
                {
                    break;
                }
            }
            disconnect(false);
        }

        void handlePacket(UInt32 cmd, UInt32 seq, byte[] dataBuf)
        {
            if (cmd == 9)
            {
                handleReadMem(seq, dataBuf);
            }
        }

        void handleReadMem(UInt32 seq, byte[] dataBuf)
        {
            if (seq != lastReadMemSeq)
            {
                //log("seq != lastReadMemSeq, ignored");
                return;
            }
            lastReadMemSeq = 0;

            int i = 0;
            int iBufferlength = dataBuf.Length;
            for (i = 0; i < dataBuf.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (dataBuf[i] == 0x00)
                    {
                        iBufferlength = i;
                        break;
                    }
                }
            }
            byte[] actualBuffer = new byte[iBufferlength];
            for (i = 0; i < actualBuffer.Length; i++)
            {
                actualBuffer[i] = dataBuf[i];
            }

            string szResult = "";

            if(dataBuf.Length <= 4)
            {
                Array.Reverse(actualBuffer);
                szResult = BitConverter.ToString(actualBuffer).Replace("-", string.Empty);
            }
            else
            {
                szResult = Encoding.Unicode.GetString(actualBuffer).Trim('\0');
            }
            

            //t.BeginInvoke((MethodInvoker)delegate () { t.Text = szResult; ; });
            lock (retValLock)
            {
                retVal = szResult;
                retDone = true;
            }
            return;
            //log(byteToHex(dataBuf, 0));

        }

        public void sendReadMemPacket(UInt32 addr, UInt32 size, UInt32 pid)
        {
            sendEmptyPacket(9, pid, addr, size);
            lastReadMemSeq = currentSeq;
        }

        public void sendEmptyPacket(UInt32 cmd, UInt32 arg0 = 0, UInt32 arg1 = 0, UInt32 arg2 = 0, UInt32 arg3 = 0, UInt32 arg4 = 0)
        {
            UInt32[] args = new UInt32[16];

            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            args[3] = arg3;
            args[4] = arg4;
            sendPacket(0, cmd, args, 0);
        }

        public void setServer(String serverHost, int serverPort)
        {
            host = serverHost;
            port = serverPort;
        }

        public Boolean connectToServer()
        {
            if (tcp != null)
            {
                disconnect();
            }
            tcp = new TcpClient();
            tcp.NoDelay = true;
            try
            {
                if (tcp.ConnectAsync(host, port).Wait(1000))
                {
                    currentSeq = 0;
                    netStream = tcp.GetStream();
                    heartbeatSendable = 1;
                    packetRecvThread = new Thread(new ThreadStart(packetRecvThreadStart));
                    packetRecvThread.Start();
                    Program.Connected = true;
                }
                else
                {
                    Program.Connected = false;
                }
            }
            catch
            {
                Program.Connected = false;
            }

            return Program.Connected;
        }

        public void disconnect(bool waitPacketThread = true)
        {
            try
            {
                if (tcp != null)
                {
                    tcp.Close();
                }
                if (waitPacketThread)
                {
                    if (packetRecvThread != null)
                    {
                        packetRecvThread.Join();
                    }
                }
            }
            catch { }
            tcp = null;
            Program.Connected = false;
        }

        public void sendPacket(UInt32 type, UInt32 cmd, UInt32[] args, UInt32 dataLen)
        {
            int t = 0;
            currentSeq += 1000;
            byte[] buf = new byte[84];
            BitConverter.GetBytes(0x12345678).CopyTo(buf, t);
            t += 4;
            BitConverter.GetBytes(currentSeq).CopyTo(buf, t);
            t += 4;
            BitConverter.GetBytes(type).CopyTo(buf, t);
            t += 4;
            BitConverter.GetBytes(cmd).CopyTo(buf, t);
            for (int i = 0; i < 16; i++)
            {
                t += 4;
                UInt32 arg = 0;
                if (args != null)
                {
                    arg = args[i];
                }
                BitConverter.GetBytes(arg).CopyTo(buf, t);
            }
            t += 4;
            BitConverter.GetBytes(dataLen).CopyTo(buf, t);
            try
            {
                netStream.Write(buf, 0, buf.Length);
            }
            catch (Exception)
            {
            }
        }

        public void sendWriteMemPacket(UInt32 addr, UInt32 pid, byte[] buf)
        {
            UInt32[] args = new UInt32[16];
            args[0] = pid;
            args[1] = addr;
            args[2] = (UInt32)buf.Length;
            sendPacket(1, 10, args, args[2]);
            netStream.Write(buf, 0, buf.Length);
        }

        public void sendHeartbeatPacket()
        {
            if (Program.Connected)
            {
                lock (syncLock)
                {
                    if (heartbeatSendable == 1)
                    {
                        heartbeatSendable = 0;
                        sendPacket(0, 0, null, 0);
                    }
                    else
                    {
                        timeout++;
                        if (timeout == 5)
                        {
                            disconnect(false);
                        }
                    }
                }
            }

        }
    }
}