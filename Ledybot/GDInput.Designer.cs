namespace Ledybot
{
    partial class GDInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GDInput));
            this.tb_Default = new System.Windows.Forms.TextBox();
            this.tb_Specific = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_BrowseDefault = new System.Windows.Forms.Button();
            this.btn_BrowseSpecific = new System.Windows.Forms.Button();
            this.nud_DexNumber = new System.Windows.Forms.NumericUpDown();
            this.btn_Save = new System.Windows.Forms.Button();
            this.cmb_Levels = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_Gender = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nud_Count = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DexNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_Default
            // 
            this.tb_Default.Location = new System.Drawing.Point(12, 138);
            this.tb_Default.Name = "tb_Default";
            this.tb_Default.ReadOnly = true;
            this.tb_Default.Size = new System.Drawing.Size(114, 20);
            this.tb_Default.TabIndex = 5;
            // 
            // tb_Specific
            // 
            this.tb_Specific.Location = new System.Drawing.Point(12, 186);
            this.tb_Specific.Name = "tb_Specific";
            this.tb_Specific.Size = new System.Drawing.Size(114, 20);
            this.tb_Specific.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Dex Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Default: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Specific";
            // 
            // btn_BrowseDefault
            // 
            this.btn_BrowseDefault.Location = new System.Drawing.Point(133, 136);
            this.btn_BrowseDefault.Name = "btn_BrowseDefault";
            this.btn_BrowseDefault.Size = new System.Drawing.Size(125, 23);
            this.btn_BrowseDefault.TabIndex = 6;
            this.btn_BrowseDefault.Text = "Browse";
            this.btn_BrowseDefault.UseVisualStyleBackColor = true;
            this.btn_BrowseDefault.Click += new System.EventHandler(this.btn_BrowseDefault_Click);
            // 
            // btn_BrowseSpecific
            // 
            this.btn_BrowseSpecific.Location = new System.Drawing.Point(133, 184);
            this.btn_BrowseSpecific.Name = "btn_BrowseSpecific";
            this.btn_BrowseSpecific.Size = new System.Drawing.Size(125, 23);
            this.btn_BrowseSpecific.TabIndex = 8;
            this.btn_BrowseSpecific.Text = "Browse";
            this.btn_BrowseSpecific.UseVisualStyleBackColor = true;
            this.btn_BrowseSpecific.Click += new System.EventHandler(this.btn_BrowseSpecific_Click);
            // 
            // nud_DexNumber
            // 
            this.nud_DexNumber.Location = new System.Drawing.Point(123, 39);
            this.nud_DexNumber.Maximum = new decimal(new int[] {
            807,
            0,
            0,
            0});
            this.nud_DexNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_DexNumber.Name = "nud_DexNumber";
            this.nud_DexNumber.Size = new System.Drawing.Size(48, 20);
            this.nud_DexNumber.TabIndex = 2;
            this.nud_DexNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_DexNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(12, 222);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(246, 23);
            this.btn_Save.TabIndex = 9;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
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
            this.cmb_Levels.Location = new System.Drawing.Point(87, 65);
            this.cmb_Levels.Name = "cmb_Levels";
            this.cmb_Levels.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Levels.Size = new System.Drawing.Size(84, 21);
            this.cmb_Levels.TabIndex = 3;
            this.cmb_Levels.Text = "91 to Higher";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Level:";
            // 
            // cmb_Gender
            // 
            this.cmb_Gender.FormattingEnabled = true;
            this.cmb_Gender.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cmb_Gender.Location = new System.Drawing.Point(112, 92);
            this.cmb_Gender.Name = "cmb_Gender";
            this.cmb_Gender.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Gender.Size = new System.Drawing.Size(59, 21);
            this.cmb_Gender.TabIndex = 4;
            this.cmb_Gender.Text = "Male";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Gender: ";
            // 
            // nud_Count
            // 
            this.nud_Count.Location = new System.Drawing.Point(123, 13);
            this.nud_Count.Maximum = new decimal(new int[] {
            802,
            0,
            0,
            0});
            this.nud_Count.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nud_Count.Name = "nud_Count";
            this.nud_Count.Size = new System.Drawing.Size(48, 20);
            this.nud_Count.TabIndex = 1;
            this.nud_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Count.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Max Count:";
            // 
            // GDInput
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 275);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nud_Count);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmb_Gender);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmb_Levels);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.nud_DexNumber);
            this.Controls.Add(this.btn_BrowseSpecific);
            this.Controls.Add(this.btn_BrowseDefault);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Specific);
            this.Controls.Add(this.tb_Default);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(286, 314);
            this.MinimumSize = new System.Drawing.Size(286, 314);
            this.Name = "GDInput";
            this.Text = "GDInput";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.GDInput_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.GDInput_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.nud_DexNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Default;
        private System.Windows.Forms.TextBox tb_Specific;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_BrowseDefault;
        private System.Windows.Forms.Button btn_BrowseSpecific;
        private System.Windows.Forms.NumericUpDown nud_DexNumber;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.ComboBox cmb_Levels;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_Gender;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud_Count;
        private System.Windows.Forms.Label label6;
    }
}