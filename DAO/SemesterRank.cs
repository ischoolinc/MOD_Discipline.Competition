using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.discipline_competition.DAO
{
    class SemesterRank
    {
        public static DataTable GetSemesterRank(string schoolYear,string semester)
        {
            string sql = string.Format(@"
SELECT
    class.class_name
    , semester_rank.*
FROM
    $ischool.discipline_competition.semester_rank AS semester_rank
    LEFT OUTER JOIN class
        ON class.id = semester_rank.ref_class_id
WHERE
    school_year = {0}
    AND semester = {1}
ORDER BY
    semester_rank.rank
            ", schoolYear, semester);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return dt;
        }
    }
}
