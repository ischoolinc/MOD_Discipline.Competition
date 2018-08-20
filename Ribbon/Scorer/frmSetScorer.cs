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
    public partial class frmSetScorer : BaseForm
    {
        public frmSetScorer()
        {
            InitializeComponent();
        }

        private bool _initFinish = false;

        private void frmSetScorer_Load(object sender, EventArgs e)
        {
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            int semester = int.Parse(School.DefaultSemester);

            //Init SchoolYear
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);

            cbxSchoolYear.SelectedIndex = 1;

            //Init Semester
            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);

            cbxSemester.SelectedIndex = semester - 1;

            //Init DataGridView
            ReloadDataGridView();

            _initFinish = true;
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();

            #region SQL
            string sql = string.Format(@"
SELECT
    class.grade_year
    , class.class_name
    , student.seat_no
    , student.name
    , student.sa_login_name
    , scorer.uid
FROM
    $ischool.discipline_competition.scorer AS scorer
    LEFT OUTER JOIN student
        ON student.id = scorer.ref_student_id
    LEFT OUTER JOIN class
        ON class.id = student.ref_class_id
WHERE
    scorer.school_year = {0}
    AND scorer.semester = {1}
ORDER BY
    class.grade_year
    , class.display_order
    , student.seat_no
            ", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString()); 
            #endregion

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            lbScorerCount.Text = string.Format("評分員人數: {0}位",dt.Rows.Count);

            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;

                dgvrow.Cells[col++].Value = "" + row["grade_year"];
                dgvrow.Cells[col++].Value = "" + row["class_name"];
                dgvrow.Cells[col++].Value = "" + row["seat_no"];
                dgvrow.Cells[col++].Value = "" + row["name"];
                dgvrow.Cells[col++].Value = "" + row["sa_login_name"];
                dgvrow.Cells[col++].Value = "刪除";
                dgvrow.Tag = "" + row["uid"]; // 評分員編號

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 5 )
            {
                string studentName = dataGridViewX1.Rows[e.RowIndex].Cells[3].Value.ToString();
                string scorerID = dataGridViewX1.Rows[e.RowIndex].Tag.ToString();
                DialogResult result = MsgBox.Show(string.Format("確定刪除 {0} 評分員身分?", studentName),"提醒",MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        AccessHelper access = new AccessHelper();
                        access.DeletedValues(access.Select<UDT.Scorer>(string.Format("uid = {0}", scorerID)));
                        MsgBox.Show("刪除成功");

                        ReloadDataGridView();
                    }
                    catch(Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                    
                }
            }
        }

        private void cbxSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void cbxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void bntLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddScorer_Click(object sender, EventArgs e)
        {
            frmAddScorer form = new frmAddScorer(cbxSchoolYear.SelectedItem.ToString(),cbxSemester.SelectedItem.ToString());
            form.FormClosed += delegate 
            {
                ReloadDataGridView();
            };
            form.ShowDialog();

        }
    }
}
