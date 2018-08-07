using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition.UDT
{
    /// <summary>
    /// 週統計
    /// </summary>
    [TableName("ischool.discipline_competition.weekly_stats")]
    class WeeklyStats : ActiveRecord
    {
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
        /// 開始日期
        /// </summary>
        [Field(Field = "start_date", Indexed = false)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Field(Field = "end_date", Indexed = false)]
        public DateTime EndDate { get; set; }
    }
}
