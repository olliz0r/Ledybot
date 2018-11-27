using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Windows.Forms;
using LedyLib;

namespace Ledybot
{
    static class Program
    {
        public static NTR ntrClient;
        public static Data data;
        public static GTSBot7 gtsBot;
        public static EggBot eggBot;
        public static ScriptHelper scriptHelper;
        public static RemoteControl helper;
        public static MainForm f1;
        public static LookupTable PKTable;
        public static PKHeX pkhex;
        public static GiveawayDetails gd;
        public static BanlistDetails bld;
        public static List<KeyValuePair<string, ArrayList>> ServerList = new List<KeyValuePair<string, ArrayList>>();


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ntrClient = new NTR();
            ntrClient.DataReady += NTR.handleDataReady;
            PKTable = new LookupTable();
            pkhex = new PKHeX();
            data = new Data();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new MainForm();
            gd = new GiveawayDetails();
            bld = new BanlistDetails();
            scriptHelper = new ScriptHelper(ntrClient);
            scriptHelper.onAutoDisconnect += f1.startAutoDisconnect;
            helper = new RemoteControl(scriptHelper, ntrClient);
            helper.onDumpedPKHeXData += setDumpedData;


            Application.Run(f1);

        }

        public static void createGTSBot(int iP, int iPtF, int iPtFGender, int iPtFLevel, bool bBlacklist, bool bReddit, int iSearchDirection, string waittime, string consoleName, bool useLedySync, string ledySyncIp, string ledySyncPort, int game, bool tradeQueue)
        {
            gtsBot = new GTSBot7(iP, iPtF, iPtFGender, iPtFLevel, bBlacklist, bReddit, iSearchDirection, waittime, consoleName, useLedySync, ledySyncIp, ledySyncPort, game, tradeQueue, helper, PKTable, data, scriptHelper);
            gtsBot.onChangeStatus += f1.ChangeStatus;
            gtsBot.onItemDetails += f1.ReceiveItemDetails;
            Data.GtsBot7 = gtsBot;
        }

        public static void createEggBot(int iP, int game)
        {
            eggBot = new EggBot(iP, game, helper);
        }

        static void setDumpedData(byte[] data)
        {
            f1.dumpedPKHeX.Data = data;
        }

        public static void createPipe(string pipename)
        {
            NamedPipeServerStream server = new NamedPipeServerStream(pipename, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);

            f1.SendConsoleMessage("Awaiting connection from client...");

            server.WaitForConnectionAsync().ContinueWith(t =>
            {
                f1.SendConsoleMessage("Connection Received.");
                StartReadingAsync(server);

                foreach (var pair in ServerList)
                {
                    if (pair.Key == pipename)
                    {
                        pair.Value.Add(server);
                        return;
                    }
                }

                ArrayList newPipeName = new ArrayList
                {
                    server
                };

                ServerList.Add(new KeyValuePair<string, ArrayList>(pipename, newPipeName));
            });
            

        }

        public static void StartReadingAsync(NamedPipeServerStream PipeServer)
        {
            // Debug.WriteLine("Pipe " + FullPipeNameDebug() + " calling ReadAsync");

            // okay we're connected, now immediately listen for incoming buffers
            //
            byte[] pBuffer = new byte[500];
            PipeServer.ReadAsync(pBuffer, 0, 500).ContinueWith(t =>
            {
                // Debug.WriteLine("Pipe " + FullPipeNameDebug() + " finished a read request");

                // before we call the user back, start reading ANOTHER buffer, so the network stack
                // will have something to deliver into and we don't keep it waiting.
                // We're called on the "anonymous task" thread. if we queue another call to
                // the pipe's read, that request goes down into the kernel, onto a different thread
                // and this will be called back again, later. it's not recursive, and perfectly legal.

                int ReadLen = t.Result;
                if (ReadLen == 0)
                {
                    return;
                }

                // lodge ANOTHER read request BEFORE calling the user back. Doing this ensures
                // the read is ready before we call the user back, which may cause a write request to happen,
                // which will zip over to the other end of the pipe, cause a write to happen THERE, and we won't be ready to receive it
                // (perhaps it will stay stuck in a kernel queue, and it's not necessary to do this)
                //
                StartReadingAsync(PipeServer);

                string message = Encoding.Unicode.GetString(pBuffer).TrimEnd('\0').Trim(' ');

                f1.ExecuteCommand(message, false, PipeServer);

            });
        }

    }
}
