using System;
using System.IO;
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

        private void btn_BrowseDefault_Click(object sender, EventArgs e)
        {
            OpenFileDialog dg = new OpenFileDialog();
            dg.InitialDirectory = @Application.StartupPath;
            dg.Title = "Select an EKX/PKX file";
            dg.Filter = "Gen 7 pokémon files|*.pk7";
            if(dg.ShowDialog() == DialogResult.OK)
            {
                tb_Default.Text = dg.FileName;
            }
        }

        private void btn_BrowseSpecific_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                tb_Specific.Text = fbd.SelectedPath + "\\";
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            dex = (int) nud_DexNumber.Value;
            def = tb_Default.Text;
            specific = tb_Specific.Text;
            gender = cmb_Gender.SelectedIndex;
            level = cmb_Levels.SelectedIndex;
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
            else
            {
                tb_Default.Text = input;
            }
        }
    }
}
