using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.discipline_competition.DAO
{
    class WeeklyRank
    {
        public static DataTable GetWeekltRank(string schoolYear,string semester,int weekNumber)
        {
            string sql = string.Format(@"
SELECT
    rank.*
    , class.class_name
FROM
    $ischool.discipline_competition.weekly_rank AS rank
    LEFT OUTER JOIN class
        ON class.id = rank.ref_class_id
WHERE
    school_year = {0}
    AND semester = {1}
    AND week_number = {2}
ORDER BY
    rank.grade_year
    , rank.rank
    , class.display_order
    , class.class_name
            ", schoolYear, semester, weekNumber);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return dt;
        }
    }
}
