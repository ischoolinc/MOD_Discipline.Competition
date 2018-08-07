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
            #endregion

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

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();

            string periodUID = _dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID;
            List<UDT.CheckItem>listCheckItem =_access.Select<UDT.CheckItem>(string.Format("ref_period_id = {0}", periodUID));

            foreach (UDT.CheckItem checkItem in listCheckItem)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = checkItem.Enabled;
                dgvrow.Cells[col++].Value = checkItem.Name;
                dgvrow.Cells[col++].Value = checkItem.MaxScore;
                dgvrow.Cells[col++].Value = checkItem.MinScore;
                dgvrow.Cells[col++].Value = checkItem.DisplayOrder;
                dgvrow.Tag = checkItem;

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 資料驗證
            Validate();

            List<string> listData = new List<string>();

            int rowIndex = 0;
            #region 資料整理
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (rowIndex == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                rowIndex++;
                string uid = dgvrow.Tag == null ? "null" : ((UDT.CheckItem)dgvrow.Tag).UID;
                string periodID = _dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID;
                string checkItemName = "" + dgvrow.Cells[1].Value;
                string enabled = ("" + dgvrow.Cells[0].Value) == "True" ? "true" : "false";
                string displayOrder = ("" + dgvrow.Cells[4].Value) == "" ? "null" : ("" + dgvrow.Cells[4].Value);
                string maxScore = "" + dgvrow.Cells[2].Value;
                string minScore = "" + dgvrow.Cells[3].Value;

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

            #region 資料儲存
            string sql = string.Format(@"
WITH data_row AS(
    {0}
) ,update_data AS(
	UPDATE $ischool.discipline_competition.check_item AS check_item SET
		name = data_row.name
		, enabled = data_row.enabled
		, display_order = data_row.display_order
		, max_score = data_row.max_score
		, min_score = data_row.min_score
	FROM
		data_row
	WHERE
		data_row.uid = check_item.uid
) ,insert_data AS(
	INSERT INTO $ischool.discipline_competition.check_item AS check_item(
		ref_period_id
		, name
		, enabled
		, display_order
		, max_score
		, min_score	
	)
	SELECT
		ref_period_id
		, name
		, enabled
		, display_order
		, max_score
		, min_score	
	FROM
		data_row
	WHERE
		data_row.uid IS NULL
)
DELETE
FROM
    $ischool.discipline_competition.check_item
WHERE
    uid IN(
        SELECT
        	check_item.uid
        FROM
        	$ischool.discipline_competition.check_item AS check_item
        	LEFT OUTER JOIN data_row
        		ON data_row.uid = check_item.uid
		WHERE
			data_row.uid IS NULL
            AND check_item.ref_period_id = data_row.ref_period_id
    )
            ", dataRow);

            UpdateHelper up = new UpdateHelper();
            try
            {
                up.Execute(sql);
                MsgBox.Show("資料儲存成功!");

                ReloadDataGridView();
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            } 
            #endregion

        }

        // 資料驗證
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
                // 評分項目
                if (string.IsNullOrEmpty("" + dgvrow.Cells[1].Value))
                {
                    MsgBox.Show("評分項目欄位必填不可空白!");
                    return false;
                }
                int n;
                // 加減分最大值
                if (string.IsNullOrEmpty("" + dgvrow.Cells[2].Value))
                {
                    MsgBox.Show("加減分最大值欄位必填不可空白!");
                }
                else
                {
                    if (!int.TryParse("" + dgvrow.Cells[2].Value, out n))
                    {
                        MsgBox.Show("加減分最大值欄位只允許填入數值!");
                        return false;
                    }
                }
                // 加減分最小值
                if (string.IsNullOrEmpty("" + dgvrow.Cells[3].Value))
                {
                    MsgBox.Show("加減分最小值欄位必填不可空白!");
                    return false;
                }
                else
                {
                    if (!int.TryParse("" + dgvrow.Cells[3].Value, out n))
                    {
                        MsgBox.Show("加減分最小值欄位只允許填入數值!");
                        return false;
                    }
                }
                // 排列序號
                if (!string.IsNullOrEmpty("" + dgvrow.Cells[4].Value))
                {
                    if (!int.TryParse("" + dgvrow.Cells[4].Value,out n))
                    {
                        MsgBox.Show("排列序號欄位指允許填入數值!");
                        return false;
                    }
                }
            }

            return true;
        }


        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
