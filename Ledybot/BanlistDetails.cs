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
        public DataTable details = new DataTable();

        public BanlistDetails()
        {
            InitializeComponent();
            loadDetails();
        }

        private void loadDetails()
        {
            details.TableName = "Banlist";
            details.Columns.Add("FC", typeof(string));

            if (File.Exists(Application.StartupPath + "\\banlistdetails.xml"))
            {
                details.ReadXml(Application.StartupPath + "\\banlistdetails.xml");
            }

            foreach (DataRow row in details.Rows)
            {
                Program.f1.banlist.Add(row[0]);
            }

            dgv_Details.DataSource = details;
            dgv_Details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void saveDetails()
        {
            details.WriteXml(Application.StartupPath + "\\banlistdetails.xml", true);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            BLInput input = new BLInput();
            if(input.ShowDialog(this) == DialogResult.OK)
            {
                if (input.input != "")
                {
                    if (!Program.f1.banlist.Contains(input.input))
                    {
                        Program.f1.banlist.Add(input.input);
                        details.Rows.Add(input.input);
                    }
                }
            }
            input.Dispose();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Details.SelectedRows)
            {
                Program.f1.banlist.Remove(row.Cells[0].Value.ToString());
                for (int i = details.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = details.Rows[i];
                    if (dr[0].ToString() == row.Cells[0].Value.ToString())
                        dr.Delete();
                        break;
                }
            }
        }
    }
}
