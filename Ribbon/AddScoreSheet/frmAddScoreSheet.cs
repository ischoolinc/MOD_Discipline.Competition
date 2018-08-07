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
using K12.Data;

namespace Ischool.discipline_competition
{
    public partial class frmAddScoreSheet : BaseForm
    {
        private DateTime _dateTime;
        private AccessHelper _access = new AccessHelper();

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
            lbDateTime.Text = this._dateTime.ToShortDateString();

            // Init Class
            {
                List<ClassRecord> listClassRecord = Class.SelectAll();
                foreach (ClassRecord cr in listClassRecord)
                {
                    cbxClass.Items.Add(cr.Name);
                    this._dicClassIDBYName.Add(cr.Name,cr.ID);
                }
            }

            // Init Period
            {
                List<UDT.Period> listPeriod = _access.Select<UDT.Period>();

                foreach (UDT.Period period in listPeriod)
                {
                    cbxPeriod.Items.Add(period.Name);
                    _dicPeriodByName.Add(period.Name,period);
                }
                if (cbxPeriod.Items.Count > 0)
                {
                    cbxPeriod.SelectedIndex = 0;
                }
            }
            // Init CheckItem
            //ReloadCheckItem();

            // Init Score
            //ReloadScore();
        }

        private void ReloadCheckItem()
        {
            cbxCheckItem.Items.Clear();

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
                ss.CreateTime = DateTime.Now;
                ss.Score = int.Parse(cbxScore.SelectedItem.ToString());
                ss.SchoolYear = int.Parse(School.DefaultSchoolYear);
                ss.Semester = int.Parse(School.DefaultSemester);

                listInsertData.Add(ss);

                // 寫入資料庫
                try
                {
                    this._access.InsertValues(listInsertData);
                    MsgBox.Show("儲存成功!");
                    this.Close();
                }
                catch(Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }

        private bool Validate()
        {
            // 驗證班級 : 不能空白
            if (string.IsNullOrWhiteSpace(cbxClass.SelectedItem.ToString()))
            {
                MsgBox.Show("違規班級不能空白!");
                return false;
            }
            // 驗證評分時段 : 不能空白
            if (string.IsNullOrWhiteSpace(cbxPeriod.SelectedItem.ToString()))
            {
                MsgBox.Show("評分時段不能空白!");
                return false;
            }
            // 驗證評分項目
            if (string.IsNullOrWhiteSpace(cbxCheckItem.SelectedItem.ToString()))
            {
                MsgBox.Show("評分項不能空白!");
                return false;
            }
            // 驗證違規之座號 & 違規之座標
            if (string.IsNullOrWhiteSpace(tbxSeatNo.Text) && string.IsNullOrWhiteSpace(tbxCoordinate.Text))
            {
                MsgBox.Show("違規之座號與違規之座標請擇一輸入!");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(tbxSeatNo.Text) && !string.IsNullOrWhiteSpace(tbxCoordinate.Text))
            {
                MsgBox.Show("違規之座號與違規之座標請擇一輸入!");
                return false;
            }
            // 驗證評分加減分
            if (string.IsNullOrWhiteSpace(cbxScore.Items.ToString()))
            {
                MsgBox.Show("評分加減分不能空白!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadCheckItem();
        }

        private void cbxCheckItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadScore();
        }
    }
}
