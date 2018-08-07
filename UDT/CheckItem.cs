using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition.UDT
{
    /// <summary>
    /// 評分項目
    /// </summary>
    [TableName("ischool.discipline_competition.check_item")]
    class CheckItem : ActiveRecord
    {
        /// <summary>
        /// 時段系統編號
        /// </summary>
        [Field(Field = "ref_period_id", Indexed = false)]
        public int RefPeriodID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field = "name", Indexed = false)]
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Field(Field = "enabled", Indexed = false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Field(Field = "create_time", Indexed = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 顯示順序
        /// </summary>
        [Field(Field = "display_order", Indexed = false)]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 加減分最大值
        /// </summary>
        [Field(Field = "max_score", Indexed = false)]
        public int MaxScore { get; set; }

        /// <summary>
        /// 加減分最小值
        /// </summary>
        [Field(Field = "min_score", Indexed = false)]
        public int MinScore { get; set; }

        /// <summary>
        /// 建立者帳號
        /// </summary>
        [Field(Field = "created_by", Indexed = false)]
        public string CreatedBy { get; set; }
    }
}
