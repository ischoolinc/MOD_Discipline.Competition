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

        public frmSetScorePeriod()
        {
            InitializeComponent();
        }

        private void frmSetScorePeriod_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void ReloadDataGridView()
        {
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> listData = new List<string>();

            int endRowIndex = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (endRowIndex == dataGridViewX1.Rows.Count -1)
                {
                    break;
                }

                string data = string.Format(@"
SELECT
    '{0}'::TEXT AS name
    , {1}::BOOLEAN AS enabled
    , '{2}'::TEXT AS created_by
    , {3}::INTEGER AS uid
                ",dgvrow.Cells[1].Value,dgvrow.Cells[0].Value, _userAccount,dgvrow.Tag == null ? "null" : dgvrow.Tag.ToString());

                listData.Add(data);

                endRowIndex++;
            }

            string dataRow = string.Join("UNION ALL", listData);

            string sql = string.Format(@"
WITH data_row AS(
    {0}
) , update_data AS(
    UPDATE $ischool.discipline_competition.period AS period SET
        name = data_row.name
        , enabled =  data_row.enabled
    FROM
        data_row
    WHERE 
        data_row.uid = period.uid
) , insert_data AS(
    INSERT INTO $ischool.discipline_competition.period(
        name
        , enabled
        , created_by
    )
    SELECT
        name
        , enabled
        , created_by
    FROM
        data_row 
    WHERE
        data_row.uid IS NULL
)
    DELETE
    FROM
        $ischool.discipline_competition.period
    WHERE
        uid IN (
            SELECT
                period.uid
            FROM
                $ischool.discipline_competition.period AS period
                LEFT OUTER JOIN data_row
                    ON data_row.uid = period.uid
            WHERE
                data_row.uid IS NULL
        )
            ", dataRow);
            UpdateHelper up = new UpdateHelper();
            try
            {
                up.Execute(sql);
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
