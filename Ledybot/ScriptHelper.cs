using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ledybot
{
    public class ScriptHelper
    {
        public uint left = 0xFDF;
        public uint right = 0xFEF;
        public uint up = 0xFBF;
        public uint down = 0xF7F;
        public uint start = 0xFF7;
        public uint Abtn = 0xFFE;
        public uint Bbtn = 0xFFD;

        public uint searchBtn = 0x01B618E0;
        
        public void connect(string host, int port)
        {
            Program.ntrClient.setServer(host, port);
            Program.ntrClient.connectToServer();
        }

        public void disconnect()
        {
            Program.ntrClient.disconnect();
        }

        public void write(uint addr, byte[] buf, int pid = -1)
        {
            Program.ntrClient.sendWriteMemPacket(addr, (uint)pid, buf);
        }

        public void press(uint buttons)
        {
            byte[] data = new byte[12];
            uint oldbuttons = buttons;
            uint oldtouch = 0x2000000;
            uint oldcpad = 0x800800;
            data[0x00] = (byte)(oldbuttons & 0xFF);
            data[0x01] = (byte)((oldbuttons >> 0x08) & 0xFF);
            data[0x02] = (byte)((oldbuttons >> 0x10) & 0xFF);
            data[0x03] = (byte)((oldbuttons >> 0x18) & 0xFF);

            //Touch
            data[0x04] = (byte)(oldtouch & 0xFF);
            data[0x05] = (byte)((oldtouch >> 0x08) & 0xFF);
            data[0x06] = (byte)((oldtouch >> 0x10) & 0xFF);
            data[0x07] = (byte)((oldtouch >> 0x18) & 0xFF);

            //CPad
            data[0x08] = (byte)(oldcpad & 0xFF);
            data[0x09] = (byte)((oldcpad >> 0x08) & 0xFF);
            data[0x0A] = (byte)((oldcpad >> 0x10) & 0xFF);
            data[0x0B] = (byte)((oldcpad >> 0x18) & 0xFF);

            Program.scriptHelper.write(0x10DF20, data, 0x10);
            Thread.Sleep(150);
            oldbuttons = 0xFFF;
            data[0x00] = (byte)(oldbuttons & 0xFF);
            data[0x01] = (byte)((oldbuttons >> 0x08) & 0xFF);
            data[0x02] = (byte)((oldbuttons >> 0x10) & 0xFF);
            data[0x03] = (byte)((oldbuttons >> 0x18) & 0xFF);

            this.write(0x10DF20, data, 0x10);
        }

        public void touch(uint touch)
        {
            byte[] data = new byte[12];
            uint oldbuttons = 0xFFF;
            uint oldtouch = touch;
            uint oldcpad = 0x800800;
            data[0x00] = (byte)(oldbuttons & 0xFF);
            data[0x01] = (byte)((oldbuttons >> 0x08) & 0xFF);
            data[0x02] = (byte)((oldbuttons >> 0x10) & 0xFF);
            data[0x03] = (byte)((oldbuttons >> 0x18) & 0xFF);

            //Touch
            data[0x04] = (byte)(oldtouch & 0xFF);
            data[0x05] = (byte)((oldtouch >> 0x08) & 0xFF);
            data[0x06] = (byte)((oldtouch >> 0x10) & 0xFF);
            data[0x07] = (byte)((oldtouch >> 0x18) & 0xFF);

            //CPad
            data[0x08] = (byte)(oldcpad & 0xFF);
            data[0x09] = (byte)((oldcpad >> 0x08) & 0xFF);
            data[0x0A] = (byte)((oldcpad >> 0x10) & 0xFF);
            data[0x0B] = (byte)((oldcpad >> 0x18) & 0xFF);

            Program.scriptHelper.write(0x10DF20, data, 0x10);
            Thread.Sleep(150);
            oldtouch = 0x2000000;
            data[0x04] = (byte)(oldtouch & 0xFF);
            data[0x05] = (byte)((oldtouch >> 0x08) & 0xFF);
            data[0x06] = (byte)((oldtouch >> 0x10) & 0xFF);
            data[0x07] = (byte)((oldtouch >> 0x18) & 0xFF);

            Program.scriptHelper.write(0x10DF20, data, 0x10);

        }

        public string readSafe(UInt32 addr, UInt32 byteCount, int pid)
        {
            lock (Program.ntrClient.retValLock)
            {
                Program.ntrClient.retDone = false;
            }
            bool waiting = true;
            Program.ntrClient.sendReadMemPacket(addr, byteCount, (uint)pid);
            string szReturn = "";
            while (waiting)
            {
                Thread.Sleep(10);
                lock (Program.ntrClient.retValLock)
                {
                    if (Program.ntrClient.retDone)
                    {
                        waiting = false;
                        szReturn = Program.ntrClient.retVal;
                    }
                }
            }
            return szReturn;
        }

        public void listprocess()
        {
            Program.ntrClient.sendEmptyPacket(5);
        }

    }
}