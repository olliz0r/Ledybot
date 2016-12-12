using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class MainForm : Form
    {

        public delegate void LogDelegate(string l);
        public LogDelegate delLastLog;
        public string lastlog = "";
        public int pid = 0;
        public PKHeX dumpedPKHeX = new PKHeX();

        public uint boxOff = 0x330D9838;
        public uint partyOff = 0x34195E10;
        private uint eggOff = 0x3313EDD8;

        private bool botWorking = false;
        private bool botStop = false;
        private int botNumber = -1;

        public ArrayList blacklist = new ArrayList();

        private GTSBot7 GTSBot7;

        static Dictionary<uint, DataReadyWaiting> waitingForData = new Dictionary<uint, DataReadyWaiting>();

        public MainForm()
        {
            Program.ntrClient.DataReady += handleDataReady;
            Program.ntrClient.Connected += connectCheck;
            Program.ntrClient.InfoReady += getGame;
            delLastLog = new LogDelegate(lastLog);
            InitializeComponent();

            ofd_Injection.Title = "Select an EKX/PKX file";
            ofd_Injection.Filter = "Gen 7 pokémon files|*.pk7";
            string path = @Application.StartupPath;
            ofd_Injection.InitialDirectory = path;

            btn_Disconnect.Enabled = false;
        }

        public void startAutoDisconnect()
        {
            disconnectTimer.Enabled = true;
        }

        public void lastLog(string l)
        {
            lastlog = l;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            string szIp = tb_IP.Text;

            if (!Program.Connected)
            {
                Program.scriptHelper.connect(szIp, 8000);
            }
            else
            {
                MessageBox.Show("You are already connected!");
            }

        }

        public void getGame(object sender, EventArgs e)
        {
            InfoReadyEventArgs args = (InfoReadyEventArgs)e;

            string log = args.info;
            if (log.Contains("niji_loc"))
            {
                string splitlog = log.Substring(log.IndexOf(", pname: niji_loc") - 8, log.Length - log.IndexOf(", pname: niji_loc"));
                pid = Convert.ToInt32("0x" + splitlog.Substring(0, 8), 16);
                Program.helper.pid = pid;
                Program.scriptHelper.write(0x3DFFD0, BitConverter.GetBytes(0xE3A01000), pid);
                MessageBox.Show("Connection Successful!");
            }
        }

        public void setupButtons()
        {
            btn_Connect.Enabled = false;
            btn_Disconnect.Enabled = true;
            btn_Start.Enabled = true;
            btn_Inject.Enabled = true;
            btn_Delete.Enabled = true;
            btn_EggAvailable.Enabled = true;
            btn_EggStart.Enabled = true;
        }

        public void undoButtons()
        {
            btn_Connect.Enabled = true;
            btn_Disconnect.Enabled = false;
            btn_Start.Enabled = false;
            btn_Stop.Enabled = false;
            btn_Inject.Enabled = false;
            btn_Delete.Enabled = false;
            btn_EggAvailable.Enabled = false;
            btn_EggStart.Enabled = false;
            btn_EggStop.Enabled = false;
        }

        public void connectCheck(object sender, EventArgs e)
        {

            Program.scriptHelper.listprocess();
            setupButtons();
            Program.Connected = true;
        }

        static void handleDataReady(object sender, DataReadyEventArgs e)
        { // We move data processing to a separate thread. This way even if processing takes a long time, the netcode doesn't hang.
            DataReadyWaiting args;
            if (waitingForData.TryGetValue(e.seq, out args))
            {
                Array.Copy(e.data, args.data, Math.Min(e.data.Length, args.data.Length));
                Thread t = new Thread(new ParameterizedThreadStart(args.handler));
                t.Start(args);
                waitingForData.Remove(e.seq);
            }
        }

        Thread workerThread = null;
        Worker workerObject = null;

        private async void btn_Start_Click(object sender, EventArgs e)
        {
            btn_Stop.Enabled = true;
            btn_Start.Enabled = false;
            botWorking = true;
            botStop = false;
            botNumber = 3;
            GTSBot7 = new GTSBot7(pid, tb_PokemonToFind.Text, tb_Default.Text, tb_Folder.Text, cb_Spanish.Checked, (int)nud_Dex.Value, cmb_Gender.SelectedIndex, cmb_Levels.SelectedIndex);
            Task<int> Bot = GTSBot7.RunBot();
            int result = await Bot;
            if (botStop)
                result = 8;
            switch (result)
            {
                case 8:
                    MessageBox.Show("Bot stopped by user", "Wonder Trade Bot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    MessageBox.Show("An error has occurred.", "GTS Bot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            botWorking = false;
            botNumber = -1;
        }

        public void AppendListViewItem(string szTrainerName, string szNickname, string fc)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, string, string>(AppendListViewItem), new object[] { szTrainerName, szNickname, fc });
                return;
            }
            string[] row = { DateTime.Now.ToString("h:mm:ss"), szTrainerName, szNickname, fc.Insert(4, "-").Insert(9, "-") };
            var listViewItem = new ListViewItem(row);

            lv_log.Items.Add(listViewItem);
            lv_log.Items[lv_log.Items.Count - 1].EnsureVisible();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            GTSBot7.botstop = true;
            btn_Start.Enabled = true;
            btn_Stop.Enabled = false;
            botStop = true;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            ListViewToCSV(lv_log, AppDomain.CurrentDomain.BaseDirectory + "\\export.csv", true);
            MessageBox.Show("Exported!");
        }

        public static void ListViewToCSV(ListView listView, string filePath, bool includeHidden)
        {
            //make header string
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString());
        }

        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                if (!isColumnNeeded(i))
                    continue;

                if (!isFirstTime)
                    result.Append(",");
                isFirstTime = false;

                result.Append(String.Format("\"{0}\"", columnValue(i)));
            }
            result.AppendLine();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["IP"].Value = tb_IP.Text;
            config.AppSettings.Settings["Deposited"].Value = tb_PokemonToFind.Text;
            config.AppSettings.Settings["Dex"].Value = nud_Dex.Value.ToString();
            config.AppSettings.Settings["Level"].Value = cmb_Levels.SelectedIndex.ToString();
            config.AppSettings.Settings["Spanish"].Value = cb_Spanish.Checked.ToString();
            config.AppSettings.Settings["Default"].Value = tb_Default.Text;
            config.AppSettings.Settings["Folder"].Value = tb_Folder.Text;
            config.AppSettings.Settings["Gender"].Value = cmb_Gender.SelectedIndex.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            tb_IP.Text = config.AppSettings.Settings["IP"].Value;
            tb_PokemonToFind.Text = config.AppSettings.Settings["Deposited"].Value;
            nud_Dex.Value = Int32.Parse(config.AppSettings.Settings["Dex"].Value);
            cmb_Levels.SelectedIndex = Int32.Parse(config.AppSettings.Settings["Level"].Value);
            cb_Spanish.Checked = Boolean.Parse(config.AppSettings.Settings["Spanish"].Value);
            tb_Default.Text = config.AppSettings.Settings["Default"].Value;
            tb_Folder.Text = config.AppSettings.Settings["Folder"].Value;
            cmb_Gender.SelectedIndex = Int32.Parse(config.AppSettings.Settings["Gender"].Value);
        }

        private void btn_BrowseInject_Click(object sender, EventArgs e)
        {
            if (ofd_Injection.ShowDialog() == DialogResult.OK)
            {
                tb_FileInjection.Text = ofd_Injection.FileName;
                ofd_Injection.InitialDirectory = Path.GetDirectoryName(ofd_Injection.FileName);
            }
        }



        private void btn_Inject_Click(object sender, EventArgs e)
        {

            if (tb_FileInjection.Text == "")
            {
                MessageBox.Show("Please select a file!", "Ledybot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] pkmEncrypted = System.IO.File.ReadAllBytes(tb_FileInjection.Text);
            byte[] cloneshort = PKHeX.encryptArray(pkmEncrypted.Take(232).ToArray());
            uint boxIndex = Decimal.ToUInt32((nud_BoxInjection.Value - 1) * 30 + nud_SlotInjection.Value - 1);
            uint count = Decimal.ToUInt32(nud_CountInjection.Value);
            if (boxIndex + count > 32 * 30)
            {
                uint newCount = 32 * 30 - boxIndex;
                count = newCount;
            }

            byte[] dataToWrite = new byte[count * 232];
            for (int i = 0; i < count; i++)
                cloneshort.CopyTo(dataToWrite, i * 232);
            uint offset = boxOff + boxIndex * 232;
            Program.scriptHelper.write(offset, dataToWrite, pid);
            MessageBox.Show("Injection Successful!");
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            if (Program.Connected)
            {

                if (workerThread != null && workerObject != null)
                {
                    workerObject.RequestStop();
                    workerThread.Abort();

                    workerObject = null;
                    workerThread = null;
                }

                Program.scriptHelper.disconnect();

                if (!Program.Connected)
                {
                    MessageBox.Show("Disconnection Successful!");
                    undoButtons();
                }
            }
            else
            {
                MessageBox.Show("You are already disconnected!");
            }
        }

        public void addwaitingForData(uint newkey, DataReadyWaiting newvalue)
        {
            waitingForData.Add(newkey, newvalue);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Program.ntrClient.sendHeartbeatPacket();
            }
            catch (Exception)
            {

            }
        }

        private void nud_BoxInjection_ValueChanged(object sender, EventArgs e)
        {
            nud_CountInjection.Maximum = 32 * 30 - (Decimal.ToUInt32((nud_BoxInjection.Value - 1) * 30 + nud_SlotInjection.Value - 1));
        }

        private void nud_SlotInjection_ValueChanged(object sender, EventArgs e)
        {
            nud_CountInjection.Maximum = 32 * 30 - (Decimal.ToUInt32((nud_BoxInjection.Value - 1) * 30 + nud_SlotInjection.Value - 1));
        }

        private void tp_GTS_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void tp_GTS_DragDrop(object sender, DragEventArgs e)
        {
            string input = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (File.GetAttributes(input).HasFlag(FileAttributes.Directory))
            {
                tb_Folder.Text = input;
            }
            else
            {
                tb_Default.Text = input;
            }

        }

        private void tp_Injection_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void tp_Injection_DragDrop(object sender, DragEventArgs e)
        {
            string input = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (!File.GetAttributes(input).HasFlag(FileAttributes.Directory))
            {
                tb_FileInjection.Text = input;
            }
            else
            {
                MessageBox.Show("Please drag a file!", "Ledybot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            byte[] cloneshort = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x26, 0x06, 0x00, 0x00, 0x7E, 0xE9, 0x71, 0x52, 0xB0, 0x31, 0x42, 0x8E, 0xCC, 0xE2, 0xC5, 0xAF, 0xDB, 0x67, 0x33, 0xFC, 0x2C, 0xEF, 0x5E, 0xFC, 0xC5, 0xCA, 0xD6, 0xEB, 0x3D, 0x99, 0xBC, 0x7A, 0xA7, 0xCB, 0xD6, 0x5D, 0x78, 0x91, 0xA6, 0x27, 0x8D, 0x61, 0x92, 0x16, 0xB8, 0xCF, 0x5D, 0x37, 0x80, 0x30, 0x7C, 0x40, 0xFB, 0x48, 0x13, 0x32, 0xE7, 0xFE, 0xE6, 0xDF, 0x0E, 0x3D, 0xF9, 0x63, 0x29, 0x1D, 0x8D, 0xEA, 0x96, 0x62, 0x68, 0x92, 0x97, 0xA3, 0x49, 0x1C, 0x03, 0x6E, 0xAA, 0x31, 0x89, 0xAA, 0xC5, 0xD3, 0xEA, 0xC3, 0xD9, 0x82, 0xC6, 0xE0, 0x5C, 0x94, 0x3B, 0x4E, 0x5F, 0x5A, 0x28, 0x24, 0xB3, 0xFB, 0xE1, 0xBF, 0x8E, 0x7B, 0x7F, 0x00, 0xC4, 0x40, 0x48, 0xC8, 0xD1, 0xBF, 0xB6, 0x38, 0x3B, 0x90, 0x23, 0xFB, 0x23, 0x7D, 0x34, 0xBE, 0x00, 0xDA, 0x6A, 0x70, 0xC5, 0xDF, 0x84, 0xBA, 0x14, 0xE4, 0xA1, 0x60, 0x2B, 0x2B, 0x38, 0x8F, 0xA0, 0xB6, 0x60, 0x41, 0x36, 0x16, 0x09, 0xF0, 0x4B, 0xB5, 0x0E, 0x26, 0xA8, 0xB6, 0x43, 0x7B, 0xCB, 0xF9, 0xEF, 0x68, 0xD4, 0xAF, 0x5F, 0x74, 0xBE, 0xC3, 0x61, 0xE0, 0x95, 0x98, 0xF1, 0x84, 0xBA, 0x11, 0x62, 0x24, 0x80, 0xCC, 0xC4, 0xA7, 0xA2, 0xB7, 0x55, 0xA8, 0x5C, 0x1C, 0x42, 0xA2, 0x3A, 0x86, 0x05, 0xAD, 0xD2, 0x11, 0x19, 0xB0, 0xFD, 0x57, 0xE9, 0x4E, 0x60, 0xBA, 0x1B, 0x45, 0x2E, 0x17, 0xA9, 0x34, 0x93, 0x2D, 0x66, 0x09, 0x2D, 0x11, 0xE0, 0xA1, 0x74, 0x42, 0xC4, 0x73, 0x19, 0x28, 0x22, 0xF0, 0x43, 0x28, 0x54, 0xA6 };
            uint boxIndex = Decimal.ToUInt32((nud_BoxInjection.Value - 1) * 30 + nud_SlotInjection.Value - 1);
            uint count = Decimal.ToUInt32(nud_CountInjection.Value);
            if (boxIndex + count > 32 * 30)
            {
                uint newCount = 32 * 30 - boxIndex;
                count = newCount;
            }

            byte[] dataToWrite = new byte[count * 232];
            for (int i = 0; i < count; i++)
                cloneshort.CopyTo(dataToWrite, i * 232);
            uint offset = boxOff + boxIndex * 232;
            Program.scriptHelper.write(offset, dataToWrite, pid);
            MessageBox.Show("Deletion Successful!");
        }

        public byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public byte calculateChecksum(byte[] principal)
        {
            byte[] newPrincipal = new byte[4];
            Array.Copy(principal, newPrincipal, 4);
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(newPrincipal);
                byte MyChecksum = (byte)(hash[0] >> 1);
                return MyChecksum;
            }
        }

        public string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }



        private void btn_EggAvailable_Click(object sender, EventArgs e)
        {
            byte[] data = BitConverter.GetBytes(0x01);
            Program.scriptHelper.write(eggOff, data, pid);
        }

        private void btn_EggStart_Click(object sender, EventArgs e)
        {
            workerObject = new Worker();
            workerObject.setValues(1, pid);
            //workerThread = new Thread(workerObject.DoWork);
            workerThread.Start();
            btn_EggStop.Enabled = true;
            btn_EggStart.Enabled = false;
        }

        private void btn_EggStop_Click(object sender, EventArgs e)
        {
            if (workerThread != null && workerObject != null)
            {
                workerObject.RequestStop();
                workerThread.Abort();

                workerObject = null;
                workerThread = null;

                btn_EggStart.Enabled = true;
                btn_EggStop.Enabled = false;
            }
        }

        private void disconnectTimer_Tick(object sender, EventArgs e)
        {
            disconnectTimer.Enabled = false;
            Program.ntrClient.disconnect();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.ntrClient.disconnect();
            Application.Exit();
        }
    }

    public class DataReadyWaiting
    {
        public byte[] data;
        public object arguments;
        public delegate void DataHandler(object data_arguments);
        public DataHandler handler;

        public DataReadyWaiting(byte[] data_, DataHandler handler_, object arguments_)
        {
            this.data = data_;
            this.handler = handler_;
            this.arguments = arguments_;
        }
    }

}
