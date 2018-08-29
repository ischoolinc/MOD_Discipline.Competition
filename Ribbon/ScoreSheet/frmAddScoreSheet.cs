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
using FISCA.UDT;
using FISCA.Data;

namespace Ischool.discipline_competition
{
    public partial class frmAddScoreSheet : BaseForm
    {
        private DateTime _dateTime;
        private AccessHelper _access = new AccessHelper();
        private QueryHelper _qh = new QueryHelper();
        private string _userAccount = DAO.Actor.Instance.GetUserAccount();
        private string _userName = DAO.Actor.Instance.GetUserName();

        private Dictionary<string, UDT.Period> _dicPeriodByName = new Dictionary<string, UDT.Period>();
        private Dictionary<string, string> _dicClassIDBYName = new Dictionary<string, string>();
        private Dictionary<string, UDT.CheckItem> _dicCheckItemBYName = new Dictionary<string, UDT.CheckItem>();

        public frmAddScoreSheet(DateTime dateTime)
        {
            InitializeComponent();

            this._dateTime = dateTime;
        }

        private void frmAddScoreSheet_Load(object sender, EventArgs e)
        {
            lbAccount.Text = DAO.Actor.Instance.GetUserAccount();

            dateTimeInput1.Value = DateTime.Now;

            #region Init Class
            {
                string sql = @"
SELECT
    *
FROM
    class
ORDER BY
    grade_year
    , class_name
    , display_order
";
                DataTable dt = this._qh.Select(sql);

                foreach (DataRow row in dt.Rows)
                {
                    cbxClass.Items.Add("" + row["class_name"]);
                    this._dicClassIDBYName.Add("" + row["class_name"], "" + row["id"]);
                }
            }
            #endregion

            #region Init Period
            {
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
            } 
            #endregion
        }

        private void ReloadCheckItem()
        {
            cbxCheckItem.Items.Clear();
            this._dicCheckItemBYName.Clear();

            List<UDT.CheckItem> listCheckItem = this._access.Select<UDT.CheckItem>(string.Format("ref_period_id = {0}",this._dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID));

            foreach (UDT.CheckItem checkItem in listCheckItem)
            {
                cbxCheckItem.Items.Add(checkItem.Name);
                this._dicCheckItemBYName.Add(checkItem.Name,checkItem);
            }

            if (cbxCheckItem.Items.Count > 0 )
            {
                cbxCheckItem.SelectedIndex = 0;
            }
        }

        private void ReloadScore()
        {
            cbxScore.Items.Clear();

            int maxScore = this._dicCheckItemBYName[cbxCheckItem.SelectedItem.ToString()].MaxScore;
            int minScore = this._dicCheckItemBYName[cbxCheckItem.SelectedItem.ToString()].MinScore;

            for (int i = minScore; i <=  maxScore; i++ )
            {
                cbxScore.Items.Add(i);
            }
            if (cbxScore.Items.Count > 0)
            {
                cbxScore.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                List<UDT.ScoreSheet> listInsertData = new List<UDT.ScoreSheet>();
                // 資料整理
                UDT.ScoreSheet ss = new UDT.ScoreSheet();
                ss.Account = lbAccount.Text;
                ss.RefCheckItemID = int.Parse(this._dicCheckItemBYName[cbxCheckItem.SelectedItem.ToString()].UID);
                ss.RefClassID = int.Parse(this._dicClassIDBYName[cbxClass.SelectedItem.ToString()]);
                ss.SeatNo = tbxSeatNo.Text;
                ss.Coordinate = tbxCoordinate.Text;
                ss.Remark = tbxRemark.Text;
                ss.CreateTime = dateTimeInput1.Value;
                ss.ScoreLastUpdate = dateTimeInput1.Value;
                ss.Score = int.Parse(cbxScore.SelectedItem.ToString());
                ss.SchoolYear = int.Parse(School.DefaultSchoolYear);
                ss.Semester = int.Parse(School.DefaultSemester);

                listInsertData.Add(ss);

                string log = GetLogString(ss);

                // 寫入資料庫
                try
                {
                    this._access.InsertValues(listInsertData);
                    FISCA.LogAgent.ApplicationLog.Log("秩序競賽", "新增評分紀錄", log);
                    MsgBox.Show("儲存成功!");
                    this.Close();
                }
                catch(Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }

        private string GetLogString(UDT.ScoreSheet ss)
        {
            StringBuilder logs = new StringBuilder();

            logs.AppendLine(string.Format("管理員「{0}」新增評分紀錄:\n 違規班級「{1}」\n 評分時段「{2}」 評分項目「{3}」\n 違規之座號「{4}」 違規之座標「{5}」\n 評分加減分「{6}」 \n 補充說明「{7}」\n 建立日期「{8}」"
                , this._userName,cbxClass.SelectedItem.ToString(),cbxPeriod.SelectedItem.ToString(),cbxCheckItem.SelectedItem.ToString(),tbxSeatNo.Text,tbxCoordinate.Text,cbxScore.SelectedItem.ToString(),tbxRemark.Text,dateTimeInput1.Value.ToString("yyyy/MM/dd HH:MM")));

            return logs.ToString();
        }

        private bool cbxClass_Validate()
        {
            if (this._dicClassIDBYName.Keys.Contains(cbxClass.Text))
            {
                errorProvider1.SetError(cbxClass, null);
                return true;
            }
            else
            {
                errorProvider1.SetError(cbxClass, "此班級名稱不存在!");
                return false;
            }
        }

        private bool cbxPeriod_Validate()
        {
            if (cbxPeriod.Items.Count == 0)
            {
                errorProvider1.SetError(cbxPeriod,"請先設定評分時段");
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxPeriod, null);
                return true;
            }
        }

        private bool cbxCheckItem_Validate()
        {
            if (cbxCheckItem.Items.Count == 0)
            {
                errorProvider1.SetError(cbxCheckItem,"請先設定時段的評分項目");
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxCheckItem,null);
                return true;
            }
        }

