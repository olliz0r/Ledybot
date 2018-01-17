using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class GiveawayDetails : Form
    {

        public DataTable details = new DataTable();

        public GiveawayDetails()
        {
            InitializeComponent();
            loadDetails();
        }

        private void loadDetails()
        {
            details.TableName = "Giveaway Details";
            details.Columns.Add("Dex Number", typeof(int));
            details.Columns.Add("Specific Path", typeof(string));
            details.Columns.Add("Optional Path", typeof(string));
            details.Columns.Add("Gender Index", typeof(int));
            details.Columns.Add("Level Index", typeof(int));
            details.Columns.Add("Count", typeof(int));
            details.Columns.Add("Traded", typeof(int));

            if (File.Exists(Application.StartupPath + "\\giveawaydetails.xml"))
            {
                details.ReadXml(Application.StartupPath + "\\giveawaydetails.xml");
            }

            foreach (DataRow row in details.Rows)
            {
                Program.f1.giveawayDetails.Add((int)row[0], new Tuple<string, string, int, int, int, ArrayList>(row[1].ToString(), row[2].ToString(), (int)row[3], (int)row[4], (int)row[5], new ArrayList()));
            }

            dgv_Details.DataSource = details;
            dgv_Details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void saveDetails()
        {
            details.WriteXml(Application.StartupPath + "\\giveawaydetails.xml", true);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            GDInput input = new GDInput();
            if (input.ShowDialog(this) == DialogResult.OK)
            {
                if (input.def != "" || input.specific != "")
                {
                    if (Program.f1.giveawayDetails.ContainsKey(input.dex))
                    {
                        Program.f1.giveawayDetails.Remove(input.dex);
                        foreach (DataRow row in details.Rows)
                        {
                            if ((int)row[0] == input.dex)
                            {
                                details.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    if (input.specific != "")
                        Directory.CreateDirectory(input.specific);
                    details.Rows.Add(input.dex, input.def, input.specific, input.gender + 1, input.level + 1, input.count, 0);
                    Program.f1.giveawayDetails.Add(input.dex, new Tuple<string, string, int, int, int, ArrayList>(input.def, input.specific, input.gender + 1, input.level + 1, input.count, new ArrayList()));
                }
            }
            input.Dispose();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Details.SelectedRows)
            {
                Program.f1.giveawayDetails.Remove(int.Parse(row.Cells[0].Value.ToString()));
                for (int i = details.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = details.Rows[i];
                    if (dr[0].ToString() == row.Cells[0].Value.ToString())
                    {
                        dr.Delete();
                        break;
                    }
                }
            }
        }

        public void disableButtons()
        {
            btn_Add.Enabled = false;
            btn_Delete.Enabled = false;
        }

        public void enableButtons()
        {
            btn_Add.Enabled = true;
            btn_Delete.Enabled = true;
        }

        private void dgv_Details_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void dgv_Details_DragDrop(object sender, DragEventArgs e)
        {
            string input = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (File.GetAttributes(input).HasFlag(FileAttributes.Directory))
            {
                
                foreach(string file in Directory.GetFiles(input))
                {
                    if(Path.GetExtension(file) == ".pk7")
                    {
                        addPK7(file);
                    }
                }
            } else if(Path.GetExtension(input) == ".pk7")
            {
                addPK7(input);
            }
        }

        private void addPK7(string input)
        {
            Program.pkhex.Data = System.IO.File.ReadAllBytes(input);
            int dexNum = Program.pkhex.Species;
            int level = Program.PKTable.getLevel(Program.pkhex.Species, (int)Program.pkhex.EXP);
            int gender = Program.pkhex.Gender;
            int levelIndex = 0;
            int genderIndex = 0;
            if (level < 11)
            {
                levelIndex = 0;
            }
            else if (level < 21)
            {
                levelIndex = 1;
            }
            else if (level < 31)
            {
                levelIndex = 2;
            }
            else if (level < 41)
            {
                levelIndex = 3;
            }
            else if (level < 51)
            {
                levelIndex = 4;
            }
            else if (level < 61)
            {
                levelIndex = 5;
            }
            else if (level < 71)
            {
                levelIndex = 6;
            }
            else if (level < 81)
            {
                levelIndex = 7;
            }
            else if (level < 91)
            {
                levelIndex = 8;
            }
            else
            {
                levelIndex = 9;
            }
            switch (gender)
            {
                case 0:
                    genderIndex = 0;
                    break;
                case 1:
                    genderIndex = 1;
                    break;
                default:
                    genderIndex = 0;
                    break;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + "\\");

            if(!Program.f1.giveawayDetails.ContainsKey(dexNum))
            {
                details.Rows.Add(dexNum, input, Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + "\\", genderIndex + 1, levelIndex + 1, -1, 0);
                Program.f1.giveawayDetails.Add(dexNum, new Tuple<string, string, int, int, int, ArrayList>(input, Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + "\\", genderIndex + 1, levelIndex + 1, -1, new ArrayList()));
            }

        }
    }
}
