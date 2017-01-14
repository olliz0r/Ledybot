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

            if (File.Exists(Application.StartupPath + "\\giveawaydetails.xml"))
            {
                details.ReadXml(Application.StartupPath + "\\giveawaydetails.xml");
            }

            foreach(DataRow row in details.Rows)
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
                if (input.specific != "")
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
                    details.Rows.Add(input.dex, input.def, input.specific, input.gender + 1, input.level + 1, input.count);
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
                        dr.Delete();
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
    }
}
