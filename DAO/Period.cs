using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using K12.Data;

namespace Ischool.discipline_competition.DAO
{
    class Period
    {
        /// <summary>
        /// 新增、修改、刪除
        /// </summary>
        public static void SavePeriodData(string dataRow)
        {
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
            up.Execute(sql);
        }
    }
}
