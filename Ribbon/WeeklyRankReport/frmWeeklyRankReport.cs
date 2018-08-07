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
using K12.Data;
using FISCA.Data;
using FISCA.UDT;
using Aspose.Cells;
using System.IO;

namespace Ischool.discipline_competition
{
    public partial class frmWeeklyRankReport : BaseForm
    {
        private bool _initFinish = false;


        public frmWeeklyRankReport()
        {
            InitializeComponent();
        }

        private void frmWeeklyRankReport_Load(object sender, EventArgs e)
        {
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            int semester = int.Parse(School.DefaultSemester);
            // Init SchoolYear
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);
            cbxSchoolYear.SelectedIndex = 1;
            // Init Semester
            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);
            cbxSemester.SelectedIndex = semester - 1;
            // Init WeekNo
            ReloadWeekNo();

            this._initFinish = true;
        }

        private void ReloadWeekNo()
        {
            cbxWeekNo.Items.Clear();

            string sql = string.Format(@"
SELECT DISTINCT
    week_number
FROM
    $ischool.discipline_competition.weekly_rank
WHERE
    school_year = {0}
    AND semester = {1}
ORDER BY
    week_number
            ", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString());

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                cbxWeekNo.Items.Add("" + row["weeK_number"]);
            }
            if (cbxWeekNo.Items.Count > 0)
            {
                cbxWeekNo.SelectedIndex = 0;
            }
        }

        // 取得週排名資料
        private DataTable getWeeklyRank(string schoolYear, string semester, string weekNo)
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
            ", schoolYear, semester, weekNo);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return dt;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {  
            DataTable dt = getWeeklyRank(cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString(), cbxWeekNo.SelectedItem.ToString());

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
                    fillSheetData(wb, template, i, s1rowIndex, row);
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
            string fileName = string.Format("{0}_{1}_秩序競賽第{2}週_排名結果", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString(), cbxWeekNo.SelectedItem.ToString());
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
        private void fillSheetData(Workbook wb, Workbook template, int sheetNo, int rowIndex, DataRow row)
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

        private void cbxSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                ReloadWeekNo();
            }
        }

        private void cbxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                ReloadWeekNo();
            }
        }
    }
}
