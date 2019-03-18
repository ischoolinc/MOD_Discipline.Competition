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
using Aspose.Cells;
using System.IO;

namespace Ischool.discipline_competition
{
    public partial class ScoreSheetReport : BaseForm
    {

        private bool _initFinish = false;
        private QueryHelper _qh = new QueryHelper();
        private Dictionary<string, WeekNoRecord> _dicWeekNoRecord = new Dictionary<string, WeekNoRecord>();

        private class WeekNoRecord
        {
            public string WeekNo { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        public ScoreSheetReport()
        {
            InitializeComponent();
        }

        private void ScoreSheetReport_Load(object sender, EventArgs e)
        {
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            int semester = int.Parse(School.DefaultSemester);

            cbxSchoolYear.Items.Add(schoolYear + 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.SelectedIndex = 1;

            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);
            cbxSemester.SelectedIndex = semester - 1;

            ReloadCbxWeekNo();

            this._initFinish = true;
        }

        private void cbxSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                ReloadCbxWeekNo();
            }
        }

        private void cbxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                ReloadCbxWeekNo();
            }
        }

        private void ReloadCbxWeekNo()
        {
            this._dicWeekNoRecord.Clear();
            cbxWeekNo.Items.Clear();

            string sql = string.Format(@"
SELECT DISTINCT
    week_number
    , start_date
    , end_date
FROM
    $ischool.discipline_competition.weekly_stats
WHERE
    school_year = {0}
    AND semester = {1}
ORDER BY
    week_number
            ",cbxSchoolYear.SelectedItem.ToString(),cbxSemester.SelectedItem.ToString());

            DataTable dt = this._qh.Select(sql);
            foreach (DataRow row in dt.Rows)
            {
                WeekNoRecord data = new WeekNoRecord();
                data.WeekNo = "" + row["week_number"];
                data.StartDate = "" + row["start_date"];
                data.EndDate = "" + row["end_date"];

                if (!this._dicWeekNoRecord.ContainsKey(data.WeekNo))
                {
                    this._dicWeekNoRecord.Add(data.WeekNo,data);

                    cbxWeekNo.Items.Add(data.WeekNo);
                }
            }
            if (cbxWeekNo.Items.Count > 0)
            {
                cbxWeekNo.SelectedIndex = 0;
                btnPrint.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = false;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.btnPrint.Enabled = false;

            #region 取得資料
            string startDate = DateTime.Parse(this._dicWeekNoRecord[cbxWeekNo.SelectedItem.ToString()].StartDate).ToString("yyyy/MM/dd");
            string endDate = DateTime.Parse(this._dicWeekNoRecord[cbxWeekNo.SelectedItem.ToString()].EndDate).ToString("yyyy/MM/dd");
            string sql = string.Format(@"
SELECT
    class.grade_year
    , class.class_name
    , score_sheet.create_time
    , period.name AS period_name
    , score_sheet.seat_no
    , score_sheet.coordinate
    , check_item.name AS item_name
    , score_sheet.score 
    , score_sheet.remark
FROM
    $ischool.discipline_competition.score_sheet AS score_sheet
    LEFT OUTER JOIN class
        ON class.id = score_sheet.ref_class_id
    LEFT OUTER JOIN $ischool.discipline_competition.check_item AS check_item
        ON check_item.uid = score_sheet.ref_check_item_id
    LEFT OUTER JOIN $ischool.discipline_competition.period AS period
        ON period.uid = check_item.ref_period_id
WHERE
    school_year = {0}
    AND semester = {1}
    AND date_trunc('day', score_sheet.create_time) >= '{2}'
    AND date_trunc('day', score_sheet.create_time) <= '{3}'
    AND (
        is_canceled <> true 
		OR is_canceled IS NULL
    )
            ", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString(), startDate, endDate);

            DataTable dt = this._qh.Select(sql);
            #endregion

            // 設定樣板
            Workbook template = new Workbook(new MemoryStream(Properties.Resources.加扣分違規表樣板));
            Workbook wb = template;

            #region FillData
            string title = string.Format("第{0}週生活教育競賽違規加扣分項目表", cbxWeekNo.SelectedItem.ToString());

            wb.Worksheets["一年級"].Cells[0, 0].PutValue(title);
            wb.Worksheets["二年級"].Cells[0, 0].PutValue(title);
            wb.Worksheets["三年級"].Cells[0, 0].PutValue(title);

            int rowIndex = 2;
            int oneRowIndex = 2;
            int twoRowIndex = 2;
            int threeRowIndex = 2;

            foreach (DataRow row in dt.Rows)
            {
                Worksheet sheet = null;
                string gradeYear = "" + row["grade_year"];

                switch (gradeYear)
                {
                    case "1":
                        sheet = wb.Worksheets["一年級"];
                        rowIndex = oneRowIndex;
                        oneRowIndex++;
                        break;
                    case "2":
                        sheet = wb.Worksheets["二年級"];
                        rowIndex = twoRowIndex;
                        twoRowIndex++;
                        break;
                    case "3":
                        sheet = wb.Worksheets["三年級"];
                        rowIndex = threeRowIndex;
                        threeRowIndex++;
                        break;
                    default:
                        break;
                }
                if (sheet != null)
                {
                    int colIndex = 0;
                    sheet.Cells.CopyRow(template.Worksheets[0].Cells, 2, rowIndex);
                    sheet.Cells[rowIndex, colIndex++].PutValue("" + row["class_name"]);
                    sheet.Cells[rowIndex, colIndex++].PutValue(DateTime.Parse("" + row["create_time"]).ToString("yyyy/MM/dd"));
                    sheet.Cells[rowIndex, colIndex++].PutValue("" + row["period_name"]);
                    sheet.Cells[rowIndex, colIndex++].PutValue(ParseSeatNo_Coordinate("" + row["seat_no"], "" + row["coordinate"]));
                    sheet.Cells[rowIndex, colIndex++].PutValue("" + row["item_name"]);
                    sheet.Cells[rowIndex, colIndex++].PutValue("" + row["score"]);
                    sheet.Cells[rowIndex, colIndex++].PutValue("" + row["remark"]);

                    rowIndex++;
                }

            } 
            #endregion

            #region 儲存資料
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileName = string.Format("第{0}週生活教育競賽違規加扣分項目表", cbxWeekNo.SelectedItem.ToString());
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

            this.btnPrint.Enabled = true;
        }

        public string ParseSeatNo_Coordinate(string seatNo,string coordinate)
        {
            string data = "";
            if (string.IsNullOrEmpty(seatNo))
            {
                data = coordinate;
            }
            else
            {
                data = seatNo + "號";
            }
            return data;
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
