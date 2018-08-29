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
    public partial class frmSetCheckItem : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Period> _dicPeriodByName = new Dictionary<string, UDT.Period>();
        private bool _initFinish = false;

        public frmSetCheckItem()
        {
            InitializeComponent();
        }

        private void frmSetCheckItem_Load(object sender, EventArgs e)
        {
            #region Init Period
            List<UDT.Period> listPeriod = _access.Select<UDT.Period>();

            foreach (UDT.Period period in listPeriod)
            {
                cbxPeriod.Items.Add(period.Name);
                _dicPeriodByName.Add(period.Name, period);
            }
            if (cbxPeriod.Items.Count > 0)
            {
                cbxPeriod.SelectedIndex = 0;
            }
            else
            {
                MsgBox.Show("請先設定評分時段資料!");
                this.Close();
            }
            #endregion

            // Init dgvColumn
            dgvAddOrCut.Items.Add("加分");
            dgvAddOrCut.Items.Add("減分");

            ReloadDataGridView();

            _initFinish = true;
        }

        private void cbxPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        /// <summary>
        /// Sort: 排列序號
        /// </summary>
        private class sortDisplayOrder : IComparer<UDT.CheckItem>
        {
            int IComparer<UDT.CheckItem>.Compare(UDT.CheckItem x, UDT.CheckItem y)
            {
                if (x.DisplayOrder > y.DisplayOrder)
                {
                    return 1;
                }
                if (x.DisplayOrder < y.DisplayOrder)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();

            string periodUID = _dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID;
            List<UDT.CheckItem>listCheckItem =_access.Select<UDT.CheckItem>(string.Format("ref_period_id = {0}", periodUID));
            listCheckItem.Sort(new sortDisplayOrder());

            foreach (UDT.CheckItem checkItem in listCheckItem)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = checkItem.Enabled;
                dgvrow.Cells[col++].Value = checkItem.Name;
                if (checkItem.MinScore == 0)
                {
                    dgvrow.Cells[col++].Value = "加分";
                    dgvrow.Cells[col++].Value = checkItem.MaxScore;
                }
                else
                {
                    dgvrow.Cells[col++].Value = "減分";
                    dgvrow.Cells[col++].Value = (checkItem.MinScore * -1);
                }
                //dgvrow.Cells[col++].Value = checkItem.MaxScore;
                //dgvrow.Cells[col++].Value = checkItem.MinScore;
                dgvrow.Cells[col++].Value = "" + checkItem.DisplayOrder == "0" ? "" : "" + checkItem.DisplayOrder;
                dgvrow.Tag = checkItem;

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 資料驗證
            if (!Validate())
            {
                MsgBox.Show("資料驗證失敗，無法儲存!");
                return;
            }

            List<string> listData = new List<string>();
            int rowIndex = 0;
            string periodID = _dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID;

            #region 資料整理
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (rowIndex == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                rowIndex++;
                string uid = dgvrow.Tag == null ? "null" : ((UDT.CheckItem)dgvrow.Tag).UID;
                string checkItemName = "" + dgvrow.Cells[1].Value;
                string enabled = ("" + dgvrow.Cells[0].Value) == "True" ? "true" : "false";
                string displayOrder = ("" + dgvrow.Cells[4].Value) == "" ? "null" : ("" + dgvrow.Cells[4].Value);
                string maxScore = "";
                string minScore = "";
                if ("" + dgvrow.Cells[2].Value == "加分")
                {
                    maxScore = "" + dgvrow.Cells[3].Value;
                    minScore = "0";
                }
                else
                {
                    maxScore = "0";
                    minScore = "-" + dgvrow.Cells[3].Value;
                }

                string data = string.Format(@"
SELECT
    {0}::INTEGER AS uid
    , {1}::INTEGER AS ref_period_id
    , '{2}'::TEXT AS name
    , {3}::BOOLEAN AS enabled
    , {4}::INTEGER AS display_order
    , {5}::INTEGER AS max_score
    , {6}::INTEGER AS min_score
                ", uid, periodID, checkItemName, enabled, displayOrder, maxScore, minScore);

                listData.Add(data);
            } 
            #endregion

            string dataRow = string.Join("UNION ALL",listData);
            
            try
            {
                DAO.CheckItem.SaveCheckItem(dataRow, periodID);
                MsgBox.Show("資料儲存成功!");

                ReloadDataGridView();
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            } 
        }

        private bool dgvName_Validate(DataGridViewRow dgvrow)
        {
            if (string.IsNullOrEmpty("" + dgvrow.Cells[1].Value))
            {
                dgvrow.Cells[1].ErrorText = "評分項目欄位不可空白!";
                return false;
            }
            else
            {
                dgvrow.Cells[1].ErrorText = null;
                return true;
            }
        }

        private bool dgvAddorCut_Validate(DataGridViewRow dgvrow)
        {
            if (string.IsNullOrEmpty("" + dgvrow.Cells[2].Value))
            {
                //dgvrow.Cells[2].ErrorText = "加減分欄位不可空白!";
                dgvrow.ErrorText = "加減分欄位不可空白!";
                return false;
            }
            else
            {
                if (dgvAddOrCut.Items.Contains("" + dgvrow.Cells[2].Value))
                {
                    dgvrow.Cells[2].ErrorText = null;
                    return true;
                }
                else
                {
                    //dgvrow.Cells[2].ErrorText = "加減分欄位只允許填入加分或減分!";
                    dgvrow.ErrorText = "加減分欄位只允許填入加分或減分!";
                    return false;
                }
            }
        }

        private bool dgvMaxScore_Validate(DataGridViewRow dgvrow)
        {
            int n;
            if (string.IsNullOrEmpty("" + dgvrow.Cells[3].Value) || "" + dgvrow.Cells[3].Value == "0")
            {
                //dgvrow.Cells[3].ErrorText = "最大值欄位不可空白、不為零!";
                dgvrow.ErrorText = "最大值欄位不可空白、不為零!";
                return false;
            }
            else
            {
                if (!int.TryParse("" + dgvrow.Cells[3].Value, out n))
                {
                    dgvrow.Cells[3].ErrorText = "最大值欄位只允許填入數值!";
                    return false;
                }
                else
                {
                    dgvrow.Cells[3].ErrorText = null;
                    return true;
                }
            }
        }

        private bool dgvDisplayOrder_Validate(DataGridViewRow dgvrow)
        {
            int n = 0;
            if (!string.IsNullOrEmpty("" + dgvrow.Cells[4].Value))
            {
                if (!int.TryParse("" + dgvrow.Cells[4].Value, out n))
                {
                    dgvrow.Cells[4].ErrorText = "排列序號欄位指允許填入數值!";
                    return false;
                }
                else
                {
                    dgvrow.Cells[4].ErrorText = null;
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 資料驗證
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            int rowIndex = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if(rowIndex == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                rowIndex++;

                // 驗證評分項目
                if (!dgvName_Validate(dgvrow))
                {
                    return false;
                }

                // 驗證加減分欄位
                if (!dgvAddorCut_Validate(dgvrow))
                {
                    return false;
                }

                // 驗證加減分最大值欄位
                if (!dgvMaxScore_Validate(dgvrow))
                {
                    return false;
                }

                // 驗證排列序號欄位
                if (!dgvDisplayOrder_Validate(dgvrow))
                {
                    return false;
                }
            }

            return true;
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                dgvName_Validate(dataGridViewX1.Rows[e.RowIndex]);
            }
            if (e.RowIndex > -1 && e.ColumnIndex == 2)
            {
                dgvAddorCut_Validate(dataGridViewX1.Rows[e.RowIndex]);
            }
            if (e.RowIndex > -1 && e.ColumnIndex == 3)
            {
                dgvMaxScore_Validate(dataGridViewX1.Rows[e.RowIndex]);
            }
            if (e.RowIndex > -1 && e.ColumnIndex == 4)
            {
                dgvDisplayOrder_Validate(dataGridViewX1.Rows[e.RowIndex]);
            }
        }
    }
}
