using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public static GiveawayDetails gd;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new MainForm();
            gd = new GiveawayDetails();
            Application.Run(f1);
        }
    }
}
