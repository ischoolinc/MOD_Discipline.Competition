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
                dgvrow.Tag = "" + row["uid"];

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

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 2)
            {
                string teacherName = "" + dataGridViewX1.Rows[e.RowIndex].Cells[0].Value;
                string adminID = "" + dataGridViewX1.Rows[e.RowIndex].Tag;
                DialogResult result = MsgBox.Show(string.Format("確定刪除教師「{0}」管理員身分?", teacherName), "提醒", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DAO.Admin.DeleteAdminData(DAO.Actor.Instance.GetRoleAdminID(), adminID);
                        MsgBox.Show("資料刪除成功!");
                        ReloadDataGridView();
                    }
                    catch(Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
