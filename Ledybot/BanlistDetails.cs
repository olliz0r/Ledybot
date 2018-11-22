using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class BanlistDetails : Form
    {

        public BanlistDetails()
        {
            InitializeComponent();

            dgv_Details.DataSource = Program.data.bdetails;
            dgv_Details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            BLInput input = new BLInput();
            if(input.ShowDialog(this) == DialogResult.OK)
            {
                if (input.input != "")
                {
                    if (!Program.data.banlist.Contains(input.input))
                    {
                        Program.data.banlist.Add(input.input);
                        Program.data.bdetails.Rows.Add(input.input);
                    }
                }
            }
            input.Dispose();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Details.SelectedRows)
            {
                Program.data.banlist.Remove(row.Cells[0].Value.ToString());
                for (int i = Program.data.bdetails.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Program.data.bdetails.Rows[i];
                    if (dr[0].ToString() == row.Cells[0].Value.ToString())
                    {
                        dr.Delete();
                        break;
                    }
                }
            }
        }
    }
}
