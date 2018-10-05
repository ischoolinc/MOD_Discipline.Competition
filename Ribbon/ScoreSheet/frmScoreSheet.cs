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

            #region Init cbxIsCancel
            {
                CancelStatus item1 = new CancelStatus();
                item1.Text = "--全部--";
                CancelStatus item2 = new CancelStatus();
                item2.Text = "是";
                item2.IsCancel = "true";
                CancelStatus item3 = new CancelStatus();
                item3.Text = "否";
                item3.IsCancel = "false";

                cbxIsCancel.Items.Add(item1);
                cbxIsCancel.Items.Add(item2);
                cbxIsCancel.Items.Add(item3);
                cbxIsCancel.DisplayMember = "Text";
                cbxIsCancel.ValueMember = "IsCancel";

                cbxIsCancel.SelectedIndex = 0;
            }
            #endregion

            dateTimeInput1.Value = DateTime.Now;

            ReloadDataGridView();
            _initFinish = true;
        }

        private void cbxIsCancel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
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
            this.SuspendLayout();

            dataGridViewX1.Rows.Clear();

            DataTable dt = DAO.ScoreSheet.GetScoreSheetByCondition(dateTimeInput1.Value.ToString("yyyy-MM-dd"), ((CancelStatus)cbxIsCancel.SelectedItem).IsCancel);

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
                dgvrow.Cells[col++].Value = ("" + row["身分"]) == "評分員" ? ("" + row["student_name"]) : ("" + row["account"]);
                dgvrow.Cells[col++].Value = "" + row["身分"];
                dgvrow.Cells[col++].Value = ("" + row["is_canceled"]) == "true" ? "是" : "否";
                dgvrow.Tag = row;

                dataGridViewX1.Rows.Add(dgvrow);
            }
            this.ResumeLayout();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddScoreSheet form = new frmAddScoreSheet(dateTimeInput1.Value);
            form.FormClosed += delegate
            {
                ReloadDataGridView();
            };
            form.ShowDialog();
        }

        private class CancelStatus
        {
            public string Text { get; set; }
            public string IsCancel { get; set; }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}
