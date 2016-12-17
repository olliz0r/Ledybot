using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class GiveawayDetails : Form
    {
        public GiveawayDetails()
        {
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            GDInput input = new GDInput();
            if (input.ShowDialog(this) == DialogResult.OK)
            {
                if (input.def != "" && input.specific != "")
                {
                    if (Program.f1.giveawayDetails.ContainsKey(input.dex))
                    {
                        Program.f1.giveawayDetails.Remove(input.dex);
                        foreach (DataGridViewRow row in dgv_Details.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == input.dex.ToString())
                            {
                                dgv_Details.Rows.Remove(row);
                                break;
                            }
                        }
                    }
                    dgv_Details.Rows.Add(input.dex, input.def, input.specific, input.gender + 1, input.level + 1);
                    Program.f1.giveawayDetails.Add(input.dex, new Tuple<string, string, int, int>(input.def, input.specific, input.gender + 1, input.level + 1));
                }
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Details.SelectedRows)
            {
                Program.f1.giveawayDetails.Remove(int.Parse(row.Cells[0].Value.ToString()));
                dgv_Details.Rows.Remove(row);
            }
        }
    }
}
