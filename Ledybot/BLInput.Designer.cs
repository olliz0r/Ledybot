namespace Ledybot
{
    partial class BLInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BLInput));
            this.btn_Save = new System.Windows.Forms.Button();
            this.nud_FC1 = new System.Windows.Forms.NumericUpDown();
            this.nud_FC3 = new System.Windows.Forms.NumericUpDown();
            this.nud_FC2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FC3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FC2)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(12, 39);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(260, 23);
            this.btn_Save.TabIndex = 10;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // nud_FC1
            // 
            this.nud_FC1.Location = new System.Drawing.Point(13, 13);
            this.nud_FC1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nud_FC1.Name = "nud_FC1";
            this.nud_FC1.Size = new System.Drawing.Size(54, 20);
            this.nud_FC1.TabIndex = 11;
            this.nud_FC1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_FC3
            // 
            this.nud_FC3.Location = new System.Drawing.Point(218, 13);
            this.nud_FC3.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nud_FC3.Name = "nud_FC3";
            this.nud_FC3.Size = new System.Drawing.Size(54, 20);
            this.nud_FC3.TabIndex = 12;
            this.nud_FC3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_FC2
            // 
            this.nud_FC2.Location = new System.Drawing.Point(115, 13);
            this.nud_FC2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nud_FC2.Name = "nud_FC2";
            this.nud_FC2.Size = new System.Drawing.Size(54, 20);
            this.nud_FC2.TabIndex = 13;
            this.nud_FC2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "-";
            // 
            // BLInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 75);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_FC2);
            this.Controls.Add(this.nud_FC3);
            this.Controls.Add(this.nud_FC1);
            this.Controls.Add(this.btn_Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 114);
            this.MinimumSize = new System.Drawing.Size(300, 114);
            this.Name = "BLInput";
            this.Text = "BLInput";
            ((System.ComponentModel.ISupportInitialize)(this.nud_FC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FC3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_FC2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.NumericUpDown nud_FC1;
        private System.Windows.Forms.NumericUpDown nud_FC3;
        private System.Windows.Forms.NumericUpDown nud_FC2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}