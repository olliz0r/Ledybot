using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class GiveawayDetails : Form
    {


        public GiveawayDetails()
        {
            InitializeComponent();

            dgv_Details.DataSource = Program.data.gdetails;
            dgv_Details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            GDInput input = new GDInput();
            if (input.ShowDialog(this) == DialogResult.OK)
            {
                if (input.def != "" || input.specific != "")
                {
                    if (Program.data.giveawayDetails.ContainsKey(input.dex))
                    {
                        Program.data.giveawayDetails.Remove(input.dex);
                        foreach (DataRow row in Program.data.gdetails.Rows)
                        {
                            if ((int)row[0] == input.dex)
                            {
                                Program.data.gdetails.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    if (input.specific != "")
                        Directory.CreateDirectory(input.specific);
                    Program.data.gdetails.Rows.Add(input.dex, input.def, input.specific, input.gender + 1, input.level + 1, input.count, 0);
                    Program.data.giveawayDetails.Add(input.dex, new Tuple<string, string, int, int, int, ArrayList>(input.def, input.specific, input.gender + 1, input.level + 1, input.count, new ArrayList()));
                }
            }
            input.Dispose();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Details.SelectedRows)
            {
                Program.data.giveawayDetails.Remove(int.Parse(row.Cells[0].Value.ToString()));
                for (int i = Program.data.gdetails.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Program.data.gdetails.Rows[i];
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

            if(!Program.data.giveawayDetails.ContainsKey(dexNum))
            {
                Program.data.gdetails.Rows.Add(dexNum, input, Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + "\\", genderIndex + 1, levelIndex + 1, -1, 0);
                Program.data.giveawayDetails.Add(dexNum, new Tuple<string, string, int, int, int, ArrayList>(input, Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + "\\", genderIndex + 1, levelIndex + 1, -1, new ArrayList()));
            }

        }
    }
}
