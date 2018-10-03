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
            #region 身分
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
            }
            #endregion

            #region 時間
            {
                lbLastDate.Text = DateTime.Parse("" + this._row["scorer_last_update"]).ToString("yyyy/MM/dd");
                lbTime.Text = DateTime.Parse("" + this._row["scorer_last_update"]).ToString("HH:mm");
            }
            #endregion

            tbxClassName.Text = "" + this._row["class_name"];

            #region 評分項目
            {
                if (string.IsNullOrEmpty("" + this._row["check_item_name"]))
                {
                    errorProvider1.SetError(tbxCheckItem,"原評分項目已被刪除!");
                    this.btnSave.Enabled = false;
                }
                else
                {
                    tbxCheckItem.Text = "" + this._row["check_item_name"];
                    int minScore = ("" + this._row["min_score"]) == "" ? 0 : int.Parse("" + this._row["min_score"]);
                    int maxScore = ("" + this._row["max_score"]) == "" ? 0 : int.Parse("" + this._row["max_score"]);
                    int score = ("" + this._row["score"]) == "" ? 0 : int.Parse("" + this._row["score"]);
                    for (int s = minScore; s <= maxScore; s++)
                    {
                        cbxScore.Items.Add(s);
                    }
                    if (cbxScore.Items.IndexOf(score) == -1)
                    {
                        errorProvider1.SetError(cbxScore, "評分紀錄的加減分不在設定範圍內，請重新設定!");
                    }
                    else
                    {
                        cbxScore.SelectedIndex = cbxScore.Items.IndexOf(score);
                    }
                }
            }
            #endregion

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
                try
                {
                    DAO.ScoreSheet.UpdateScoreSheet(this._scoreSheetUID, tbxSeatNo.Text.Trim(), tbxCoordinate.Text.Trim(), tbxRemark.Text.Trim(), tbxPic1URL.Text.Trim(), tbxPic1Comment.Text.Trim(), tbxPic2URL.Text.Trim(), tbxPic2Comment.Text.Trim(), ckbxIsCancel.Checked, this._userName, this._userAccount, tbxCancelReason.Text.Trim(), cbxScore.SelectedItem.ToString());
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

        private void tbxSeatNo_TextChanged(object sender, EventArgs e)
        {
            tbxSeatNo_Validate();   
        }

        private void tbxCoordinate_TextChanged(object sender, EventArgs e)
        {
            tbxCoordinate_Validate();
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

        private bool SeatNoCoordinate_Validate()
        {
            // 座號座標擇一輸入
            if (!string.IsNullOrEmpty(tbxSeatNo.Text) && !string.IsNullOrEmpty(tbxCoordinate.Text))
            {
                errorProvider1.SetError(tbxSeatNo, "座號座標擇一輸入!");
                errorProvider1.SetError(tbxCoordinate, "座號座標擇一輸入!");
                return false;
            }
            else
            {
                errorProvider1.SetError(tbxSeatNo, null);
                errorProvider1.SetError(tbxCoordinate, null);
                return true;
            }
        }

        private bool tbxSeatNo_Validate()
        {
            if (string.IsNullOrEmpty(tbxSeatNo.Text))
            {
                errorProvider1.SetError(tbxSeatNo,null);
                return true;
            }
            else
            {
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
                errorProvider1.SetError(tbxCoordinate,null);
                return true;
            }
            else
            {
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
            // 驗證違規之座號 
            if (!tbxSeatNo_Validate())
            {
                return false;
            }
            // 驗證違規之座標
            if (!tbxCoordinate_Validate())
            {
                return false;
            }
            // 驗證取消原因
            if (!tbxCancelReason_Validate())
            {
                return false;
            }
            // 驗證成功
            else
            {
                return true;
            }
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

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
