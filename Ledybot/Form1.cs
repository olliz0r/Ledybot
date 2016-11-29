using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            string szIp = tb_IP.Text;
            string szPort = tb_Port.Text;

            int iPort = Convert.ToInt32(szPort);

            if (!Program.Connected)
            {
                Program.Connected = Program.scriptHelper.connect(szIp, iPort);
            }
        }

        Thread workerThread = null;
        Worker workerObject = null;

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (workerThread == null && workerObject == null)
            {
                workerObject = new Worker();
                workerObject.setValues(tb_PokemonToFind.Text, tb_GiveAway.Text, tb_Default.Text, tb_Folder.Text, tb_Level.Text, tb_PID.Text, cb_Spanish.Checked);
                workerThread = new Thread(workerObject.DoWork);
                workerThread.Start();
            }         
        }

        public void AppendListViewItem(string szTrainerName, string szNickname, string szCountry, string szSubCountry)
        {
            if (InvokeRequired)
            {
                 this.Invoke(new Action<string, string, string, string>(AppendListViewItem), new object[] { szTrainerName, szNickname, szCountry, szSubCountry });
                return;
            }
            string[] row = { DateTime.Now.ToString("h:mm:ss"), szTrainerName, szNickname, szCountry, szSubCountry };
            var listViewItem = new ListViewItem(row);

            lv_log.Items.Add(listViewItem);
            lv_log.Items[lv_log.Items.Count - 1].EnsureVisible();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if(workerThread != null && workerObject != null)
            {
                workerObject.RequestStop();
                workerThread.Join();

                workerObject = null;
                workerThread = null;
            }
        }
    }
}
