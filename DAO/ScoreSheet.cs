using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using K12.Data;
using FISCA.Data;

namespace Ischool.discipline_competition.DAO
{
    class ScoreSheet
    {
        private static QueryHelper _qh = new QueryHelper();

        private static UpdateHelper _up = new UpdateHelper();

        public static DataTable GetScoreSheetByCondition(string date,string isCancel)
        {
            string condition = "";
            if (string.IsNullOrEmpty(isCancel))
            {
                condition = "";
            }
            else
            {
                condition = string.Format(@"
    AND score_sheet.is_canceled = {0}::BOOLEAN
            ", isCancel);
            }

            string sql = string.Format(@"
SELECT
    class.class_name 
    , check_item.name AS check_item_name
    , check_item.max_score 
    , check_item.min_score
    , student.name AS student_name
    , teacher.teacher_name
    , CASE
        WHEN student.name IS NOT NULL THEN '評分員'
        WHEN teacher.teacher_name IS NOT NULL THEN '管理員'
        ELSE ''
        END AS 身分
    , score_sheet.*
FROM
    $ischool.discipline_competition.score_sheet AS score_sheet
    LEFT OUTER JOIN class
        ON class.id = score_sheet.ref_class_id
    LEFT OUTER JOIN $ischool.discipline_competition.check_item AS check_item
        ON check_item.uid = score_sheet.ref_check_item_id
    LEFT OUTER JOIN $ischool.discipline_competition.scorer AS scorer
        ON scorer.account = score_sheet.account
    LEFT OUTER JOIN student
        ON student.id = scorer.ref_student_id
    LEFT OUTER JOIN $ischool.discipline_competition.admin AS admin
        ON admin.account = score_sheet.account
    LEFT OUTER JOIN teacher
        ON teacher.id = admin.ref_teacher_id
WHERE
    score_sheet.create_time::DATE = '{0}'::DATE
    {1}
ORDER BY
    class.grade_year
    , class.display_order
    , check_item.display_order
            ", date, condition);

            return _qh.Select(sql);
        }

        public static void UpdateScoreSheet(string scoreSheetID,string seatNo,string coordinate,string remark,string pic1URL,string pic1Comment,string pic2URL,string pic2Comment,bool isCancel,string userName,string userAccount,string cancelReason,string score)
        {
            #region DataRow
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
    , {13}::BIGINT AS score
                ", scoreSheetID
                    , seatNo
                    , coordinate
                    , remark
                    , pic1URL
                    , pic1Comment
                    , pic2URL
                    , pic2Comment
                    , isCancel
                    , DateTime.Now.ToString("yyyy/MM/dd HH:mm")
                    , userName
                    , userAccount
                    , cancelReason
                    , score
                    );
            #endregion

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
    , score = data_row.score
FROM
    data_row
WHERE
    $ischool.discipline_competition.score_sheet.uid = data_row.uid
                ", dataRow);

            _up.Execute(sql);
        }
    }
}
