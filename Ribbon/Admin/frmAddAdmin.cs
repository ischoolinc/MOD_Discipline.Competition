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
using FISCA.UDT;

namespace Ischool.discipline_competition
{
    public partial class frmAddAdmin : BaseForm
    {
        public frmAddAdmin()
        {
            InitializeComponent();
        }

        private void frmAddAdmin_Load(object sender, EventArgs e)
        {
            // 取得在校老師並且尚未指定管理員身分
            string sql = @"
SELECT
    teacher.*
FROM
    teacher
    LEFT OUTER JOIN $ischool.discipline_competition.admin AS admin
        ON admin.ref_teacher_id = teacher.id
WHERE
    admin.uid IS NULL
    AND teacher.status IN(1,2)
";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;

                dgvrow.Cells[col++].Value = "" + row["teacher_name"];
                dgvrow.Cells[col++].Value = "" + row["nickname"];
                dgvrow.Cells[col++].Value = ParseGender("" + row["gender"]);
                dgvrow.Cells[col++].Value = "" + row["st_login_name"];
                dgvrow.Cells[col++].Value = "" + row["dept"];
                dgvrow.Cells[col++].Value = "指定";
                dgvrow.Tag = "" + row["id"];

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private string ParseGender(string gender)
        {
            switch (gender)
            {
                case "0":
                    return "女";
                case "1":
                    return "男";
                default:
                    return null;
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridViewX1.SelectedCells[0].RowIndex;
            int colIndex = dataGridViewX1.SelectedCells[0].ColumnIndex;
            string teacherName = "" + dataGridViewX1.Rows[rowIndex].Cells[0].Value;
            string account = "" + dataGridViewX1.Rows[rowIndex].Cells[3].Value;
            string teacherID = "" + dataGridViewX1.Rows[rowIndex].Tag;
            string roleID = DAO.Actor.Instance.GetRoleAdminID();
            string loginID = DAO.Actor.Instance.GetLoginIDByAccount(account);
            string userAccount = DAO.Actor.Instance.GetUserAccount();

            if (rowIndex > -1 && colIndex == 5)
            {
                if (string.IsNullOrEmpty(account))
                {
                    MsgBox.Show(string.Format("{0}教師沒有登入帳號，無法指定為秩序競賽管理員!",teacherName));
                    return;
                }
                DialogResult result = MsgBox.Show(string.Format("確定將{0}教師指定為秩序競賽管理員?",teacherName),"提醒",MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DAO.Admin.InsertAdminData(account,teacherID,DateTime.Now.ToString("yyyy/MM/dd"),userAccount,roleID,loginID);
                        MsgBox.Show("資料儲存成功!");
                        this.Close();
                    }
                    catch(Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                }
            }
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbxSearch.Text == "")
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    row.Visible = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        row.Visible = (row.Cells[0].Value.ToString().IndexOf(tbxSearch.Text) > -1);
                    }
                }
            }
        }
    }
}
