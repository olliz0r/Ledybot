using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedyLib
{
    public class ScriptHelper
    {
        private readonly NTR _ntrClient;

        public delegate void autoDisconnect();

        public event autoDisconnect onAutoDisconnect;

        public ScriptHelper(NTR ntrClient)
        {
            this._ntrClient = ntrClient;
        }

        public void bpadd(uint addr, string type = "code.once")
        {
            uint num = 0;
            switch (type)
            {
                case "code":
                    num = 1;
                    break;
                case "code.once":
                    num = 2;
                    break;
            }

            if (num != 0)
            {
                _ntrClient.sendEmptyPacket(11, num, addr, 1);
            }
        }

        public void remoteplay()
        {
            _ntrClient.sendEmptyPacket(901);
            _ntrClient.log("Will be disconnected in 10 seconds to enhance performance.");
            onAutoDisconnect?.Invoke();
        }

        public void bpdis(uint id)
        {
            _ntrClient.sendEmptyPacket(11, id, 0, 3);
        }

        public void bpena(uint id)
        {
            _ntrClient.sendEmptyPacket(11, id, 0, 2);
        }

        public void resume()
        {
            _ntrClient.sendEmptyPacket(11, 0, 0, 4);
        }

        public void connect(string host, int port)
        {
            _ntrClient.setServer(host, port);
            _ntrClient.connectToServer();
        }

        public void reload()
        {
            _ntrClient.sendReloadPacket();
        }

        public void listprocess()
        {
            _ntrClient.sendEmptyPacket(5);
        }

        public void listthread(int pid)
        {
            _ntrClient.sendEmptyPacket(7, (uint)pid);
        }

        public void attachprocess(int pid, uint patchAddr = 0)
        {
            _ntrClient.sendEmptyPacket(6, (uint)pid, patchAddr);
        }

        public void queryhandle(int pid)
        {
            _ntrClient.sendEmptyPacket(12, (uint)pid);
        }

        public void memlayout(int pid)
        {
            _ntrClient.sendEmptyPacket(8, (uint)pid);
        }

        public void disconnect()
        {
            _ntrClient.disconnect();
        }

        public void sayhello()
        {
            _ntrClient.sendHelloPacket();
        }

        public void data(uint addr, uint size = 0x100, int pid = -1, string filename = null)
        {
            if (filename == null && size > 1024)
            {
                size = 1024;
            }
            _ntrClient.sendReadMemPacket(addr, size, (uint)pid, filename);
        }

        public uint data(uint addr, uint size = 0x100, int pid = -1)
        {
            return _ntrClient.sendReadMemPacket(addr, size, (uint)pid);
        }

        public void write(uint addr, byte[] buf, int pid = -1)
        {
            _ntrClient.sendWriteMemPacket(addr, (uint)pid, buf);
        }

        public void writebyte(uint addr, byte buf, int pid = -1)
        {
            _ntrClient.sendWriteMemPacketByte(addr, (uint)pid, buf);
        }

        public void sendfile(String localPath, String remotePath)
        {
            FileStream fs = new FileStream(localPath, FileMode.Open);
            byte[] buf = new byte[fs.Length];
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            _ntrClient.sendSaveFilePacket(remotePath, buf);
        }
    }
}
