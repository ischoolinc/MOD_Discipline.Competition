using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using Aspose.Cells;
using System.IO;
using FISCA.Data;

namespace Ischool.discipline_competition
{
    public partial class frmWeeklyScore : BaseForm
    {
        public frmWeeklyScore()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (checkWeekNo())
            {
                string schoolYear = cbxSchoolYear.SelectedItem.ToString();
                string semester = cbxSemester.SelectedItem.ToString();
                int weekNo = int.Parse(tbxWeekNo.Text);

                DateTime dtStart = DateTime.Now.AddDays(-7);
                DateTime dtEnd = DateTime.Now;

                // 1. 統計當週各班成績
                WeeklyStatsCalculator calOne = new WeeklyStatsCalculator(schoolYear, semester, weekNo, dtStart, dtEnd);
                calOne.Execute();

                // 2. 計算各年級班排名
                WeeklyRankCalculator calTwo = new WeeklyRankCalculator(schoolYear, semester, weekNo, dtStart, dtEnd);
                calTwo.Execute();

                // 3. 找出當週排名
                DataTable dt = getWeeklyRank(schoolYear, semester, weekNo);

                DialogResult result = MsgBox.Show("週排名已計算完成，確定產出排名報表?","提醒",MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    print(dt);
                }
            }
        }

        // 驗證週次欄位
        private bool checkWeekNo()
        {
            if (string.IsNullOrEmpty(tbxWeekNo.Text))
            {
                MsgBox.Show("週次欄位不可空白!");
                return false;
            }
            int n;
            if (!int.TryParse(tbxWeekNo.Text, out n))
            {
                MsgBox.Show("週次欄位只能填數值!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private DataTable getWeeklyRank(string schoolYear,string semester,int weekNo)
        {
            string sql = string.Format(@"
SELECT
    rank.*
    , class.class_name
FROM
    $ischool.discipline_competition.weekly_rank AS rank
    LEFT OUTER JOIN class
        ON class.id = rank.ref_class_id
WHERE
    school_year = {0}
    AND semester = {1}
    AND week_number = {2}
ORDER BY
    rank.grade_year
    , rank.rank
    , class.display_order
    , class.class_name
            ",schoolYear,semester,weekNo);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return dt;
        }

        private void print(DataTable dt)
        {
            Workbook template = new Workbook(new MemoryStream(Properties.Resources.週統計樣板));
            Workbook wb = new Workbook(new MemoryStream(Properties.Resources.週統計樣板));

            int s1rowIndex = 1;
            int s2rowIndex = 1;
            int s3rowIndex = 1;

            foreach (DataRow row in dt.Rows)
            {
                int i = int.Parse("" + row["grade_year"]) - 1;
                if (i == 0)
                {
                    fillSheetData(wb,template,i,s1rowIndex,row);
                    s1rowIndex++;
                }
                if (i == 1)
                {
                    fillSheetData(wb, template, i, s2rowIndex, row);
                    s2rowIndex++;
                }
                if (i == 2)
                {
                    fillSheetData(wb, template, i, s3rowIndex, row);
                    s3rowIndex++;
                }

            }

            #region 儲存資料
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileName = string.Format("{0}_{1}_秩序競賽第{2}週_排名結果", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString(), tbxWeekNo.Text);
            saveFileDialog.Title = fileName;
            saveFileDialog.FileName = string.Format("{0}.xlsx", fileName);
            saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = new DialogResult();
                try
                {
                    wb.Save(saveFileDialog.FileName);
                    result = MsgBox.Show("檔案儲存完成，是否開啟檔案?", "是否開啟", MessageBoxButtons.YesNo);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show("開啟檔案發生失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                this.Close();
            }

            #endregion
        }

        // 填工作表資料
        private void fillSheetData(Workbook wb,Workbook template,int sheetNo,int rowIndex,DataRow row)
        {
            // 複製樣板格式
            wb.Worksheets[sheetNo].Cells.CopyRow(template.Worksheets[0].Cells, 1, rowIndex);

            wb.Worksheets[sheetNo].Cells[rowIndex, 0].PutValue("" + row["grade_year"]); // 年級
            wb.Worksheets[sheetNo].Cells[rowIndex, 1].PutValue("" + row["class_name"]); // 班級名稱
            wb.Worksheets[sheetNo].Cells[rowIndex, 2].PutValue("" + row["week_total"]); // 週總分
            wb.Worksheets[sheetNo].Cells[rowIndex, 3].PutValue("" + row["rank"]); // 週排名
            wb.Worksheets[sheetNo].Cells[rowIndex, 4].PutValue("" + row["top2_in_a_row"]); // 前2名已連續幾週
            wb.Worksheets[sheetNo].Cells[rowIndex, 5].PutValue("" + row["top3_in_a_row"]); // 前3名已連續幾週
            wb.Worksheets[sheetNo].Cells[rowIndex, 6].PutValue(("" + row["need_reset"]) == "true" ? "是" : "否"); // 本次已達重新計算條件
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmWeeklyScore_Load(object sender, EventArgs e)
        {
            #region InitSchoolYear

            int schoolYear = int.Parse(School.DefaultSchoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear - 1);

            cbxSchoolYear.SelectedIndex = 1;
            #endregion

            #region InitSemester

            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);

            cbxSemester.SelectedIndex = int.Parse(School.DefaultSemester) - 1;
            #endregion

            dtStartTime.Value = DateTime.Now.AddDays(-4);
            dtEndTime.Value = DateTime.Now;
        }
    }
}
