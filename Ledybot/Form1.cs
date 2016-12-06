using System;
using System.Configuration;
using System.Globalization;
using System.IO;
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
            Program.ntrClient.Connected += connectCheck;
            Program.ntrClient.InfoReady += getGame;
            InitializeComponent();

            ofd_Injection.Title = "Select an EKX/PKX file";
            ofd_Injection.Filter = "Gen 7 pokémon files|*.pk7";
            string path = @Application.StartupPath;
            ofd_Injection.InitialDirectory = path;
        }

        public int pid = 0;

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
            InfoReadyEventArgs args = (InfoReadyEventArgs) e;
            
            string log = args.info;
            if(log.Contains("niji_loc"))
            {
                string splitlog = log.Substring(log.IndexOf(", pname: niji_loc") - 8, log.Length - log.IndexOf(", pname: niji_loc"));
                pid = Convert.ToInt32("0x" + splitlog.Substring(0, 8), 16);
                Program.scriptHelper.write(0x3DFFD0, BitConverter.GetBytes(0xE3A01000), pid);
                MessageBox.Show("Connection Successful!");
            }
        }

        public void connectCheck(object sender, EventArgs e)
        {

            Program.scriptHelper.listprocess();
        }

        Thread workerThread = null;
        Worker workerObject = null;

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (workerThread == null && workerObject == null)
            {
                workerObject = new Worker();
                workerObject.setValues(tb_PokemonToFind.Text, tb_Default.Text, tb_Folder.Text, pid, cb_Spanish.Checked, (int)nud_Dex.Value, cmb_Gender.SelectedIndex, cmb_Levels.SelectedIndex);
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
            if (workerThread != null && workerObject != null)
            {
                workerObject.RequestStop();
                workerThread.Join();

                workerObject = null;
                workerThread = null;
            }
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

            Application.Exit();
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
                txt_FileInjection.Text = ofd_Injection.FileName;
                ofd_Injection.InitialDirectory = Path.GetDirectoryName(ofd_Injection.FileName);
            }
        }

        public uint boxOff = 0x330D9838;

        private void btn_Inject_Click(object sender, EventArgs e)
        {
            byte[] pkmEncrypted = System.IO.File.ReadAllBytes(txt_FileInjection.Text);
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
                Program.ntrClient.disconnect();

                if (!Program.Connected)
                {
                    MessageBox.Show("Disconnection Successful!");
                }
            }
            else
            {
                MessageBox.Show("You are already disconnected!");
            }
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
    }
}
