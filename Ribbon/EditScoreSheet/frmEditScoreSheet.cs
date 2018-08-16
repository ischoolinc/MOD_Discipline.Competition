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
    public partial class frmEditScoreSheet : BaseForm
    {
        private DataRow _row;
        private AccessHelper _access = new AccessHelper();
        private UpdateHelper _up = new UpdateHelper();
        private string _scoreSheetUID;
        private string _userAccount = DAO.Actor.Instance.GetUserAccount();
        private string _userName = DAO.Actor.Instance.GetUserName();

        public frmEditScoreSheet(DataRow row)
        {
            InitializeComponent();

            this._row = row;
            this._scoreSheetUID = "" + row["uid"];

        }

        private void frmEditScoreSheet_Load(object sender, EventArgs e)
        {
            lbIdentity.Text = "" + this._row["身分"];
            if (lbIdentity.Text == "管理員")
            {
                lbScorer.Text = "" + this._row["teacher_name"];
            }
            else
            {
                lbScorer.Text = "" + this._row["student_name"];
            }
            tbxCheckItem.Text = "" + this._row["check_item_name"];
            lbLastDate.Text = DateTime.Parse("" + this._row["scorer_last_update"]).ToString("yyyy/MM/dd HH:mm");
            tbxClassName.Text = "" + this._row["class_name"];
            //tbxScore.Text = "" + _row["score"];
            int minScore = int.Parse("" + this._row["min_score"]);
            int maxScore = int.Parse("" + this._row["max_score"]);
            int score = int.Parse("" + this._row["score"]);
            for (int s = minScore; s <= maxScore; s++)
            {
                cbxScore.Items.Add(s);
            }
            cbxScore.SelectedIndex = cbxScore.Items.IndexOf(score);
            tbxSeatNo.Text = "" + this._row["seat_no"];
            tbxCoordinate.Text = "" + this._row["coordinate"];
            tbxRemark.Text = "" + this._row["remark"];
            pbx1.ImageLocation = "" + this._row["picture1"];
            pbx2.ImageLocation = "" + this._row["picture2"];
            tbxPic1URL.Text = "" + this._row["picture1"];
            tbxPic2URL.Text = "" + this._row["picture2"];
            tbxPic1Comment.Text = "" + this._row["pic1_comment"];
            tbxPic2Comment.Text = "" + this._row["pic2_comment"];

            ckbxIsCancel.Checked = ("" + this._row["is_canceled"]) == "true" ? true : false;
            tbxCancelReason.Text = "" + this._row["cancel_reason"];
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                // 建立Log紀錄
                string log = GetLogString();

                #region DataRow
                // 資料整理
                string dataRow = string.Format(@"
SELECT
    {0}::BIGINT AS uid
    , '{1}'::TEXT AS seat_no
    , '{2}'::TEXT AS coordinate
    , '{3}'::TEXT AS remark
    , '{4}'::TEXT AS picture1
    , '{5}'::TEXT AS pic1_comment
    , '{6}'::TEXT AS picture2
    , '{7}'::TEXT AS pic2_comment
    , {8}::BOOLEAN AS is_canceled
    , '{9}'::TIMESTAMP AS canceled_time
    , '{10}'::TEXT AS canceled_name
    , '{11}'::TEXT AS cnaceled_by
    , '{12}'::TEXT AS cancel_reason
                ", _scoreSheetUID
                        , tbxSeatNo.Text.Trim()
                        , tbxCoordinate.Text.Trim()
                        , tbxRemark.Text.Trim()
                        , tbxPic1URL.Text.Trim()
                        , tbxPic1Comment.Text.Trim()
                        , tbxPic2URL.Text.Trim()
                        , tbxPic2Comment.Text.Trim()
                        , ckbxIsCancel.Checked
                        , DateTime.Now.ToString("yyyy/MM/dd HH:mm")
                        , this._userName
                        , this._userAccount
                        , tbxCancelReason.Text
                        );
                #endregion

                #region SQL
                string sql = string.Format(@"
WITH data_row AS(
    {0}
)
UPDATE $ischool.discipline_competition.score_sheet SET
    seat_no = data_row.seat_no
    , coordinate = data_row.coordinate
    , remark = data_row.remark
    , picture1 = data_row.picture1
    , pic1_comment = data_row.pic1_comment
    , picture2 = data_row.picture2
    , pic2_comment = data_row.pic2_comment
    , is_canceled = data_row.is_canceled
    , canceled_time = data_row.canceled_time
    , canceled_name = data_row.canceled_name
    , canceled_by = data_row.cnaceled_by
    , cancel_reason = data_row.cancel_reason
FROM
    data_row
WHERE
    $ischool.discipline_competition.score_sheet.uid = data_row.uid
                ", dataRow);
                
                #endregion
                
                try
                {
                    this._up.Execute(sql);
                    FISCA.LogAgent.ApplicationLog.Log("秩序競賽", "修改評分紀錄", log);
                    MsgBox.Show("資料儲存成功!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }

            }
        }

        private string GetLogString()
        {
            StringBuilder log = new StringBuilder();

            log.AppendLine(string.Format("「{0}」建立的評分紀錄，建立者為「{1}」{2}，違規班級「{3}」評分項目「{4}」： \n 原評分最後修改日期「{5}」變更為「{6}」"
                , "" + this._row["create_time"],lbScorer.Text,lbIdentity.Text,tbxClassName.Text,tbxCheckItem.Text,lbLastDate.Text,DateTime.Now.ToString("yyyy/MM/dd HH:MM")));

            if (cbxScore.SelectedItem.ToString() != "" + this._row["score"])
            {
                log.AppendLine(string.Format("\n 原評分加減分「{0}」變更為「{1}」", "" + this._row["score"],cbxScore.SelectedItem.ToString()));
            }
            if (tbxSeatNo.Text.Trim() != "" + this._row["seat_no"])
            {
                log.AppendLine(string.Format("\n 原違規座號「{0}」變更為「{1}」", "" + this._row["seat_no"], tbxSeatNo.Text.Trim()));
            }
            if (tbxCoordinate.Text.Trim() != "" + this._row["coordinate"])
            {
                log.AppendLine(string.Format("\n 原違規座標「{0}」變更為「{1}」", "" + this._row["coordinate"], tbxCoordinate.Text.Trim()));
            }
            if (tbxRemark.Text.Trim() != "" + this._row["remark"])
            {
                log.AppendLine(string.Format("\n 原備註「{0}」變更為「{1}」", "" + this._row["remark"], tbxRemark.Text.Trim()));
            }
            if (tbxPic1URL.Text.Trim() != "" + this._row["picture1"])
            {
                log.AppendLine(string.Format("\n 原照片1「{0}」變更為「{1}」", "" + this._row["picture1"], tbxPic1URL.Text.Trim()));
            }
            if (tbxPic1Comment.Text.Trim() != "" + this._row["pic1_comment"])
            {
                log.AppendLine(string.Format("\n 原照片1的說明「{0}」變更為「{1}」", "" + this._row["pic1_comment"], tbxPic1Comment.Text.Trim()));
            }
            if (tbxPic2URL.Text.Trim() != "" + this._row["picture2"])
            {
                log.AppendLine(string.Format("\n 原照片2「{0}」變更為「{1}」", "" + this._row["picture2"], tbxPic2URL.Text.Trim()));
            }
            if (tbxPic2Comment.Text.Trim() != "" + this._row["pic2_comment"])
            {
                log.AppendLine(string.Format("\n 原照片2的說明「{0}」變更為「{1}」", "" + this._row["pic2_comment"], tbxPic2Comment.Text.Trim()));
            }
            if (ckbxIsCancel.Checked.ToString() != "" + this._row["is_canceled"])
            {
                log.AppendLine(string.Format("\n 原評分紀錄是否取消「{0}」變更為「{1}」", ("" + this._row["is_canceled"]) == "true" ? "是" : "否", ckbxIsCancel.Checked.ToString() == "True" ? "是" : "否"));
            }
            if (tbxCancelReason.Text.Trim() != "" + this._row["cancel_reason"])
            {
                log.AppendLine(string.Format("\n 原取消原因「{0}」變更為「{1}」", "" + this._row["cancel_reason"], tbxCancelReason.Text.Trim()));
            }

            log.AppendLine(string.Format("\n 修改者為「{0}」管理員，修改者登入帳號為「{1}」", this._userName,this._userAccount));

            return log.ToString();
        }

        private bool cbxScore_Validate()
        {
            if (cbxScore.SelectedItem == null)
            {
                errorProvider1.SetError(cbxScore,"請選擇評分項目分數!");
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxScore,null);
                return true;
            }
        }

        private bool tbxSeatNo_Validate()
        {
            if (string.IsNullOrEmpty(tbxSeatNo.Text))
            {
                errorProvider1.SetError(tbxSeatNo, "違規座號不可空白!");
                return false;
            }
            else
            {
                errorProvider1.SetError(tbxCoordinate, null);
                string[] seatNos = tbxSeatNo.Text.Split(',');
                int n = 0;
                bool validateSuccess = false;
                foreach (string seatNo in seatNos)
                {
                    if (!int.TryParse(seatNo, out n))
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

        private bool tbxCancelReason_Validate()
        {
            if (ckbxIsCancel.Checked)
            {
                if (string.IsNullOrEmpty(tbxCancelReason.Text))
                {
                    errorProvider1.SetError(tbxCancelReason,"取消原因不可空白!");
                    return false;
                }
                else
                {
                    errorProvider1.SetError(tbxCancelReason,null);
                    return true;
                }
            }
            else
            {
                errorProvider1.SetError(tbxCancelReason,null);
                return true;
            }
        }

        private bool Validate()
        {
            // 驗證評分項目加減分
            if (!cbxScore_Validate())
            {
                return false;
            }

            // 驗證違規之座號 & 違規之座標
            if ((string.IsNullOrEmpty(tbxSeatNo.Text) && string.IsNullOrEmpty(tbxCoordinate.Text)) /*|| (!string.IsNullOrWhiteSpace(tbxSeatNo.Text) && !string.IsNullOrWhiteSpace(tbxCoordinate.Text))*/)
            {
                errorProvider1.SetError(tbxSeatNo, "違規之座號與違規之座標請擇一輸入!");
                errorProvider1.SetError(tbxCoordinate, "違規之座號與違規之座標請擇一輸入!");

                return false;
            }
            else
            {
                if (!string.IsNullOrEmpty(tbxSeatNo.Text) && string.IsNullOrEmpty(tbxCoordinate.Text)) 
                {
                    if (!tbxSeatNo_Validate())
                    {
                        return false;
                    }
                }
                if (string.IsNullOrEmpty(tbxSeatNo.Text) && !string.IsNullOrEmpty(tbxCoordinate.Text))
                {
                    if (!tbxCoordinate_Validate())
                    {
                        return false;
                    }
                }
            }

            // 驗證取消原因
            if (!tbxCancelReason_Validate())
            {
                return false;
            }

            // 驗證成功
            return true;
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckbxIsCancel_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxIsCancel.Checked)
            {
                tbxCanceledName.Text = this._userName;
                tbxCanceledBy.Text = this._userAccount;
                tbxCancelReason_Validate();
            }
            else
            {
                tbxCanceledName.Text = "";
                tbxCanceledBy.Text = "";
                tbxCancelReason.Text = "";
            }
        }

        private void tbxCancelReason_TextChanged(object sender, EventArgs e)
        {
            tbxCancelReason_Validate();
        }
    }
}
