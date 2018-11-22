using System;
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
        public static bool Connected = false;
        public static MainForm f1;
        public static LookupTable PKTable;
        public static PKHeX pkhex;
        public static GiveawayDetails gd;
        public static BanlistDetails bld;

        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ntrClient = new NTR();
            ntrClient.onLogArrival += log;
            ntrClient.Disconnected += disconnected;
            scriptHelper = new ScriptHelper(ntrClient);
            scriptHelper.onAutoDisconnect += startAutoDisconnect;
            helper = new RemoteControl(scriptHelper, ntrClient);
            helper.onWaitingForData += addwaitingfordata;
            helper.onWriteLastLog += writeLastLog;
            helper.onDumpedPKHeXData += setDumpedData;
            PKTable = new LookupTable();
            pkhex = new PKHeX();
            data = new Data();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new MainForm();
            gd = new GiveawayDetails();
            bld = new BanlistDetails();
            Application.Run(f1);
        }

        public static void createGTSBot(int iP, int iPtF, int iPtFGender, int iPtFLevel, bool bBlacklist, bool bReddit, int iSearchDirection, string waittime, string consoleName, bool useLedySync, string ledySyncIp, string ledySyncPort, int game, bool useLedybotTV, string ledybotTVIp, string ledybotTVPort)
        {
            gtsBot = new GTSBot7(iP, iPtF, iPtFGender, iPtFLevel, bBlacklist, bReddit, iSearchDirection, waittime, consoleName, useLedySync, ledySyncIp, ledySyncPort, game, useLedybotTV, ledybotTVIp, ledybotTVPort, helper, PKTable, data, scriptHelper);
            gtsBot.onChangeStatus += ChangeStatus;
            gtsBot.onItemDetails += appendItems;
        }

        public static void createEggBot(int iP, int game)
        {
            eggBot = new EggBot(iP, game, helper);
        }

        static void log(string msg)
        {
            try
            {
                f1.BeginInvoke(f1.delLastLog, msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void disconnected()
        {
            Connected = false;
        }

        static void startAutoDisconnect()
        {
            f1.startAutoDisconnect();
        }

        static void addwaitingfordata(uint newkey, DataReadyWaiting newvalue)
        {
            f1.addwaitingForData(newkey, newvalue);
        }

        static void writeLastLog(string str)
        {
            f1.lastlog = str;
        }

        static void setDumpedData(byte[] data)
        {
            f1.dumpedPKHeX.Data = data;
        }

        static void ChangeStatus(string msg)
        {
            f1.ChangeStatus(msg);
        }

        static void appendItems(string szTrainerName, string szNickname, string szCountry, string szSubRegion,
            string szSent, string fc, string page, string index)
        {
            f1.AppendListViewItem(szTrainerName, szNickname, szCountry, szSubRegion, szSent, fc, page, index);
        }
    }
}
