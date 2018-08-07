using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;
namespace Ischool.discipline_competition.UDT
{
    /// <summary>
    /// 發佈的週排行(發佈後不能再修改)
    /// </summary>
    [TableName("ischool.discipline_competition.weekly_rank")]
    class WeeklyRank : ActiveRecord
    {
        /// <summary>
        /// 週統計編號
        /// </summary>
        [Field(Field = "ref_weekly_stats_id", Indexed = false)]
        public int RefWeeklyStatsID { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Field = "school_year", Indexed = false)]
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        [Field(Field = "semester", Indexed = false)]
        public int semester { get; set; }

        /// <summary>
        /// 班級編號
        /// </summary>
        [Field(Field = "ref_class_id", Indexed = false)]
        public int RefClassID { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        [Field(Field = "grade_year", Indexed = false)]
        public int GradeYear { get; set; }

        /// <summary>
        /// 週總分
        /// </summary>
        [Field(Field = "week_total", Indexed = false)]
        public int WeekTotal { get; set; }

        /// <summary>
        /// 週次
        /// </summary>
        [Field(Field = "week_number", Indexed = false)]
        public int WeekNumber { get; set; }

        /// <summary>
        /// 產生日期
        /// </summary>
        [Field(Field = "create_time", Indexed = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 產生者帳號
        /// </summary>
        [Field(Field = "created_by", Indexed = false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 週排名
        /// </summary>
        [Field(Field = "rank", Indexed = false)]
        public int Rank { get; set; }

        /// <summary>
        /// 前2名已連續幾週
        /// </summary>
        [Field(Field = "top2_in_a_row", Indexed = false)]
        public int Top2InARow { get; set; }

        /// <summary>
        /// 前3名已連續幾週
        /// </summary>
        [Field(Field = "top3_in_a_row", Indexed = false)]
        public int Top3InARow { get; set; }

        /// <summary>
        /// 本次已達成重新計算條件
        /// </summary>
        [Field(Field = "need_reset", Indexed = false)]
        public bool NeedReset { get; set; }

        /// <summary>
        /// 發佈日期
        /// </summary>
        [Field(Field = "public_time", Indexed = false)]
        public DateTime PublicTime { get; set; }

        /// <summary>
        /// 發佈者帳號
        /// </summary>
        [Field(Field = "public_by", Indexed = false)]
        public string PublicBy { get; set; }
    }
}
