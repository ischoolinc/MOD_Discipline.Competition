namespace Ischool.discipline_competition
{
    partial class frmAddScorer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cbxGradeYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.tbxSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnLeave = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.ckbxAll = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbScorerCount = new DevComponents.DotNetBar.LabelX();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(118, 39);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "學生清單";
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.AllowUserToResizeColumns = false;
            this.dataGridViewX1.AllowUserToResizeRows = false;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column7,
            this.Column5});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(12, 132);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(661, 425);
            this.dataGridViewX1.TabIndex = 1;
            this.dataGridViewX1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellClick);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 61);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(42, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "年級";
            // 
            // cbxGradeYear
            // 
            this.cbxGradeYear.DisplayMember = "Text";
            this.cbxGradeYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxGradeYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGradeYear.FormattingEnabled = true;
            this.cbxGradeYear.ItemHeight = 19;
            this.cbxGradeYear.Location = new System.Drawing.Point(60, 60);
            this.cbxGradeYear.Name = "cbxGradeYear";
            this.cbxGradeYear.Size = new System.Drawing.Size(70, 25);
            this.cbxGradeYear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxGradeYear.TabIndex = 3;
            this.cbxGradeYear.SelectedIndexChanged += new System.EventHandler(this.cbxGradeYear_SelectedIndexChanged);
            // 
            // labelX3
            // 
            this.labelX3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(386, 102);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(119, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "搜尋(姓名、學號)";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // tbxSearch
            // 
            this.tbxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.tbxSearch.Border.Class = "TextBoxBorder";
            this.tbxSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxSearch.Location = new System.Drawing.Point(511, 101);
            this.tbxSearch.Name = "tbxSearch";
            this.tbxSearch.Size = new System.Drawing.Size(162, 25);
            this.tbxSearch.TabIndex = 5;
            this.tbxSearch.TextChanged += new System.EventHandler(this.tbxSearch_TextChanged);
            // 
            // btnLeave
            // 
            this.btnLeave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLeave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeave.BackColor = System.Drawing.Color.Transparent;
            this.btnLeave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLeave.Location = new System.Drawing.Point(598, 563);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(75, 23);
            this.btnLeave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLeave.TabIndex = 10;
            this.btnLeave.Text = "離開";
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(517, 563);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ckbxAll
            // 
            this.ckbxAll.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.ckbxAll.BackgroundStyle.Class = "";
            this.ckbxAll.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ckbxAll.Location = new System.Drawing.Point(12, 101);
            this.ckbxAll.Name = "ckbxAll";
            this.ckbxAll.Size = new System.Drawing.Size(100, 23);
            this.ckbxAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ckbxAll.TabIndex = 12;
            this.ckbxAll.Text = "全選";
            this.ckbxAll.CheckedChanged += new System.EventHandler(this.ckbxAll_CheckedChanged);
            // 
            // Column6
            // 
            this.Column6.HeaderText = "指定";
            this.Column6.Name = "Column6";
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.Width = 60;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "年級";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "班級";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "座號";
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "姓名";
            this.Column4.Name = "Column4";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "學號";
            this.Column7.Name = "Column7";
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "登入帳號";
            this.Column5.Name = "Column5";
            // 
            // lbScorerCount
            // 
            this.lbScorerCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbScorerCount.BackgroundStyle.Class = "";
            this.lbScorerCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbScorerCount.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbScorerCount.Location = new System.Drawing.Point(12, 563);
            this.lbScorerCount.Name = "lbScorerCount";
            this.lbScorerCount.Size = new System.Drawing.Size(118, 23);
            this.lbScorerCount.TabIndex = 13;
            this.lbScorerCount.Text = "人數綜計:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::Ischool.discipline_competition.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(326, 355);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // frmAddScorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 598);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbScorerCount);
            this.Controls.Add(this.ckbxAll);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.tbxSearch);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cbxGradeYear);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "frmAddScorer";
            this.Text = "新增評分員";
            this.Load += new System.EventHandler(this.frmAddScorer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxGradeYear;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxSearch;
        private DevComponents.DotNetBar.ButtonX btnLeave;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckbxAll;
        private DevComponents.DotNetBar.LabelX lbScorerCount;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}