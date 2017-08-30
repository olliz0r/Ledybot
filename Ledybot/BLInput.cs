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
    public partial class BLInput : Form
    {

        public string input = "";

        public BLInput()
        {
            InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            input += nud_FC1.Value.ToString().PadLeft(4, '0');
            input += nud_FC2.Value.ToString().PadLeft(4, '0');
            input += nud_FC3.Value.ToString().PadLeft(4, '0');
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
