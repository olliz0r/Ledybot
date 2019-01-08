using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LedyLib;


namespace Ledybot
{
    public partial class MainForm : Form
    {

        public int pid = 0;
        public int game = 0;
        public PKHeX dumpedPKHeX = new PKHeX();

        public uint wcOff;

        private bool tradeQueue = false;

        private uint eggOff;

        private bool botWorking = false;
        private bool botStop = false;
        private int botNumber = -1;

        private Form touchfrm;

        public MainForm()
        {
            Program.ntrClient.Connected += connectCheck;
            Program.ntrClient.InfoReady += getGame;
            InitializeComponent();
            ofd_Injection.Title = "Select an EKX/PKX file";
            ofd_Injection.Filter = "Gen 7 pokémon files|*.pk7";
            ofd_WCInjection.Title = "Select a WC7 file";
            ofd_WCInjection.Filter = "Gen 7 wondercard files|*.wc7";
            string path = @Application.StartupPath;
            ofd_Injection.InitialDirectory = path;
            ofd_WCInjection.InitialDirectory = path;
            btn_Disconnect.Enabled = false;
            this.combo_pkmnList.Items.AddRange(Program.PKTable.Species7);

            touchfrm = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedSingle
            };
            touchfrm.ClientSize = new System.Drawing.Size(320, 240);
            touchfrm.MouseDown += Touchfrm_MouseDown;
            touchfrm.MouseMove += Touchfrm_MouseMove;
            touchfrm.MouseUp += Touchfrm_MouseUp;
            touchfrm.Show(this);
        }

        bool touchtouch = false;

        private void Touchfrm_MouseMove(object sender, MouseEventArgs e)
        {
            if (touchtouch && Program.ntrClient.isConnected)
                Program.helper.holdtouch(Math.Min(319, Math.Max(0, e.X)),
                                         Math.Min(239, Math.Max(0, e.Y)));
        }

        private void Touchfrm_MouseDown(object sender, MouseEventArgs e)
        {
            if (Program.ntrClient.isConnected)
            {
                Program.helper.holdtouch(e.X, e.Y);
                touchtouch = true;
            }
            else
            {
                MessageBox.Show("You haven't even connected yet, you dummy!");
                touchtouch = false;
            }
        }

        private void Touchfrm_MouseUp(object sender, MouseEventArgs e)
        {
            touchtouch = false;
            if (Program.ntrClient.isConnected)
                Program.helper.freetouch();
        }

