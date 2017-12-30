using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class GDInput : Form
    {
        public GDInput()
        {
            InitializeComponent();
            cmb_Gender.SelectedIndex = 0;
            cmb_Levels.SelectedIndex = 0;
        }

        public int dex = -1;
        public string def = "";
        public string specific = "";
        public int gender = 0;
        public int level = 0;
        public int count = -1;

        private void btn_BrowseDefault_Click(object sender, EventArgs e)
        {
            OpenFileDialog dg = new OpenFileDialog();
            dg.InitialDirectory = @Application.StartupPath;
            dg.Title = "Select a PK7 file";
            dg.Filter = "Gen 7 pokémon files|*.pk7";
            if (dg.ShowDialog() == DialogResult.OK)
            {
                tb_Default.Text = dg.FileName;
                Program.pkhex.Data = System.IO.File.ReadAllBytes(dg.FileName);
                nud_DexNumber.Value = Program.pkhex.Species;
                int level = Program.PKTable.getLevel(Program.pkhex.Species, (int)Program.pkhex.EXP);
                int gender = Program.pkhex.Gender;
                if (level < 11)
                {
                    cmb_Levels.SelectedIndex = 0;
                }
                else if (level < 21)
                {
                    cmb_Levels.SelectedIndex = 1;
                }
                else if (level < 31)
                {
                    cmb_Levels.SelectedIndex = 2;
                }
                else if (level < 41)
                {
                    cmb_Levels.SelectedIndex = 3;
                }
                else if (level < 51)
                {
                    cmb_Levels.SelectedIndex = 4;
                }
                else if (level < 61)
                {
                    cmb_Levels.SelectedIndex = 5;
                }
                else if (level < 71)
                {
                    cmb_Levels.SelectedIndex = 6;
                }
                else if (level < 81)
                {
                    cmb_Levels.SelectedIndex = 7;
                }
                else if (level < 91)
                {
                    cmb_Levels.SelectedIndex = 8;
                }
                else
                {
                    cmb_Levels.SelectedIndex = 9;
                }
                switch (gender)
                {
                    case 0:
                        cmb_Gender.SelectedIndex = 0;
                        break;
                    case 1:
                        cmb_Gender.SelectedIndex = 1;
                        break;
                    default:
                        cmb_Gender.SelectedIndex = 0;
                        break;
                }

                tb_Specific.Text = Path.GetDirectoryName(dg.FileName) + "\\" + Path.GetFileNameWithoutExtension(dg.FileName) + "\\";
            }
        }

        private void btn_BrowseSpecific_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb_Specific.Text = fbd.SelectedPath + "\\";
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            dex = (int)nud_DexNumber.Value;
            def = tb_Default.Text;
            specific = tb_Specific.Text;
            gender = cmb_Gender.SelectedIndex;
            level = cmb_Levels.SelectedIndex;
            count = (int)nud_Count.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GDInput_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void GDInput_DragDrop(object sender, DragEventArgs e)
        {
            string input = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (File.GetAttributes(input).HasFlag(FileAttributes.Directory))
            {
                tb_Specific.Text = input + "\\";
            }
            else if (Path.GetExtension(input) == ".pk7")
            {
                //byte[] pkmEncrypted = 
                Program.pkhex.Data = System.IO.File.ReadAllBytes(input);
                nud_DexNumber.Value = Program.pkhex.Species;
                int level = Program.PKTable.getLevel(Program.pkhex.Species, (int)Program.pkhex.EXP);
                int gender = Program.pkhex.Gender;
                if (level < 11)
                {
                    cmb_Levels.SelectedIndex = 0;
                }
                else if (level < 21)
                {
                    cmb_Levels.SelectedIndex = 1;
                }
                else if (level < 31)
                {
                    cmb_Levels.SelectedIndex = 2;
                }
                else if (level < 41)
                {
                    cmb_Levels.SelectedIndex = 3;
                }
                else if (level < 51)
                {
                    cmb_Levels.SelectedIndex = 4;
                }
                else if (level < 61)
                {
                    cmb_Levels.SelectedIndex = 5;
                }
                else if (level < 71)
                {
                    cmb_Levels.SelectedIndex = 6;
                }
                else if (level < 81)
                {
                    cmb_Levels.SelectedIndex = 7;
                }
                else if (level < 91)
                {
                    cmb_Levels.SelectedIndex = 8;
                }
                else
                {
                    cmb_Levels.SelectedIndex = 9;
                }
                switch (gender)
                {
                    case 0:
                        cmb_Gender.SelectedIndex = 0;
                        break;
                    case 1:
                        cmb_Gender.SelectedIndex = 1;
                        break;
                    default:
                        cmb_Gender.SelectedIndex = 0;
                        break;
                }
                tb_Default.Text = input;
                tb_Specific.Text = Path.GetDirectoryName(input) + "\\" + Path.GetFileNameWithoutExtension(input) + "\\";


            }
        }
    }
}
