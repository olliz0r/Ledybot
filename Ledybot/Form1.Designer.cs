namespace Ledybot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tb_IP = new System.Windows.Forms.TextBox();
            this.tb_Port = new System.Windows.Forms.TextBox();
            this.l_dummy = new System.Windows.Forms.Label();
            this.tb_PID = new System.Windows.Forms.TextBox();
            this.lb_dummy2 = new System.Windows.Forms.Label();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.tb_PokemonToFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Default = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Folder = new System.Windows.Forms.TextBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.lv_log = new System.Windows.Forms.ListView();
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Trainer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NickName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Country = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SubCountry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Export = new System.Windows.Forms.Button();
            this.cb_Spanish = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nud_Dex = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_Gender = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmb_Levels = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dex)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_IP
            // 
            this.tb_IP.Location = new System.Drawing.Point(13, 13);
            this.tb_IP.Name = "tb_IP";
            this.tb_IP.Size = new System.Drawing.Size(100, 20);
            this.tb_IP.TabIndex = 0;
            this.tb_IP.Text = "192.168.1.19";
            // 
            // tb_Port
            // 
            this.tb_Port.Location = new System.Drawing.Point(135, 13);
            this.tb_Port.Name = "tb_Port";
            this.tb_Port.Size = new System.Drawing.Size(34, 20);
            this.tb_Port.TabIndex = 1;
            this.tb_Port.Text = "8000";
            // 
            // l_dummy
            // 
            this.l_dummy.AutoSize = true;
            this.l_dummy.Location = new System.Drawing.Point(119, 16);
            this.l_dummy.Name = "l_dummy";
            this.l_dummy.Size = new System.Drawing.Size(10, 13);
            this.l_dummy.TabIndex = 2;
            this.l_dummy.Text = ":";
            // 
            // tb_PID
            // 
            this.tb_PID.Location = new System.Drawing.Point(191, 13);
            this.tb_PID.Name = "tb_PID";
            this.tb_PID.Size = new System.Drawing.Size(19, 20);
            this.tb_PID.TabIndex = 3;
            this.tb_PID.Text = "2c";
            // 
            // lb_dummy2
            // 
            this.lb_dummy2.AutoSize = true;
            this.lb_dummy2.Location = new System.Drawing.Point(175, 16);
            this.lb_dummy2.Name = "lb_dummy2";
            this.lb_dummy2.Size = new System.Drawing.Size(10, 13);
            this.lb_dummy2.TabIndex = 4;
            this.lb_dummy2.Text = "-";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(216, 11);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(75, 23);
            this.btn_Connect.TabIndex = 5;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // tb_PokemonToFind
            // 
            this.tb_PokemonToFind.Location = new System.Drawing.Point(12, 67);
            this.tb_PokemonToFind.Name = "tb_PokemonToFind";
            this.tb_PokemonToFind.Size = new System.Drawing.Size(100, 20);
            this.tb_PokemonToFind.TabIndex = 6;
            this.tb_PokemonToFind.Text = "Ledyba";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Deposited Pokemon:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Default .pk7:";
            // 
            // tb_Default
            // 
            this.tb_Default.Location = new System.Drawing.Point(12, 176);
            this.tb_Default.Name = "tb_Default";
            this.tb_Default.Size = new System.Drawing.Size(354, 20);
            this.tb_Default.TabIndex = 13;
            this.tb_Default.Text = "D:/Ledybot/pk7s/Ditto.pk7";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Specific .pk7 folder:";
            // 
            // tb_Folder
            // 
            this.tb_Folder.Location = new System.Drawing.Point(12, 219);
            this.tb_Folder.Name = "tb_Folder";
            this.tb_Folder.Size = new System.Drawing.Size(354, 20);
            this.tb_Folder.TabIndex = 15;
            this.tb_Folder.Text = "D:/Ledybot/pk7s";
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(161, 121);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(58, 23);
            this.btn_Start.TabIndex = 16;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(225, 121);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(58, 23);
            this.btn_Stop.TabIndex = 17;
            this.btn_Stop.Text = "Stop";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // lv_log
            // 
            this.lv_log.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Time,
            this.Trainer,
            this.NickName,
            this.Country,
            this.SubCountry});
            this.lv_log.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_log.Location = new System.Drawing.Point(13, 246);
            this.lv_log.Name = "lv_log";
            this.lv_log.Size = new System.Drawing.Size(353, 262);
            this.lv_log.TabIndex = 18;
            this.lv_log.UseCompatibleStateImageBehavior = false;
            this.lv_log.View = System.Windows.Forms.View.Details;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 40;
            // 
            // Trainer
            // 
            this.Trainer.Text = "Trainer";
            // 
            // NickName
            // 
            this.NickName.Text = "Name";
            this.NickName.Width = 50;
            // 
            // Country
            // 
            this.Country.Text = "Country";
            // 
            // SubCountry
            // 
            this.SubCountry.Text = "SubCountry";
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(12, 515);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(354, 23);
            this.btn_Export.TabIndex = 19;
            this.btn_Export.Text = "Export";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // cb_Spanish
            // 
            this.cb_Spanish.AutoSize = true;
            this.cb_Spanish.Location = new System.Drawing.Point(289, 125);
            this.cb_Spanish.Name = "cb_Spanish";
            this.cb_Spanish.Size = new System.Drawing.Size(64, 17);
            this.cb_Spanish.TabIndex = 20;
            this.cb_Spanish.Text = "Spanish";
            this.cb_Spanish.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(119, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Dex:";
            // 
            // nud_Dex
            // 
            this.nud_Dex.Location = new System.Drawing.Point(122, 67);
            this.nud_Dex.Maximum = new decimal(new int[] {
            802,
            0,
            0,
            0});
            this.nud_Dex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Dex.Name = "nud_Dex";
            this.nud_Dex.Size = new System.Drawing.Size(47, 20);
            this.nud_Dex.TabIndex = 23;
            this.nud_Dex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Dex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(175, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Gender: ";
            // 
            // cmb_Gender
            // 
            this.cmb_Gender.FormattingEnabled = true;
            this.cmb_Gender.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cmb_Gender.Location = new System.Drawing.Point(178, 67);
            this.cmb_Gender.Name = "cmb_Gender";
            this.cmb_Gender.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Gender.Size = new System.Drawing.Size(59, 21);
            this.cmb_Gender.TabIndex = 25;
            this.cmb_Gender.Text = "Male";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(243, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Level:";
            // 
            // cmb_Levels
            // 
            this.cmb_Levels.FormattingEnabled = true;
            this.cmb_Levels.Items.AddRange(new object[] {
            "1 - 10",
            "11 - 20",
            "21 - 30",
            "31 - 40",
            "41 - 50",
            "51 - 60",
            "61 - 70",
            "71 - 80",
            "81 - 90",
            "91 - Higher"});
            this.cmb_Levels.Location = new System.Drawing.Point(246, 67);
            this.cmb_Levels.Name = "cmb_Levels";
            this.cmb_Levels.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Levels.Size = new System.Drawing.Size(84, 21);
            this.cmb_Levels.TabIndex = 27;
            this.cmb_Levels.Text = "91 to Higher";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 552);
            this.Controls.Add(this.cmb_Levels);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmb_Gender);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nud_Dex);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cb_Spanish);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.lv_log);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.tb_Folder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_Default);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_PokemonToFind);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.lb_dummy2);
            this.Controls.Add(this.tb_PID);
            this.Controls.Add(this.l_dummy);
            this.Controls.Add(this.tb_Port);
            this.Controls.Add(this.tb_IP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Ledybot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_IP;
        private System.Windows.Forms.TextBox tb_Port;
        private System.Windows.Forms.Label l_dummy;
        private System.Windows.Forms.TextBox tb_PID;
        private System.Windows.Forms.Label lb_dummy2;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.TextBox tb_PokemonToFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Default;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Folder;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.ListView lv_log;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader Trainer;
        private System.Windows.Forms.ColumnHeader NickName;
        private System.Windows.Forms.ColumnHeader Country;
        private System.Windows.Forms.ColumnHeader SubCountry;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.CheckBox cb_Spanish;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nud_Dex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_Gender;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmb_Levels;
    }
}

