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
using FISCA.UDT;
using FISCA.Data;
using Aspose.Cells;
using System.IO;
using FISCA.Presentation;

namespace Ischool.discipline_competition
{
    public partial class frmSemesterScore : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private QueryHelper _qh = new QueryHelper();
        private Dictionary<string, PrintHistory> _dicPrintHistory = new Dictionary<string, PrintHistory>();
        private BackgroundWorker _bgw = new BackgroundWorker();

        private class PrintHistory
        {
            public string SchoolYear { get; set; }
            public string Semester { get; set; }
            public string CreateDate { get; set; }
            public string CreateBy { get; set; }
        }

        public frmSemesterScore()
        {
            InitializeComponent();
            this._bgw.WorkerReportsProgress = true;
            this._bgw.DoWork += new DoWorkEventHandler(this.bgw_DoWork);
            this._bgw.ProgressChanged += new ProgressChangedEventHandler(this.bgw_ProgressChanged);
            this._bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgw_RunWorkerCompleted);
        }

        private void frmSemesterScore_Load(object sender, EventArgs e)
        {
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            int semester = int.Parse(School.DefaultSemester);

            #region Init SchoolYear
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);
            cbxSchoolYear.SelectedIndex = 1;
            #endregion

            #region Init Semester
            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);
            cbxSemester.SelectedIndex = semester - 1;
            #endregion

            // 取的學期排名計算紀錄
            getPrintHistory();
        }

        private void getPrintHistory()
        {
            string sql = @"
SELECT DISTINCT
    school_year
    , semester
    , create_time
    , created_by
FROM
    $ischool.discipline_competition.semester_stats
";
            
            DataTable dt = this._qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                string key = string.Format("{0}_{1}","" + row["school_year"],"" + row["semester"]);
                if (!this._dicPrintHistory.ContainsKey(key))
                {
                    this._dicPrintHistory.Add(key,new PrintHistory());
                }
                this._dicPrintHistory[key].SchoolYear = "" + row["school_year"];
                this._dicPrintHistory[key].Semester = "" + row["semester"];
                this._dicPrintHistory[key].CreateDate = DateTime.Parse("" + row["create_time"]).ToString("yyyy/MM/dd");
                this._dicPrintHistory[key].CreateBy = "" + row["created_by"];
            }
        }

        private void btnCalculateScore_Click(object sender, EventArgs e)
        {
            DataObject data = new DataObject();
            data.SchoolYear = cbxSchoolYear.SelectedItem.ToString();
            data.Semester = cbxSemester.SelectedItem.ToString();

            this.btnCalculateScore.Enabled = false;

            this._bgw.RunWorkerAsync(data);
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            DataObject data = (DataObject)e.Argument;

            string key = string.Format("{0}_{1}", data.SchoolYear, data.Semester);
            if (this._dicPrintHistory.ContainsKey(key))
            {
                PrintHistory ph = this._dicPrintHistory[key];

                DialogResult dRresult = MsgBox.Show(string.Format("「{0}」學年度、「{1}」學期，學期排名作業已計算過! \n 計算日期{2} 計算者{3}  \n 確定重新計算?"
                    , ph.SchoolYear, ph.Semester, ph.CreateDate, ph.CreateBy), "提醒", MessageBoxButtons.YesNo);

                if (dRresult == DialogResult.Yes)
                {
                    execute(data.SchoolYear, data.Semester, e);
                }
            }
            else
            {
                execute(data.SchoolYear, data.Semester, e);
            }
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 顯示進度條
            MotherForm.SetStatusBarMessage("計算學期排名中...", e.ProgressPercentage);
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MsgBox.Show(e.Error.Message);
            }
            else
            {
                this.btnCalculateScore.Enabled = true;
                MotherForm.SetStatusBarMessage("計算學期排名完成。");

                DialogResult result = MsgBox.Show("學期排名已計算完成，確定產出排名報表?", "提醒", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    DataTable dt = (DataTable)e.Result;
                    print(dt);
                }
            }
        }

        private void execute(string schoolYear, string semester, DoWorkEventArgs e)
        {
            this._bgw.ReportProgress(15);
            // 1.計算各班學期統計
            SemesterStatsCalculator calOne = new SemesterStatsCalculator(schoolYear, semester);
            calOne.Execute();
            this._bgw.ReportProgress(30);

            // 2.根據年級計算學期排名
            SemesterRankCalculator calTwo = new SemesterRankCalculator(schoolYear, semester);
            calTwo.Execute();
            this._bgw.ReportProgress(60);

            // 3. 找出當學期排名
            DataTable dt = DAO.SemesterRank.GetSemesterRank(schoolYear, semester);
            this._bgw.ReportProgress(90);

            e.Result = dt;
        }

        private void print(DataTable dt)
        {
            Workbook template = new Workbook(new MemoryStream(Properties.Resources.學期統計排名樣板));
            Workbook wb = new Workbook(new MemoryStream(Properties.Resources.學期統計排名樣板));

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
            string fileName = string.Format("{0}_{1}_秩序競賽學期排名結果", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString());
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

        /// <summary>
        /// 填工作表資料
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="template"></param>
        /// <param name="sheetNo"></param>
        /// <param name="rowIndex"></param>
        /// <param name="row"></param>
        private void fillSheetData(Workbook wb, Workbook template, int sheetNo, int rowIndex, DataRow row)
        {
            // 複製樣板格式
            wb.Worksheets[sheetNo].Cells.CopyRow(template.Worksheets[0].Cells, 1, rowIndex);

            wb.Worksheets[sheetNo].Cells[rowIndex, 0].PutValue("" + row["grade_year"]); // 年級
            wb.Worksheets[sheetNo].Cells[rowIndex, 1].PutValue("" + row["class_name"]); // 班級名稱
            wb.Worksheets[sheetNo].Cells[rowIndex, 2].PutValue("" + row["rank_total"]); // 學期週排名次加總
            wb.Worksheets[sheetNo].Cells[rowIndex, 3].PutValue("" + row["average_score"]); // 學期週總分平均
            wb.Worksheets[sheetNo].Cells[rowIndex, 4].PutValue("" + row["rank"]); // 學期排名
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private class DataObject
        {
            public string SchoolYear;
            public string Semester;
        }
    }
}