        private bool tbxSeatNo_Validate()
        {
            if (string.IsNullOrEmpty(tbxSeatNo.Text))
            {
                errorProvider1.SetError(tbxSeatNo,"違規座號不可空白!");
                return false;
            }
            else
            {
                errorProvider1.SetError(tbxCoordinate, null);
                string [] seatNos = tbxSeatNo.Text.Split(',');
                int n = 0;
                bool validateSuccess = false;
                foreach (string seatNo in seatNos)
                {
                    if (!int.TryParse(seatNo,out n))
                    {
                        validateSuccess = false;
                    }
                    else
                    {
                        validateSuccess = true;
                    }
                }
                if (!validateSuccess)
                {
                    errorProvider1.SetError(tbxSeatNo, "格式錯誤，座號之間請用「\",\"」分開!");
                    return validateSuccess;
                }
                else
                {
                    errorProvider1.SetError(tbxSeatNo, null);
                    return validateSuccess;
                }
            }
        }

        private bool tbxCoordinate_Validate()
        {
            if (string.IsNullOrEmpty(tbxCoordinate.Text))
            {
                errorProvider1.SetError(tbxCoordinate, "違規座標不可空白!");
                return false;
            }
            else
            {
                errorProvider1.SetError(tbxSeatNo, null);
                string[] coordinates = tbxCoordinate.Text.Split(',');
                bool validateSuccess = false;
                foreach (string coordinate in coordinates)
                {
                    if (string.IsNullOrEmpty(coordinate))
                    {
                        validateSuccess = false;
                    }
                    else
                    {
                        validateSuccess = true;
                    }
                }
                if (!validateSuccess)
                {
                    errorProvider1.SetError(tbxCoordinate, "格式錯誤，座標之間請用「\",\"」分開!");
                    return validateSuccess;
                }
                else
                {
                    errorProvider1.SetError(tbxCoordinate, null);
                    return validateSuccess;
                }
            }
        }

        private bool cbxScore_Validate()
        {
            if (cbxScore.Items.Count == 0)
            {
                errorProvider1.SetError(cbxScore,"請先設定評分項目加減分數!");
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxScore, null);
                return true;
            }
        }

        private bool Validate()
        {
            // 驗證班級
            if (!cbxClass_Validate())
            {
                return false;
            }

            // 驗證評分時段
            if (!cbxPeriod_Validate())
            {
                return false;
            }
            
            // 驗證評分項目
            if (!cbxCheckItem_Validate())
            {
                return false;
            }

            // 驗證違規之座號 & 違規之座標
            if ((string.IsNullOrEmpty(tbxSeatNo.Text) && string.IsNullOrEmpty(tbxCoordinate.Text)) || (!string.IsNullOrWhiteSpace(tbxSeatNo.Text) && !string.IsNullOrWhiteSpace(tbxCoordinate.Text)))
            {
                errorProvider1.SetError(tbxSeatNo, "違規之座號與違規之座標請擇一輸入!");
                errorProvider1.SetError(tbxCoordinate, "違規之座號與違規之座標請擇一輸入!");

                return false;
            }
            if (string.IsNullOrEmpty(tbxSeatNo.Text) && !string.IsNullOrEmpty(tbxCoordinate.Text))
            {
                if (!tbxCoordinate_Validate())
                {
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(tbxSeatNo.Text) && string.IsNullOrEmpty(tbxCoordinate.Text))
            {
                if (!tbxSeatNo_Validate())
                {
                    return false;
                }
            }
            
            // 驗證評分加減分
            if (!cbxScore_Validate())
            {
                return false;
            }

            // 驗證成功
            return true;
            
        }

        private void cbxPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadCheckItem();
        }

        private void cbxCheckItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadScore();
        }

        private void cbxClass_TextChanged(object sender, EventArgs e)
        {
            cbxClass.SelectedIndex = cbxClass.Items.IndexOf(cbxClass.Text);
            cbxClass_Validate();
        }

        private void tbxSeatNo_TextChanged(object sender, EventArgs e)
        {
            tbxSeatNo_Validate();
        }

        private void tbxCoordinate_TextChanged(object sender, EventArgs e)
        {
            tbxCoordinate_Validate();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
