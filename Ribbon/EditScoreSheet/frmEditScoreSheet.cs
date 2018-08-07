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
            lbScorer.Text = "" + _row["student_name"];
            tbxCheckItem.Text = "" + _row["check_item_name"];
            lbLastDate.Text = DateTime.Parse("" + _row["scorer_last_update"]).ToString("yyyy/MM/dd HH:mm");
            tbxClassName.Text = "" + _row["class_name"];
            tbxScore.Text = "" + _row["score"];
            tbxSeatNo.Text = "" + _row["seat_no"];
            tbxCoordinate.Text = "" + _row["coordinate"];
            pbx1.ImageLocation = "" + _row["picture1"];
            pbx2.ImageLocation = "" + _row["picture2"];
            tbxPic1URL.Text = "" + _row["picture1"];
            tbxPic2URL.Text = "" + _row["picture2"];
            tbxPic1Comment.Text = "" + _row["pic1_comment"];
            tbxPic2Comment.Text = "" + _row["pic2_comment"];

            ckbxIsCancel.Checked = ("" + _row["is_canceled"]) == "true" ? true : false;
            tbxCancelReason.Text = "" + _row["cancel_reason"];
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                // 資料整理
                string dataRow = string.Format(@"
SELECT
    {0}::BIGINT AS uid
    , '{1}'::TEXT AS seat_no
    , '{2}'::TEXT AS coordinate
    , '{3}'::TEXT AS remark
    , '{4}'::TEXT AS pic1_comment
    , '{5}'::TEXT AS pic2_comment
    , {6}::BOOLEAN AS is_canceled
    , '{7}'::TIMESTAMP AS canceled_time
    , '{8}'::TEXT AS canceled_name
    , '{9}'::TEXT AS cnaceled_by
    , '{10}'::TEXT AS cancel_reason
                ",_scoreSheetUID
                , tbxSeatNo.Text
                , tbxCoordinate.Text
                , tbxRemark.Text
                , tbxPic1Comment.Text
                , tbxPic2Comment.Text
                , ckbxIsCancel.Checked
                , DateTime.Now.ToString("yyyy/MM/dd HH:mm")
                , _userName
                , _userAccount
                , tbxCancelReason.Text
                );
                // 資料儲存
                string sql = string.Format(@"
WITH data_row AS(
    {0}
)
UPDATE $ischool.discipline_competition.score_sheet SET
    seat_no = data_row.seat_no
    , coordinate = data_row.coordinate
    , remark = data_row.remark
    , pic1_comment = data_row.pic1_comment
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

                UpdateHelper up = new UpdateHelper();
                try
                {
                    up.Execute(sql);
                    MsgBox.Show("資料儲存成功!");
                    this.Close();
                }
                catch(Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
                
            }
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(tbxScore.Text.Trim()))
            {
                MsgBox.Show("評分加減分欄位不可空白!");
                return false;
            }
            else
            {
                int n;
                if (!int.TryParse(tbxScore.Text, out n))
                {
                    MsgBox.Show("評分加減分欄位只允許填入數值!");
                    return false;
                }
            }
            if(!string.IsNullOrEmpty(tbxSeatNo.Text) && !string.IsNullOrEmpty(tbxCoordinate.Text))
            {
                MsgBox.Show("違規座號與違規座標欄位請擇一輸入!");
                return false;
            }
            if (ckbxIsCancel.Checked)
            {
                if (string.IsNullOrEmpty(tbxCancelReason.Text))
                {
                    MsgBox.Show("取消原因欄位不可空白!");
                    return false;
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
