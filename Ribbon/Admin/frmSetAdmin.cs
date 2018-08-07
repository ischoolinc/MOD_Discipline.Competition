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
using FISCA.Data;

namespace Ischool.discipline_competition
{
    public partial class frmSetAdmin : BaseForm
    {
        public frmSetAdmin()
        {
            InitializeComponent();
        }

        private void frmSetAdmin_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }
        
        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            // 取得管理員資料
            string sql = @"
SELECT
    teacher.teacher_name
    , admin.*
FROM
    $ischool.discipline_competition.admin AS admin
    LEFT OUTER JOIN teacher
        ON teacher.id = admin.ref_teacher_id
            ";

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = "" + row["teacher_name"];
                dgvrow.Cells[col++].Value = "" + row["account"];
                dgvrow.Cells[col++].Value = "刪除";

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddAdmin form = new frmAddAdmin();
            form.FormClosed += delegate
            {
                ReloadDataGridView();
            };
            form.ShowDialog();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
