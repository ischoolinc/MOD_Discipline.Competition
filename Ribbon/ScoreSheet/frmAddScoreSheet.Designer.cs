namespace Ischool.discipline_competition
{
    partial class frmAddScoreSheet
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cbxClass = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cbxPeriod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cbxCheckItem = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.tbxSeatNo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.tbxCoordinate = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.cbxScore = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnLeave = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.lbAccount = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.tbxRemark = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dateTimeInput1 = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInput1)).BeginInit();
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
            this.labelX1.Location = new System.Drawing.Point(12, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(100, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "管理員登入帳號";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 55);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "違規班級";
            // 
            // cbxClass
            // 
            this.cbxClass.DisplayMember = "Text";
            this.cbxClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxClass.FormattingEnabled = true;
            this.cbxClass.ItemHeight = 19;
            this.cbxClass.Location = new System.Drawing.Point(118, 55);
            this.cbxClass.Name = "cbxClass";
            this.cbxClass.Size = new System.Drawing.Size(121, 25);
            this.cbxClass.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxClass.TabIndex = 3;
            this.cbxClass.TextChanged += new System.EventHandler(this.cbxClass_TextChanged);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(12, 101);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "評分時段";
            // 
            // cbxPeriod
            // 
            this.cbxPeriod.DisplayMember = "Text";
            this.cbxPeriod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPeriod.FormattingEnabled = true;
            this.cbxPeriod.ItemHeight = 19;
            this.cbxPeriod.Location = new System.Drawing.Point(118, 100);
            this.cbxPeriod.Name = "cbxPeriod";
            this.cbxPeriod.Size = new System.Drawing.Size(121, 25);
            this.cbxPeriod.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxPeriod.TabIndex = 5;
            this.cbxPeriod.SelectedIndexChanged += new System.EventHandler(this.cbxPeriod_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(345, 101);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(60, 23);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "評分項目";
            // 
            // cbxCheckItem
            // 
            this.cbxCheckItem.DisplayMember = "Text";
            this.cbxCheckItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxCheckItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCheckItem.FormattingEnabled = true;
            this.cbxCheckItem.ItemHeight = 19;
            this.cbxCheckItem.Location = new System.Drawing.Point(444, 100);
            this.cbxCheckItem.Name = "cbxCheckItem";
            this.cbxCheckItem.Size = new System.Drawing.Size(190, 25);
            this.cbxCheckItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxCheckItem.TabIndex = 7;
            this.cbxCheckItem.SelectedIndexChanged += new System.EventHandler(this.cbxCheckItem_SelectedIndexChanged);
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(12, 146);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(75, 23);
            this.labelX5.TabIndex = 8;
            this.labelX5.Text = "違規之座號";
            // 
            // tbxSeatNo
            // 
            // 
            // 
            // 
            this.tbxSeatNo.Border.Class = "TextBoxBorder";
            this.tbxSeatNo.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxSeatNo.Location = new System.Drawing.Point(118, 145);
            this.tbxSeatNo.Name = "tbxSeatNo";
            this.tbxSeatNo.Size = new System.Drawing.Size(190, 25);
            this.tbxSeatNo.TabIndex = 9;
            this.tbxSeatNo.TextChanged += new System.EventHandler(this.tbxSeatNo_TextChanged);
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = "";
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(345, 146);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(75, 23);
            this.labelX6.TabIndex = 10;
            this.labelX6.Text = "違規之座標";
            // 
            // tbxCoordinate
            // 
            // 
            // 
            // 
            this.tbxCoordinate.Border.Class = "TextBoxBorder";
            this.tbxCoordinate.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxCoordinate.Location = new System.Drawing.Point(444, 145);
            this.tbxCoordinate.Name = "tbxCoordinate";
            this.tbxCoordinate.Size = new System.Drawing.Size(190, 25);
            this.tbxCoordinate.TabIndex = 11;
            this.tbxCoordinate.TextChanged += new System.EventHandler(this.tbxCoordinate_TextChanged);
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(12, 191);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(75, 23);
            this.labelX7.TabIndex = 12;
            this.labelX7.Text = "評分加減分";
            // 
            // cbxScore
            // 
            this.cbxScore.DisplayMember = "Text";
            this.cbxScore.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxScore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxScore.FormattingEnabled = true;
            this.cbxScore.ItemHeight = 19;
            this.cbxScore.Location = new System.Drawing.Point(118, 190);
            this.cbxScore.Name = "cbxScore";
            this.cbxScore.Size = new System.Drawing.Size(121, 25);
            this.cbxScore.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxScore.TabIndex = 13;
            // 
            // btnLeave
            // 
            this.btnLeave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLeave.BackColor = System.Drawing.Color.Transparent;
            this.btnLeave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLeave.Location = new System.Drawing.Point(559, 347);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(75, 23);
            this.btnLeave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLeave.TabIndex = 14;
            this.btnLeave.Text = "離開";
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(478, 347);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.Class = "";
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Location = new System.Drawing.Point(345, 13);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(63, 23);
            this.labelX8.TabIndex = 16;
            this.labelX8.Text = "評分日期";
            // 
            // lbAccount
            // 
            this.lbAccount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbAccount.BackgroundStyle.Class = "";
            this.lbAccount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbAccount.Location = new System.Drawing.Point(118, 13);
            this.lbAccount.Name = "lbAccount";
            this.lbAccount.Size = new System.Drawing.Size(190, 23);
            this.lbAccount.TabIndex = 18;
            this.lbAccount.Text = "labelX9";
            // 
            // labelX9
            // 
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.Class = "";
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Location = new System.Drawing.Point(12, 236);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(75, 23);
            this.labelX9.TabIndex = 19;
            this.labelX9.Text = "備註";
            // 
            // tbxRemark
            // 
            // 
            // 
            // 
            this.tbxRemark.Border.Class = "TextBoxBorder";
            this.tbxRemark.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxRemark.Location = new System.Drawing.Point(118, 234);
            this.tbxRemark.Multiline = true;
            this.tbxRemark.Name = "tbxRemark";
            this.tbxRemark.Size = new System.Drawing.Size(516, 95);
            this.tbxRemark.TabIndex = 20;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // dateTimeInput1
            // 
            this.dateTimeInput1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.dateTimeInput1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInput1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInput1.ButtonDropDown.Visible = true;
            this.dateTimeInput1.IsPopupCalendarOpen = false;
            this.dateTimeInput1.Location = new System.Drawing.Point(444, 12);
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dateTimeInput1.MonthCalendar.BackgroundStyle.Class = "";
            this.dateTimeInput1.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.Class = "";
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.MonthCalendar.DisplayMonth = new System.DateTime(2018, 8, 1, 0, 0, 0, 0);
            this.dateTimeInput1.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimeInput1.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.Class = "";
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInput1.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTimeInput1.Name = "dateTimeInput1";
            this.dateTimeInput1.Size = new System.Drawing.Size(190, 25);
            this.dateTimeInput1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInput1.TabIndex = 21;
            // 
            // frmAddScoreSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 382);
            this.Controls.Add(this.dateTimeInput1);
            this.Controls.Add(this.tbxRemark);
            this.Controls.Add(this.labelX9);
            this.Controls.Add(this.lbAccount);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.cbxScore);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.tbxCoordinate);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.tbxSeatNo);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.cbxCheckItem);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.cbxPeriod);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cbxClass);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(671, 421);
            this.MinimumSize = new System.Drawing.Size(671, 421);
            this.Name = "frmAddScoreSheet";
            this.Text = "新增評分紀錄";
            this.Load += new System.EventHandler(this.frmAddScoreSheet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInput1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxClass;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxPeriod;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxCheckItem;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxSeatNo;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxCoordinate;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxScore;
        private DevComponents.DotNetBar.ButtonX btnLeave;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX lbAccount;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxRemark;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInput1;
    }
}