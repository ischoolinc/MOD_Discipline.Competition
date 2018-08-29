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

namespace Ischool.discipline_competition
{
    public partial class frmScoreSheet : BaseForm
    {
        private bool _initFinish = false;
        private QueryHelper _qh = new QueryHelper();

        public frmScoreSheet()
        {
            InitializeComponent();
        }

        private void frmEditScoreSheet_Load(object sender, EventArgs e)
        {
            lbSchoolYear.Text = School.DefaultSchoolYear;
            lbSemester.Text = School.DefaultSemester;

            dateTimeInput1.Value = DateTime.Now;

            ReloadDataGridView();
            _initFinish = true;

        }

        private void dateTimeInput1_ValueChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();

            string sql = string.Format(@"
SELECT
    class.class_name 
    , check_item.name AS check_item_name
    , check_item.max_score 
    , check_item.min_score
    , student.name AS student_name
    , teacher.teacher_name
    , CASE
        WHEN student.name IS NOT NULL THEN '評分員'
        WHEN teacher.teacher_name IS NOT NULL THEN '管理員'
        ELSE ''
        END AS 身分
    , score_sheet.*
FROM
    $ischool.discipline_competition.score_sheet AS score_sheet
    LEFT OUTER JOIN class
        ON class.id = score_sheet.ref_class_id
    LEFT OUTER JOIN $ischool.discipline_competition.check_item AS check_item
        ON check_item.uid = score_sheet.ref_check_item_id
    LEFT OUTER JOIN $ischool.discipline_competition.scorer AS scorer
        ON scorer.account = score_sheet.account
    LEFT OUTER JOIN student
        ON student.id = scorer.ref_student_id
    LEFT OUTER JOIN $ischool.discipline_competition.admin AS admin
        ON admin.account = score_sheet.account
    LEFT OUTER JOIN teacher
        ON teacher.id = admin.ref_teacher_id
WHERE
    score_sheet.create_time::DATE = '{0}'::DATE
ORDER BY
    class.grade_year
    , class.display_order
    , check_item.display_order
            ", dateTimeInput1.Value.ToString("yyyy-MM-dd"));

            DataTable dt = _qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = "" + row["class_name"];
                dgvrow.Cells[col++].Value = "" + row["check_item_name"];
                dgvrow.Cells[col++].Value = "" + row["seat_no"];
                dgvrow.Cells[col++].Value = "" + row["coordinate"];
                dgvrow.Cells[col++].Value = "" + row["score"];
                dgvrow.Cells[col++].Value = ("" + row["身分"]) == "評分員" ? ("" + row["student_name"]) : ("" + row["teacher_name"]);
                dgvrow.Cells[col++].Value = "" + row["身分"];
                dgvrow.Cells[col++].Value = ("" + row["is_canceled"]) == "true" ? "是" : "否";
                dgvrow.Tag = row;

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count > 0 )
            {
                frmEditScoreSheet form = new frmEditScoreSheet((DataRow)dataGridViewX1.SelectedRows[0].Tag);
                form.FormClosed += delegate
                {
                    ReloadDataGridView();
                };
                form.ShowDialog();
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddScoreSheet form = new frmAddScoreSheet(dateTimeInput1.Value);
            form.FormClosed += delegate
            {
                ReloadDataGridView();
            };
            form.ShowDialog();
        }
    }
}
