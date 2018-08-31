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
using FISCA.UDT;
using K12.Data;

namespace Ischool.discipline_competition
{
    public partial class frmSetScorePeriod : BaseForm
    {
        private string _userAccount = DAO.Actor.Instance.GetUserAccount();
        private bool _initFinish = false;

        public frmSetScorePeriod()
        {
            InitializeComponent();
        }

        private void frmSetScorePeriod_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
            _initFinish = true;
        }

        private void ReloadDataGridView()
        {
            this.SuspendLayout();

            AccessHelper access = new AccessHelper();
            List<UDT.Period> listPeriod = access.Select<UDT.Period>();

            dataGridViewX1.Rows.Clear();
            foreach (UDT.Period period in listPeriod)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;

                dgvrow.Cells[col++].Value = period.Enabled;
                dgvrow.Cells[col].Value = period.Name;
                dgvrow.Tag = period.UID;

                dataGridViewX1.Rows.Add(dgvrow);
            }

            this.ResumeLayout();
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._initFinish)
            {
                validatePeriodName(dataGridViewX1.Rows[e.RowIndex]);
            }
        }

        private bool validatePeriodName(DataGridViewRow dgvrow)
        {
            if (string.IsNullOrEmpty(("" + dgvrow.Cells[1].Value).Trim()))
            {
                dgvrow.Cells[1].ErrorText = "欄位名稱不可空白!";
                return false;
            }
            else
            {
                dgvrow.Cells[1].ErrorText = null;
                return true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 資料驗證
            {
                int rowIndex = 0;
                List<string> listPeriodName = new List<string>();
                foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
                {
                    if (rowIndex == dataGridViewX1.Rows.Count - 1)
                    {
                        break;
                    }
                    rowIndex++;
                    if (validatePeriodName(dgvrow)) 
                    {
                        if (!listPeriodName.Contains(dgvrow.Cells[1].Value.ToString()))
                        {
                            listPeriodName.Add(dgvrow.Cells[1].Value.ToString());
                        }
                        else
                        {
                            MsgBox.Show("資料驗證失敗，無法儲存!");
                            dgvrow.Cells[1].ErrorText = "欄位名稱重複!";
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            #endregion

            #region 資料整理

            List<string> listData = new List<string>();
            int endRowIndex = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (endRowIndex == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                string data = string.Format(@"
SELECT
    '{0}'::TEXT AS name
    , {1}::BOOLEAN AS enabled
    , '{2}'::TEXT AS created_by
    , {3}::INTEGER AS uid
                ", dgvrow.Cells[1].Value, "" + dgvrow.Cells[0].Value == "True" ? "true" : "false", _userAccount, dgvrow.Tag == null ? "null" : dgvrow.Tag.ToString());

                listData.Add(data);

                endRowIndex++;
            }

            string dataRow = string.Join("UNION ALL", listData);
            #endregion

            try
            {
                DAO.Period.SavePeriodData(dataRow);
                MsgBox.Show("儲存成功!");
                ReloadDataGridView();
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
    }
}
