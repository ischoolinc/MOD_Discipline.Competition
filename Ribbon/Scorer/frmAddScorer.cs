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

namespace Ischool.discipline_competition
{
    public partial class frmAddScorer : BaseForm
    {
        private bool _initFinish = false;
        private string _schoolYear;
        private string _semester;
        private string _userAccount = DAO.Actor.Instance.GetUserAccount();

        public frmAddScorer(string schoolYear , string semester)
        {
            InitializeComponent();

            _schoolYear = schoolYear;
            _semester = semester;
        }

        private void frmAddScorer_Load(object sender, EventArgs e)
        {
            QueryHelper qh = new QueryHelper();

            #region Init GadeYear
            {
                string sql = @"
SELECT DISTINCT
    class.grade_year
FROM
    student
    LEFT OUTER JOIN class
        ON class.id = student.ref_class_id
WHERE
    student.status IN(1,2)
    AND class.grade_year IS NOT NULL
ORDER BY
    grade_year
";
                
                DataTable dt = qh.Select(sql);

                foreach (DataRow row in dt.Rows)
                {
                    cbxGradeYear.Items.Add("" + row["grade_year"]);
                }
                if (cbxGradeYear.Items.Count > 0)
                {
                    cbxGradeYear.SelectedIndex = 0;
                }
            }
            #endregion

            // Init DataGridView : 已被指定評分員的學生不會出現在該名單
            #region Init DataGridView
            {
                string sql = @"
SELECT
    class.grade_year
    , class.class_name
    , student.seat_no
    , student.name
    , student.sa_login_name
    , student.id
FROM
    student
    LEFT OUTER JOIN class
        ON class.id = student.ref_class_id
    LEFT OUTER JOIN $ischool.discipline_competition.scorer AS scorer
        ON scorer.ref_student_id = student.id
WHERE
    student.status IN(1,2)
    AND scorer.uid IS NULL
	AND class.id IS NOT NULL
ORDER BY
    class.grade_year
    , class.display_order
	, class.class_name
    , student.seat_no
";
                DataTable dt = qh.Select(sql);

                foreach (DataRow row in dt.Rows)
                {
                    DataGridViewRow dgvrow = new DataGridViewRow();
                    dgvrow.CreateCells(dataGridViewX1);

                    int col = 1;

                    dgvrow.Cells[col++].Value = "" + row["grade_year"];
                    dgvrow.Cells[col++].Value = "" + row["class_name"];
                    dgvrow.Cells[col++].Value = "" + row["seat_no"];
                    dgvrow.Cells[col++].Value = "" + row["name"];
                    dgvrow.Cells[col++].Value = "" + row["sa_login_name"];
                    dgvrow.Tag = "" + row["id"];

                    dataGridViewX1.Rows.Add(dgvrow);
                }
            }
            #endregion

            ReloadDataGridView(cbxGradeYear.SelectedItem.ToString());

            _initFinish = true;
        }

        public void ReloadDataGridView(string gradeYear)
        {
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (dgvrow.Cells[1].Value.ToString() == gradeYear)
                {
                    dgvrow.Visible = true;
                }
                else
                {
                    dgvrow.Visible = false;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<UDT.Scorer> listInsertScorer = new List<UDT.Scorer>();
            // 整理資料
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (dgvrow.Cells[0].Value != null)
                {
                    if (dgvrow.Cells[0].Value.ToString() == "True")
                    {
                        UDT.Scorer scorer = new UDT.Scorer();
                        scorer.Account = dgvrow.Cells[5].Value.ToString();
                        scorer.RefStudentID = int.Parse(dgvrow.Tag.ToString());
                        scorer.SchoolYear = int.Parse(_schoolYear);
                        scorer.Semester = int.Parse(_semester);
                        scorer.CreateTime = DateTime.Now;
                        scorer.CreatedBy = _userAccount;

                        listInsertScorer.Add(scorer);
                    }
                }
            }
            // 新增資料
            AccessHelper access = new AccessHelper();
            try
            {
                access.InsertValues(listInsertScorer);
                MsgBox.Show("資料儲存成功");
                this.Close();
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxGradeYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView(cbxGradeYear.SelectedItem.ToString());
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 0)
            {
                // 如果學生沒有登入帳號無法被指定為評分員
                if (dataGridViewX1.Rows[e.RowIndex].Cells[5].Value.ToString() == "")
                {
                    string studentName = dataGridViewX1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    MsgBox.Show(string.Format("{0}學生沒有登入帳號，無法被指定為評分員!",studentName));
                }
            }
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (!string.IsNullOrEmpty(tbxSearch.Text.Trim()))
                {
                    dgvrow.Visible = dgvrow.Cells[4].Value.ToString().Contains(tbxSearch.Text) && dgvrow.Cells[1].Value.ToString() == cbxGradeYear.SelectedItem.ToString();
                }
                else
                {
                    dgvrow.Visible = dgvrow.Cells[1].Value.ToString() == cbxGradeYear.SelectedItem.ToString();
                }
            }
        }
    }
}
