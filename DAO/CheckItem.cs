using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K12.Data;

namespace Ischool.discipline_competition.DAO
{
    class CheckItem
    {
        /// <summary>
        /// 設定評分項目_儲存功能
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="periodID"></param>
        public static void SaveCheckItem(string dataRow,string periodID)
        {
            string sql = "";

            if (string.IsNullOrEmpty(dataRow))
            {
                sql = string.Format("DELETE FROM $ischool.discipline_competition.check_item WHERE ref_period_id = {0}",periodID);
            }
            else
            {
                #region SQL
                sql = string.Format(@"
WITH data_row AS(
    {0}
) ,update_data AS(
	UPDATE $ischool.discipline_competition.check_item AS check_item SET
		name = data_row.name
		, enabled = data_row.enabled
		, display_order = data_row.display_order
		, max_score = data_row.max_score
		, min_score = data_row.min_score
	FROM
		data_row
	WHERE
		data_row.uid = check_item.uid
) ,insert_data AS(
	INSERT INTO $ischool.discipline_competition.check_item (
		ref_period_id
		, name
		, enabled
		, display_order
		, max_score
		, min_score	
	)
	SELECT
		ref_period_id
		, name
		, enabled
		, display_order
		, max_score
		, min_score	
	FROM
		data_row
	WHERE
		data_row.uid IS NULL
)
DELETE
FROM
    $ischool.discipline_competition.check_item
WHERE
    uid IN(
        SELECT
        	check_item.uid
        FROM
        	$ischool.discipline_competition.check_item AS check_item
        	LEFT OUTER JOIN data_row
        		ON data_row.uid = check_item.uid
                AND check_item.ref_period_id = data_row.ref_period_id
		WHERE
			data_row.uid IS NULL
            AND check_item.ref_period_id = {1}
    )
            ", dataRow,periodID); 
                #endregion
            }
            
            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);
        }
    }
}
