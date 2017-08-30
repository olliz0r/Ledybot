using System;
using System.Windows.Forms;

namespace Ledybot
{
    static class Program
    {
        public static NTR ntrClient;
        public static ScriptHelper scriptHelper;
        public static RemoteControl helper;
        public static Boolean Connected = false;
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
            scriptHelper = new ScriptHelper();
            helper = new RemoteControl();
            PKTable = new LookupTable();
            pkhex = new PKHeX();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new MainForm();
            gd = new GiveawayDetails();
            bld = new BanlistDetails();
            Application.Run(f1);
        }
    }
}