        public async void ExecuteCommand(string command, bool button, NamedPipeServerStream stream)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, bool, NamedPipeServerStream>(ExecuteCommand), command, button, stream);
                return;
            }

            string[] commStrings = command.Split(' ');
			switch (commStrings[0].Trim('\0'))
			{
				case "connect3ds":

					string szIp = tb_IP.Text;
					if (!Program.ntrClient.isConnected)
					{
						Program.scriptHelper.connect(szIp, 8000);
					}
					else
					{
						if (button)
							MessageBox.Show("You are already connected!");
						else
						{
							string msg = "command:connect3ds Ledybot is already connected.";
							Writer(stream, msg);
						}

					}
					break;
				case "disconnect3ds":
					if (Program.ntrClient.isConnected)
					{
						Program.gtsBot?.RequestStop();

						Program.eggBot?.RequestStop();

						Program.scriptHelper.disconnect();

						if (Program.ntrClient.isConnected) return;
						if (button)
							MessageBox.Show("Successfully disconnected!");
						else
						{
							string msg = "command:disconnect3ds Ledybot successfully disconnected.";
							Writer(stream, msg);
						}
						undoButtons();
					}
					else
					{
						if (button)
							MessageBox.Show("You are already disconnected!");
						else
						{
							string msg = "command:disconnect3ds Ledybot successfully disconnected.";
							Writer(stream, msg);
						}
						undoButtons();
					}
					break;
				case "startgtsbot":
					if (!Program.data.giveawayDetails.Any())
					{
						if (button)
							MessageBox.Show("No details are set!", "GTS Bot", MessageBoxButtons.OK,
								MessageBoxIcon.Error);
						else
						{
							string msg = "command:startgtsbot No details are set!";
							Writer(stream, msg);
						}
						return;
					}
					btn_Stop.Enabled = true;
					btn_Start.Enabled = false;
					Program.gd.disableButtons();
					botWorking = true;
					botStop = false;
					botNumber = 3;

					int tradeDirection = 0;
					if (rb_frontfpo.Checked)
					{
						tradeDirection = 1;
					}
					else if (rb_front.Checked)
					{
						tradeDirection = 2;
					}
					Program.createGTSBot(pid, combo_pkmnList.SelectedIndex + 1, combo_gender.SelectedIndex, combo_levelrange.SelectedIndex, cb_Blacklist.Checked, cb_Reddit.Checked, tradeDirection, tb_waittime.Text, tb_consoleName.Text, cb_UseLedySync.Checked, tb_LedySyncIP.Text, tb_LedySyncPort.Text, game, tradeQueue);
					Task<int> Bot = Program.gtsBot.RunBot();
					if (!button)
					{
						string msg2 = "command:startgtsbot Bot started!";
						Writer(stream, msg2);
					}
					SendConsoleMessage("Bot started.");
					int result = await Bot;
					if (botStop)
						result = 8;
					switch (result)
					{
						case 1:
							SendConsoleMessage("All Pokemon Traded.");
							string msg3 = "msg:info All Pokemon Traded.";
							Writer(stream, msg3);
							break;
						case 8:
							SendConsoleMessage("Bot Stopped by User");
							string msg4 = "msg:info Bot Stopped by User.";
							Writer(stream, msg4);
							break;
						default:
							SendConsoleMessage("An error has occured.");
							string msg5 = "msg:info An error has occured.";
							Writer(stream, msg5);
							break;
					}


					Program.gd.enableButtons();
					btn_Stop.Enabled = false;
					btn_Start.Enabled = true;
					botWorking = false;
					botNumber = -1;
					break;
				case "stopgtsbot":
					Program.gtsBot.RequestStop();
                    btn_Start.Enabled = true;
                    btn_Stop.Enabled = false;
                    botStop = true;
					SendConsoleMessage("Requested Bot Stop.");
					string msg6 = "command:stopgtsbot Requested to stop the bot.";
					Writer(stream, msg6);
					break;
                case "refresh":
                    switch (commStrings.Length)
                    {
                        case 1:
                            Program.data.refreshDetails();
                            string msg7 = "command:refresh Ban list and giveaway details refreshed.";
                            Writer(stream, msg7);
                            break;
                        case 2:
                            switch (commStrings[1])
                            {
                                case "details":
                                    Program.data.refreshDetails(true, false);
                                    string msg8 = "command:refresh Giveaway details refreshed.";
                                    Writer(stream, msg8);
                                    break;
                                case "bans":
                                    Program.data.refreshDetails(false);
                                    string msg9 = "command:refresh Ban list refreshed.";
                                    Writer(stream, msg9);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 3:
                            switch (commStrings[1])
                            {
                                case "details":
                                    Program.data.refreshDetails(true, false, commStrings[2]);
                                    string msg10 = "command:refresh Giveaway details refreshed from " + commStrings[2] + ".";
                                    Writer(stream, msg10);
                                    break;
                                case "bans":
                                    Program.data.refreshDetails(false, true, "", commStrings[2]);
                                    string msg11 = "command:refresh Ban list refreshed from " + commStrings[2] + ".";
                                    Writer(stream, msg11);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "togglequeue":
                    tradeQueue = !tradeQueue;
                    if (tradeQueue)
                    {
                        SendConsoleMessage("Trade Queue Enabled.");
                        string msg12 = "command:togglequeue Trade Queue Enabled.";
                        Writer(stream, msg12);
                    }
                    else
                    {
                        SendConsoleMessage("Trade Queue Disabled.");
                        string msg13 = "command:togglequeue Trade Queue Disabled.";
                        Writer(stream, msg13);
                    }
                    break;
                case "trade":
                    if (tradeQueue)
                    {
                        if (commStrings.Length == 3)
                        {
                            Program.data.AddToQueue(int.Parse(commStrings[2]), commStrings[1]);
                            SendConsoleMessage("Added FC " + commStrings[1] + " to queue with deposit of " + commStrings[2]);
                            string msg15 = "command:trade Added FC " + commStrings[1] + " to queue with deposit of " + commStrings[2];
                            Writer(stream, msg15);
                            break;
                        }
                        string msg16 = "command:trade Invalid Use. Usage: trade {fc} {dex}";
                        Writer(stream, msg16);
                    }
                    else
                    {
                        string msg14 = "command:trade Trade Queue is not enabled.";
                        Writer(stream, msg14);
                    }
                    break;
                case "remove":
                    if (tradeQueue)
                    {
                        if (commStrings.Length == 2)
                        {
                            Program.data.RemoveFromQueue(int.Parse(commStrings[1]));
                            SendConsoleMessage("Removed trade of index " + commStrings[1] + " from queue");
                            string msg18 = "command:remove Removed trade of index " + commStrings[1] + " from queue.";
                            Writer(stream, msg18);
                            break;
                        }
                        string msg19 = "command:remove Invalid Use. Usage: remove {index}";
                        Writer(stream, msg19);
                    }
                    else
                    {
                        string msg17 = "command:remove Trade Queue is not enabled.";
                        Writer(stream, msg17);
                    }

                    break;

                case "viewqueue":
                    if (tradeQueue)
                    {
                        if (commStrings.Length == 1)
                        {
                            string msg21 = "command:viewqueue 1 " + Program.data.ViewQueue(1);
                            Writer(stream, msg21);
                            break;
                        }
                        if (commStrings.Length == 2)
                        {
                            string msg22 = "command:viewqueue " + commStrings[1] + " " + Program.data.ViewQueue(int.Parse(commStrings[1]));
                            Writer(stream, msg22);
                            break;
                        }
                        string msg19 = "command:remove Invalid Use. Usage: viewqueue {page}";
                        Writer(stream, msg19);
                    }
                    else
                    {
                        string msg20 = "command:viewqueue Trade Queue is not enabled.";
                        Writer(stream, msg20);
                    }
                    break;
                default:
                    break;
            }
        }

        public void startAutoDisconnect()
        {
            disconnectTimer.Enabled = true;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            ExecuteCommand("connect3ds", true, null);
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
                Program.scriptHelper.write(0x3E14C0, BitConverter.GetBytes(0xE3A01000), pid);
                game = 0;
                SendConsoleMessage("Connection Successful!");

                Program.helper.boxOff = 0x330D9838;
                wcOff = 0x331397E4;
                Program.helper.partyOff = 0x34195E10;
                eggOff = 0x3313EDD8;

            }
            else if (log.Contains("momiji"))
            {
                string splitlog = log.Substring(log.IndexOf(", pname:   momiji") - 8, log.Length - log.IndexOf(", pname:   momiji"));
                pid = Convert.ToInt32("0x" + splitlog.Substring(0, 8), 16);
                Program.helper.pid = pid;
                Program.scriptHelper.write(0x3F3424, BitConverter.GetBytes(0xE3A01000), pid); // Ultra Sun  // NFC ON: E3A01001 NFC OFF: E3A01000
                Program.scriptHelper.write(0x3F3428, BitConverter.GetBytes(0xE3A01000), pid); // Ultra Moon // NFC ON: E3A01001 NFC OFF: E3A01000
                game = 1;
                SendConsoleMessage("Connection Successful!");

                Program.helper.boxOff = 0x33015AB0;
                wcOff = 0x33075BF4;
                Program.helper.partyOff = 0x33F7FA44;
                eggOff = 0x3307B1E8;
            }
        }

        public void setupButtons()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(setupButtons));
                return;
            }
            btn_Connect.Enabled = false;
            btn_Disconnect.Enabled = true;
            btn_Start.Enabled = true;
            btn_Inject.Enabled = true;
            btn_Delete.Enabled = true;
            btn_WCInject.Enabled = true;
            btn_WCDelete.Enabled = true;
            btn_EggAvailable.Enabled = true;
            btn_EggStart.Enabled = true;
        }

        public void undoButtons()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(undoButtons));
                return;
            }
            btn_Connect.Enabled = true;
            btn_Disconnect.Enabled = false;
            btn_Start.Enabled = false;
            btn_Stop.Enabled = false;
            btn_Inject.Enabled = false;
            btn_Delete.Enabled = false;
            btn_WCInject.Enabled = false;
            btn_WCDelete.Enabled = false;
            btn_EggAvailable.Enabled = false;
            btn_EggStart.Enabled = false;
            btn_EggStop.Enabled = false;
        }

        public void connectCheck(object sender, EventArgs e)
        {

            Program.scriptHelper.listprocess();
            setupButtons();
            Program.ntrClient.isConnected = true;

            List<NamedPipeServerStream> needToRemove = new List<NamedPipeServerStream>();

            foreach (var list in Program.ServerList)
            {
                foreach (NamedPipeServerStream server in list.Value)
                {
                    try
                    {
                        String msg = "command:connect3ds Ledybot is connected.";
                        Writer(server, msg);
                    }
                    catch
                    {
                        server.Disconnect();
                        server.Dispose();
                        needToRemove.Add(server);
                    }
                }

                foreach (var server in needToRemove)
                {
                    list.Value.Remove(server);
                }
                needToRemove.Clear();

            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            ExecuteCommand("startgtsbot", true, null);
        }

        public void ReceiveItemDetails(object sender, ItemDetailsEventArgs e)
        {
            AppendListViewItem(e.szTrainerName, e.szNickname, e.szCountry, e.szSubRegion, e.szSent, e.fc, e.page, e.index);
        }

        public void AppendListViewItem(string szTrainerName, string szNickname, string szCountry, string szSubRegion, string szSent, string fc, string page, string index)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, string, string, string, string, string, string, string>(AppendListViewItem), new object[] { szTrainerName, szNickname, szCountry, szSubRegion, szSent, fc, page, index });
                return;
            }
            string[] row = { DateTime.Now.ToString("h:mm:ss"), szTrainerName, szNickname, szCountry, szSubRegion, szSent, fc.Insert(4, "-").Insert(9, "-"), page, index };
            var listViewItem = new ListViewItem(row);

            List<NamedPipeServerStream> needToRemove = new List<NamedPipeServerStream>();

            foreach (var list in Program.ServerList)
            {
                foreach (NamedPipeServerStream server in list.Value)
                {
                    try
                    {
                        string msg = "msg:trade " + string.Join("|", szTrainerName, szNickname, szCountry, szSubRegion, szSent, fc, page, index);
                        Writer(server, msg);
                    }
                    catch
                    {
                        server.Disconnect();
                        server.Dispose();
                        needToRemove.Add(server);
                    }
                }

                foreach (var server in needToRemove)
                {
                    list.Value.Remove(server);
                }
                needToRemove.Clear();

            }

            lv_log.Items.Add(listViewItem);
            lv_log.Items[lv_log.Items.Count - 1].EnsureVisible();
        }

        public void ChangeStatus(string szNewStatus)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(ChangeStatus), new object[] { szNewStatus });
                return;
            }
            this.rt_status.Text = "Bot Status: " + szNewStatus;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            ExecuteCommand("stopgtsbot", true, null);
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

        private void ImportCSV()
        {
            FileStream srcFS;
            srcFS = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\export.csv", FileMode.Open);
            StreamReader srcSR = new StreamReader(srcFS, System.Text.Encoding.Default);
            srcSR.ReadLine();
            do
            {
                string ins = srcSR.ReadLine();
                if (ins != null)
                {
                    string[] columns = ins.Replace("\"", "").Split(',');

                    ListViewItem lvi = new ListViewItem(columns[0]);

                    for (int i = 1; i < columns.Count(); i++)
                    {
                        lvi.SubItems.Add(columns[i]);
                    }

                    lv_log.Items.Add(lvi);
                }
                else break;
            } while (true);
            srcSR.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.IP = tb_IP.Text;
            Properties.Settings.Default.Blacklist = cb_Blacklist.Checked;
            Properties.Settings.Default.Reddit = cb_Reddit.Checked;
            Properties.Settings.Default.Thread = tb_thread.Text;
            Properties.Settings.Default.Subreddit = tb_Subreddit.Text;
            Properties.Settings.Default.RBFront = rb_front.Checked;
            Properties.Settings.Default.RBBackFPO = rb_frontfpo.Checked;
            Properties.Settings.Default.RBBack = rb_back.Checked;
            Properties.Settings.Default.Waittime = tb_waittime.Text;
            Properties.Settings.Default.ConsoleName = tb_consoleName.Text;
            Properties.Settings.Default.UseLedySync = cb_UseLedySync.Checked;
            Properties.Settings.Default.LedySyncIP = tb_LedySyncIP.Text;
            Properties.Settings.Default.LedySyncPort = tb_LedySyncPort.Text;
            Properties.Settings.Default.DepositedIndex = combo_pkmnList.SelectedIndex;
            Properties.Settings.Default.DepositedGender = combo_gender.SelectedIndex;
            Properties.Settings.Default.DepositedLevel = combo_levelrange.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tb_IP.Text = Properties.Settings.Default.IP;
            cb_Blacklist.Checked = Properties.Settings.Default.Blacklist;
            cb_Reddit.Checked = Properties.Settings.Default.Reddit;
            tb_thread.Text = Properties.Settings.Default.Thread;
            tb_Subreddit.Text = Properties.Settings.Default.Subreddit;
            rb_front.Checked = Properties.Settings.Default.RBFront;
            rb_frontfpo.Checked = Properties.Settings.Default.RBBackFPO;
            rb_back.Checked = Properties.Settings.Default.RBBack;
            tb_waittime.Text = Properties.Settings.Default.Waittime;
            tb_consoleName.Text = Properties.Settings.Default.ConsoleName;
            cb_UseLedySync.Checked = Properties.Settings.Default.UseLedySync;
            tb_LedySyncIP.Text = Properties.Settings.Default.LedySyncIP;
            tb_LedySyncPort.Text = Properties.Settings.Default.LedySyncPort;
            combo_pkmnList.SelectedIndex = Properties.Settings.Default.DepositedIndex;
            combo_gender.SelectedIndex = Properties.Settings.Default.DepositedGender;
            combo_levelrange.SelectedIndex = Properties.Settings.Default.DepositedLevel;
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
            uint offset = Program.helper.boxOff + boxIndex * 232;
            Program.scriptHelper.write(offset, dataToWrite, pid);
            MessageBox.Show("Injection Successful!");
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            ExecuteCommand("disconnect3ds", true, null);
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
            uint offset = Program.helper.boxOff + boxIndex * 232;
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

        private async void btn_EggStart_Click(object sender, EventArgs e)
        {
            Program.createEggBot(pid, game);
            btn_EggStop.Enabled = true;
            btn_EggStart.Enabled = false;
            await Program.eggBot.RunBot();
        }

        private void btn_EggStop_Click(object sender, EventArgs e)
        {
            Program.eggBot.RequestStop();
            btn_EggStart.Enabled = true;
            btn_EggStop.Enabled = false;
        }

        private void disconnectTimer_Tick(object sender, EventArgs e)
        {
            disconnectTimer.Enabled = false;
            Program.ntrClient.disconnect();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.data.saveDetails();
            Program.ntrClient.disconnect();
            Application.Exit();
        }

        private void btn_ShowPaths_Click(object sender, EventArgs e)
        {
            Program.gd.ShowDialog(this);
        }

        private void btn_Banlist_Click(object sender, EventArgs e)
        {
            Program.bld.ShowDialog(this);
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            ImportCSV();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            lv_log.Items.Clear();
        }

        private void btn_WCInject_Click(object sender, EventArgs e)
        {
            if (tb_WCInjection.Text == "")
            {
                MessageBox.Show("Please select a file!", "Ledybot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] wc = System.IO.File.ReadAllBytes(tb_WCInjection.Text);
            uint slotIndex = Decimal.ToUInt32(nud_SlotWCInjection.Value) - 1;

            uint offset = wcOff + slotIndex * 264;
            Program.scriptHelper.write(offset, wc, pid);
            MessageBox.Show("Injection Successful!");
        }

        private void btn_WCDelete_Click(object sender, EventArgs e)
        {
            byte[] cloneshort = new byte[264];
            for (int i = 0; i < cloneshort.Length; i++)
            {
                cloneshort[i] = 0x0;
            }
            uint slotIndex = Decimal.ToUInt32(nud_SlotWCInjection.Value) - 1;

            uint offset = wcOff + slotIndex * 264;
            Program.scriptHelper.write(offset, cloneshort, pid);
            MessageBox.Show("Deletion Successful!");
        }

        private void btn_BrowseWCInject_Click(object sender, EventArgs e)
        {
            if (ofd_WCInjection.ShowDialog() == DialogResult.OK)
            {
                tb_WCInjection.Text = ofd_WCInjection.FileName;
                ofd_WCInjection.InitialDirectory = Path.GetDirectoryName(ofd_WCInjection.FileName);
            }
        }

        private void tb_LedySyncIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_SendCommand_Click(object sender, EventArgs e)
        {
            consolePress();
        }


        

        public void Writer(NamedPipeServerStream stream,String str)
        {
            Tuple<String, NamedPipeServerStream> writerData = Tuple.Create(str,stream);
            ParameterizedThreadStart thready = new ParameterizedThreadStart(writeThread);
            Thread  newThread = new Thread(thready);
            //rtb_Console.AppendText("\n" + "Writing Start");
            newThread.Start(writerData);
            //rtb_Console.AppendText("\n" + "Writing End");

        }

        public void writeThread(object x)
		{
			try //catches any exceptions that may crash the pipe
			{
				string str = ((Tuple<String, NamedPipeServerStream>)x).Item1;
				NamedPipeServerStream stream = ((Tuple<String, NamedPipeServerStream>)x).Item2;
				if (stream != null) //solves the bot crashing when sending to a null pipe
				{
					byte[] bytes = Encoding.Unicode.GetBytes(str);
					stream.Write(bytes, 0, bytes.Length);
					stream.WaitForPipeDrain();
					Console.WriteLine("Wrote: \"{0}\"", str);
				}
				else
				{
					Console.WriteLine("\r\n Error nothing worked PIPE ERROR \r\n");
				}
			}
			catch
			{
				Console.WriteLine("\r\n Bad pipe. Bad BAD! *SPANKS* \r\n");
			}
        }

        private void tb_ConsoleCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                consolePress();
            }
        }

        public void consolePress()
        {
            if (tb_ConsoleCommand.Text.Length != 0){
                string[] comStrings = tb_ConsoleCommand.Text.Split(' ');

                switch (comStrings[0])
                {
                    case "connect":
                        if (comStrings.Length == 2)
                        {
                            Program.createPipe(comStrings[1]);
                        }
                        else
                            rtb_Console.AppendText("\nCommand Usage: conect {pipename}");
                        break;
                }
            }
        }


        public void SendConsoleMessage(string message)
        {
            if (rtb_Console.InvokeRequired)
            {
                this.Invoke(new Action<string>(SendConsoleMessage), message);
                return;
            }
            rtb_Console.AppendText("\n" + message);
        }


    }


}
