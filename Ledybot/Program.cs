using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    static class Program
    {
        public volatile static NTRClient ntrClient;
        public volatile static ScriptHelper scriptHelper;
        public static Boolean Connected = false;
        public volatile static MainForm f1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ntrClient = new NTRClient();
            scriptHelper = new ScriptHelper();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new MainForm();
            Application.Run(f1);
        }
    }
}
