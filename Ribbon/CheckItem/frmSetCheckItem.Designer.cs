namespace Ischool.discipline_competition
{
    partial class frmSetCheckItem
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cbxPeriod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.Column4 = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLeave = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
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
            this.labelX1.Location = new System.Drawing.Point(12, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(40, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "時段";
            // 
            // cbxPeriod
            // 
            this.cbxPeriod.DisplayMember = "Text";
            this.cbxPeriod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPeriod.FormattingEnabled = true;
            this.cbxPeriod.ItemHeight = 19;
            this.cbxPeriod.Location = new System.Drawing.Point(58, 21);
            this.cbxPeriod.Name = "cbxPeriod";
            this.cbxPeriod.Size = new System.Drawing.Size(121, 25);
            this.cbxPeriod.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxPeriod.TabIndex = 1;
            this.cbxPeriod.SelectedIndexChanged += new System.EventHandler(this.cbxPeriod_SelectedIndexChanged);
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewX1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(12, 63);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(579, 349);
            this.dataGridViewX1.TabIndex = 2;
            this.dataGridViewX1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewX1_CellValidating);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "評分";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "評分項目";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            // 
            // 
            // 
            this.Column3.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.Column3.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Column3.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column3.HeaderText = "加減分最大值";
            this.Column3.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Center;
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.ShowUpDown = true;
            this.Column3.Width = 110;
            // 
            // Column4
            // 
            // 
            // 
            // 
            this.Column4.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.Column4.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Column4.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column4.HeaderText = "加減分最小值";
            this.Column4.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Center;
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.ShowUpDown = true;
            this.Column4.Width = 110;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "排列序號";
            this.Column5.Name = "Column5";
            // 
            // btnLeave
            // 
            this.btnLeave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLeave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeave.BackColor = System.Drawing.Color.Transparent;
            this.btnLeave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLeave.Location = new System.Drawing.Point(516, 418);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(75, 23);
            this.btnLeave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLeave.TabIndex = 3;
            this.btnLeave.Text = "離開";
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(435, 418);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmSetCheckItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 453);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.cbxPeriod);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "frmSetCheckItem";
            this.Text = "管理評分項目";
            this.Load += new System.EventHandler(this.frmSetCheckItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxPeriod;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnLeave;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn Column3;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}