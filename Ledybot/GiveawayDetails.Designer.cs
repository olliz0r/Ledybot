namespace Ledybot
{
    partial class GiveawayDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GiveawayDetails));
            this.dgv_Details = new System.Windows.Forms.DataGridView();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Details)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Details
            // 
            this.dgv_Details.AllowDrop = true;
            this.dgv_Details.AllowUserToAddRows = false;
            this.dgv_Details.AllowUserToDeleteRows = false;
            this.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Details.Location = new System.Drawing.Point(13, 13);
            this.dgv_Details.Name = "dgv_Details";
            this.dgv_Details.Size = new System.Drawing.Size(492, 281);
            this.dgv_Details.TabIndex = 0;
            this.dgv_Details.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_Details_DragDrop);
            this.dgv_Details.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_Details_DragEnter);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(13, 301);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(239, 23);
            this.btn_Add.TabIndex = 1;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(266, 301);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(239, 23);
            this.btn_Delete.TabIndex = 2;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // GiveawayDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 346);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.dgv_Details);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(533, 385);
            this.MinimumSize = new System.Drawing.Size(533, 385);
            this.Name = "GiveawayDetails";
            this.Text = "Giveaway Details";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Details)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Delete;
        public System.Windows.Forms.DataGridView dgv_Details;
    }
}