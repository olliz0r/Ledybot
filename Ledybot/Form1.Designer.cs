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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tb_IP = new System.Windows.Forms.TextBox();
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
            this.FC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Export = new System.Windows.Forms.Button();
            this.cb_Spanish = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nud_Dex = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_Gender = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmb_Levels = new System.Windows.Forms.ComboBox();
            this.tc_Control = new System.Windows.Forms.TabControl();
            this.tp_GTS = new System.Windows.Forms.TabPage();
            this.tp_Injection = new System.Windows.Forms.TabPage();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.nud_CountInjection = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_Inject = new System.Windows.Forms.Button();
            this.nud_SlotInjection = new System.Windows.Forms.NumericUpDown();
            this.nud_BoxInjection = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_BrowseInject = new System.Windows.Forms.Button();
            this.tb_FileInjection = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tp_Breeding = new System.Windows.Forms.TabPage();
            this.btn_EggStop = new System.Windows.Forms.Button();
            this.btn_EggStart = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_EggAvailable = new System.Windows.Forms.Button();
            this.ofd_Injection = new System.Windows.Forms.OpenFileDialog();
            this.btn_Disconnect = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.disconnectTimer = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dex)).BeginInit();
            this.tc_Control.SuspendLayout();
            this.tp_GTS.SuspendLayout();
            this.tp_Injection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CountInjection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SlotInjection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BoxInjection)).BeginInit();
            this.tp_Breeding.SuspendLayout();
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
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(119, 13);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(75, 20);
            this.btn_Connect.TabIndex = 5;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // tb_PokemonToFind
            // 
            this.tb_PokemonToFind.Location = new System.Drawing.Point(5, 19);
            this.tb_PokemonToFind.Name = "tb_PokemonToFind";
            this.tb_PokemonToFind.Size = new System.Drawing.Size(100, 20);
            this.tb_PokemonToFind.TabIndex = 6;
            this.tb_PokemonToFind.Text = "Ledyba";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Deposited Pokemon:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Default .pk7:";
            // 
            // tb_Default
            // 
            this.tb_Default.Location = new System.Drawing.Point(5, 128);
            this.tb_Default.Name = "tb_Default";
            this.tb_Default.Size = new System.Drawing.Size(354, 20);
            this.tb_Default.TabIndex = 13;
            this.tb_Default.Text = "D:/Ledybot/pk7s/Ditto.pk7";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Specific .pk7 folder:";
            // 
            // tb_Folder
            // 
            this.tb_Folder.Location = new System.Drawing.Point(5, 171);
            this.tb_Folder.Name = "tb_Folder";
            this.tb_Folder.Size = new System.Drawing.Size(354, 20);
            this.tb_Folder.TabIndex = 15;
            this.tb_Folder.Text = "D:/Ledybot/pk7s";
            // 
            // btn_Start
            // 
            this.btn_Start.Enabled = false;
            this.btn_Start.Location = new System.Drawing.Point(154, 73);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(58, 23);
            this.btn_Start.TabIndex = 16;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Enabled = false;
            this.btn_Stop.Location = new System.Drawing.Point(218, 73);
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
            this.FC});
            this.lv_log.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_log.Location = new System.Drawing.Point(6, 198);
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
            // FC
            // 
            this.FC.Text = "FC";
            this.FC.Width = 65;
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(5, 467);
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
            this.cb_Spanish.Location = new System.Drawing.Point(282, 77);
            this.cb_Spanish.Name = "cb_Spanish";
            this.cb_Spanish.Size = new System.Drawing.Size(64, 17);
            this.cb_Spanish.TabIndex = 20;
            this.cb_Spanish.Text = "Spanish";
            this.cb_Spanish.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(112, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Dex:";
            // 
            // nud_Dex
            // 
            this.nud_Dex.Location = new System.Drawing.Point(115, 19);
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
            this.label7.Location = new System.Drawing.Point(168, 3);
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
            this.cmb_Gender.Location = new System.Drawing.Point(171, 19);
            this.cmb_Gender.Name = "cmb_Gender";
            this.cmb_Gender.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Gender.Size = new System.Drawing.Size(59, 21);
            this.cmb_Gender.TabIndex = 25;
            this.cmb_Gender.Text = "Male";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(236, 3);
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
            this.cmb_Levels.Location = new System.Drawing.Point(239, 19);
            this.cmb_Levels.Name = "cmb_Levels";
            this.cmb_Levels.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Levels.Size = new System.Drawing.Size(84, 21);
            this.cmb_Levels.TabIndex = 27;
            this.cmb_Levels.Text = "91 to Higher";
            // 
            // tc_Control
            // 
            this.tc_Control.Controls.Add(this.tp_GTS);
            this.tc_Control.Controls.Add(this.tp_Injection);
            this.tc_Control.Controls.Add(this.tp_Breeding);
            this.tc_Control.Location = new System.Drawing.Point(1, 33);
            this.tc_Control.Name = "tc_Control";
            this.tc_Control.SelectedIndex = 0;
            this.tc_Control.Size = new System.Drawing.Size(377, 521);
            this.tc_Control.TabIndex = 28;
            // 
            // tp_GTS
            // 
            this.tp_GTS.AllowDrop = true;
            this.tp_GTS.Controls.Add(this.label1);
            this.tp_GTS.Controls.Add(this.cmb_Levels);
            this.tp_GTS.Controls.Add(this.tb_PokemonToFind);
            this.tp_GTS.Controls.Add(this.label8);
            this.tp_GTS.Controls.Add(this.label4);
            this.tp_GTS.Controls.Add(this.cmb_Gender);
            this.tp_GTS.Controls.Add(this.tb_Default);
            this.tp_GTS.Controls.Add(this.label7);
            this.tp_GTS.Controls.Add(this.label5);
            this.tp_GTS.Controls.Add(this.nud_Dex);
            this.tp_GTS.Controls.Add(this.tb_Folder);
            this.tp_GTS.Controls.Add(this.label6);
            this.tp_GTS.Controls.Add(this.btn_Start);
            this.tp_GTS.Controls.Add(this.cb_Spanish);
            this.tp_GTS.Controls.Add(this.btn_Stop);
            this.tp_GTS.Controls.Add(this.btn_Export);
            this.tp_GTS.Controls.Add(this.lv_log);
            this.tp_GTS.Location = new System.Drawing.Point(4, 22);
            this.tp_GTS.Name = "tp_GTS";
            this.tp_GTS.Padding = new System.Windows.Forms.Padding(3);
            this.tp_GTS.Size = new System.Drawing.Size(369, 495);
            this.tp_GTS.TabIndex = 0;
            this.tp_GTS.Text = "GTS";
            this.tp_GTS.UseVisualStyleBackColor = true;
            this.tp_GTS.DragDrop += new System.Windows.Forms.DragEventHandler(this.tp_GTS_DragDrop);
            this.tp_GTS.DragEnter += new System.Windows.Forms.DragEventHandler(this.tp_GTS_DragEnter);
            // 
            // tp_Injection
            // 
            this.tp_Injection.AllowDrop = true;
            this.tp_Injection.Controls.Add(this.btn_Delete);
            this.tp_Injection.Controls.Add(this.nud_CountInjection);
            this.tp_Injection.Controls.Add(this.label10);
            this.tp_Injection.Controls.Add(this.btn_Inject);
            this.tp_Injection.Controls.Add(this.nud_SlotInjection);
            this.tp_Injection.Controls.Add(this.nud_BoxInjection);
            this.tp_Injection.Controls.Add(this.label9);
            this.tp_Injection.Controls.Add(this.label3);
            this.tp_Injection.Controls.Add(this.btn_BrowseInject);
            this.tp_Injection.Controls.Add(this.tb_FileInjection);
            this.tp_Injection.Controls.Add(this.label2);
            this.tp_Injection.Location = new System.Drawing.Point(4, 22);
            this.tp_Injection.Name = "tp_Injection";
            this.tp_Injection.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Injection.Size = new System.Drawing.Size(369, 495);
            this.tp_Injection.TabIndex = 1;
            this.tp_Injection.Text = "Injection";
            this.tp_Injection.UseVisualStyleBackColor = true;
            this.tp_Injection.DragDrop += new System.Windows.Forms.DragEventHandler(this.tp_Injection_DragDrop);
            this.tp_Injection.DragEnter += new System.Windows.Forms.DragEventHandler(this.tp_Injection_DragEnter);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Enabled = false;
            this.btn_Delete.Location = new System.Drawing.Point(7, 120);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(354, 23);
            this.btn_Delete.TabIndex = 10;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // nud_CountInjection
            // 
            this.nud_CountInjection.Location = new System.Drawing.Point(95, 64);
            this.nud_CountInjection.Maximum = new decimal(new int[] {
            960,
            0,
            0,
            0});
            this.nud_CountInjection.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_CountInjection.Name = "nud_CountInjection";
            this.nud_CountInjection.Size = new System.Drawing.Size(38, 20);
            this.nud_CountInjection.TabIndex = 9;
            this.nud_CountInjection.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_CountInjection.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(92, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Count:";
            // 
            // btn_Inject
            // 
            this.btn_Inject.Enabled = false;
            this.btn_Inject.Location = new System.Drawing.Point(7, 91);
            this.btn_Inject.Name = "btn_Inject";
            this.btn_Inject.Size = new System.Drawing.Size(354, 23);
            this.btn_Inject.TabIndex = 7;
            this.btn_Inject.Text = "Inject";
            this.btn_Inject.UseVisualStyleBackColor = true;
            this.btn_Inject.Click += new System.EventHandler(this.btn_Inject_Click);
            // 
            // nud_SlotInjection
            // 
            this.nud_SlotInjection.Location = new System.Drawing.Point(51, 64);
            this.nud_SlotInjection.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nud_SlotInjection.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_SlotInjection.Name = "nud_SlotInjection";
            this.nud_SlotInjection.Size = new System.Drawing.Size(38, 20);
            this.nud_SlotInjection.TabIndex = 6;
            this.nud_SlotInjection.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_SlotInjection.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_SlotInjection.ValueChanged += new System.EventHandler(this.nud_SlotInjection_ValueChanged);
            // 
            // nud_BoxInjection
            // 
            this.nud_BoxInjection.Location = new System.Drawing.Point(7, 64);
            this.nud_BoxInjection.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nud_BoxInjection.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_BoxInjection.Name = "nud_BoxInjection";
            this.nud_BoxInjection.Size = new System.Drawing.Size(38, 20);
            this.nud_BoxInjection.TabIndex = 5;
            this.nud_BoxInjection.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_BoxInjection.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_BoxInjection.ValueChanged += new System.EventHandler(this.nud_BoxInjection_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(48, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Slot:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Box:";
            // 
            // btn_BrowseInject
            // 
            this.btn_BrowseInject.Location = new System.Drawing.Point(291, 24);
            this.btn_BrowseInject.Name = "btn_BrowseInject";
            this.btn_BrowseInject.Size = new System.Drawing.Size(75, 20);
            this.btn_BrowseInject.TabIndex = 2;
            this.btn_BrowseInject.Text = "Browse";
            this.btn_BrowseInject.UseVisualStyleBackColor = true;
            this.btn_BrowseInject.Click += new System.EventHandler(this.btn_BrowseInject_Click);
            // 
            // tb_FileInjection
            // 
            this.tb_FileInjection.Location = new System.Drawing.Point(7, 24);
            this.tb_FileInjection.Name = "tb_FileInjection";
            this.tb_FileInjection.ReadOnly = true;
            this.tb_FileInjection.Size = new System.Drawing.Size(279, 20);
            this.tb_FileInjection.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "File:";
            // 
            // tp_Breeding
            // 
            this.tp_Breeding.Controls.Add(this.btn_EggStop);
            this.tp_Breeding.Controls.Add(this.btn_EggStart);
            this.tp_Breeding.Controls.Add(this.label11);
            this.tp_Breeding.Controls.Add(this.btn_EggAvailable);
            this.tp_Breeding.Location = new System.Drawing.Point(4, 22);
            this.tp_Breeding.Name = "tp_Breeding";
            this.tp_Breeding.Size = new System.Drawing.Size(369, 495);
            this.tp_Breeding.TabIndex = 2;
            this.tp_Breeding.Text = "Breeding";
            this.tp_Breeding.UseVisualStyleBackColor = true;
            // 
            // btn_EggStop
            // 
            this.btn_EggStop.Enabled = false;
            this.btn_EggStop.Location = new System.Drawing.Point(203, 30);
            this.btn_EggStop.Name = "btn_EggStop";
            this.btn_EggStop.Size = new System.Drawing.Size(75, 23);
            this.btn_EggStop.TabIndex = 3;
            this.btn_EggStop.Text = "Stop";
            this.btn_EggStop.UseVisualStyleBackColor = true;
            this.btn_EggStop.Click += new System.EventHandler(this.btn_EggStop_Click);
            // 
            // btn_EggStart
            // 
            this.btn_EggStart.Enabled = false;
            this.btn_EggStart.Location = new System.Drawing.Point(122, 30);
            this.btn_EggStart.Name = "btn_EggStart";
            this.btn_EggStart.Size = new System.Drawing.Size(75, 23);
            this.btn_EggStart.TabIndex = 2;
            this.btn_EggStart.Text = "Start";
            this.btn_EggStart.UseVisualStyleBackColor = true;
            this.btn_EggStart.Click += new System.EventHandler(this.btn_EggStart_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Egg Always Available:";
            // 
            // btn_EggAvailable
            // 
            this.btn_EggAvailable.Enabled = false;
            this.btn_EggAvailable.Location = new System.Drawing.Point(8, 4);
            this.btn_EggAvailable.Name = "btn_EggAvailable";
            this.btn_EggAvailable.Size = new System.Drawing.Size(353, 23);
            this.btn_EggAvailable.TabIndex = 0;
            this.btn_EggAvailable.Text = "Egg Available";
            this.btn_EggAvailable.UseVisualStyleBackColor = true;
            this.btn_EggAvailable.Click += new System.EventHandler(this.btn_EggAvailable_Click);
            // 
            // ofd_Injection
            // 
            this.ofd_Injection.FileName = "Pokemon.pk7";
            // 
            // btn_Disconnect
            // 
            this.btn_Disconnect.Location = new System.Drawing.Point(200, 13);
            this.btn_Disconnect.Name = "btn_Disconnect";
            this.btn_Disconnect.Size = new System.Drawing.Size(75, 20);
            this.btn_Disconnect.TabIndex = 29;
            this.btn_Disconnect.Text = "Disconnect";
            this.btn_Disconnect.UseVisualStyleBackColor = true;
            this.btn_Disconnect.Click += new System.EventHandler(this.btn_Disconnect_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // disconnectTimer
            // 
            this.disconnectTimer.Tick += new System.EventHandler(this.disconnectTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 552);
            this.Controls.Add(this.btn_Disconnect);
            this.Controls.Add(this.tc_Control);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.tb_IP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Ledybot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dex)).EndInit();
            this.tc_Control.ResumeLayout(false);
            this.tp_GTS.ResumeLayout(false);
            this.tp_GTS.PerformLayout();
            this.tp_Injection.ResumeLayout(false);
            this.tp_Injection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CountInjection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_SlotInjection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BoxInjection)).EndInit();
            this.tp_Breeding.ResumeLayout(false);
            this.tp_Breeding.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_IP;
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
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.CheckBox cb_Spanish;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nud_Dex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_Gender;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmb_Levels;
        private System.Windows.Forms.TabControl tc_Control;
        private System.Windows.Forms.TabPage tp_GTS;
        private System.Windows.Forms.TabPage tp_Injection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Inject;
        private System.Windows.Forms.NumericUpDown nud_SlotInjection;
        private System.Windows.Forms.NumericUpDown nud_BoxInjection;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_BrowseInject;
        private System.Windows.Forms.TextBox tb_FileInjection;
        private System.Windows.Forms.OpenFileDialog ofd_Injection;
        private System.Windows.Forms.NumericUpDown nud_CountInjection;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_Disconnect;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.TabPage tp_Breeding;
        private System.Windows.Forms.Button btn_EggAvailable;
        private System.Windows.Forms.Button btn_EggStart;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_EggStop;
        private System.Windows.Forms.Timer disconnectTimer;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ColumnHeader FC;
    }
}

