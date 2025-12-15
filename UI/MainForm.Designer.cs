namespace CatiaHoleAutomation
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
            this.tb_BrowsePart = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectPart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbrowseCSV = new System.Windows.Forms.TextBox();
            this.btnBrowseCsv = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.dgvHoleData = new System.Windows.Forms.DataGridView();
            this.SlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X_Coordinate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y_Coordinates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Radius = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hole_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHoleAnalyzer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoleData)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_BrowsePart
            // 
            this.tb_BrowsePart.Location = new System.Drawing.Point(127, 63);
            this.tb_BrowsePart.Name = "tb_BrowsePart";
            this.tb_BrowsePart.Size = new System.Drawing.Size(388, 20);
            this.tb_BrowsePart.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Part :";
            // 
            // btnSelectPart
            // 
            this.btnSelectPart.Location = new System.Drawing.Point(518, 62);
            this.btnSelectPart.Name = "btnSelectPart";
            this.btnSelectPart.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPart.TabIndex = 2;
            this.btnSelectPart.Text = "browse";
            this.btnSelectPart.UseVisualStyleBackColor = true;
            this.btnSelectPart.Click += new System.EventHandler(this.btnSelectPart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(301, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hole Automation Tool";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select Hole Data :";
            // 
            // tbrowseCSV
            // 
            this.tbrowseCSV.Location = new System.Drawing.Point(127, 112);
            this.tbrowseCSV.Name = "tbrowseCSV";
            this.tbrowseCSV.Size = new System.Drawing.Size(388, 20);
            this.tbrowseCSV.TabIndex = 5;
            // 
            // btnBrowseCsv
            // 
            this.btnBrowseCsv.Location = new System.Drawing.Point(520, 112);
            this.btnBrowseCsv.Name = "btnBrowseCsv";
            this.btnBrowseCsv.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCsv.TabIndex = 6;
            this.btnBrowseCsv.Text = "browse";
            this.btnBrowseCsv.UseVisualStyleBackColor = true;
            this.btnBrowseCsv.Click += new System.EventHandler(this.btnBrowseCsv_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(686, 209);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 7;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // dgvHoleData
            // 
            this.dgvHoleData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoleData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SlNo,
            this.X_Coordinate,
            this.Y_Coordinates,
            this.Radius,
            this.Depth,
            this.Hole_Type});
            this.dgvHoleData.Location = new System.Drawing.Point(18, 165);
            this.dgvHoleData.Name = "dgvHoleData";
            this.dgvHoleData.Size = new System.Drawing.Size(644, 432);
            this.dgvHoleData.TabIndex = 8;
            // 
            // SlNo
            // 
            this.SlNo.HeaderText = "SlNo";
            this.SlNo.Name = "SlNo";
            // 
            // X_Coordinate
            // 
            this.X_Coordinate.HeaderText = "X - Coordinate";
            this.X_Coordinate.Name = "X_Coordinate";
            // 
            // Y_Coordinates
            // 
            this.Y_Coordinates.HeaderText = "Y-Coordinates";
            this.Y_Coordinates.Name = "Y_Coordinates";
            // 
            // Radius
            // 
            this.Radius.HeaderText = "Hole Radius";
            this.Radius.Name = "Radius";
            // 
            // Depth
            // 
            this.Depth.HeaderText = "Hole Depth";
            this.Depth.Name = "Depth";
            // 
            // Hole_Type
            // 
            this.Hole_Type.HeaderText = "Hole-Type";
            this.Hole_Type.Name = "Hole_Type";
            // 
            // btnHoleAnalyzer
            // 
            this.btnHoleAnalyzer.Location = new System.Drawing.Point(686, 280);
            this.btnHoleAnalyzer.Name = "btnHoleAnalyzer";
            this.btnHoleAnalyzer.Size = new System.Drawing.Size(75, 44);
            this.btnHoleAnalyzer.TabIndex = 9;
            this.btnHoleAnalyzer.Text = "Hole Analyzer";
            this.btnHoleAnalyzer.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 622);
            this.Controls.Add(this.btnHoleAnalyzer);
            this.Controls.Add(this.dgvHoleData);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnBrowseCsv);
            this.Controls.Add(this.tbrowseCSV);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectPart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_BrowsePart);
            this.Name = "MainForm";
            this.Text = "Hole Automation Tool";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoleData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_BrowsePart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectPart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbrowseCSV;
        private System.Windows.Forms.Button btnBrowseCsv;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.DataGridView dgvHoleData;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn X_Coordinate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y_Coordinates;
        private System.Windows.Forms.DataGridViewTextBoxColumn Radius;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hole_Type;
        private System.Windows.Forms.Button btnHoleAnalyzer;
    }
}

