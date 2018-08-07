using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition.UDT
{
    /// <summary>
    /// 學期統計
    /// </summary>
    [TableName("ischool.discipline_competition.semester_stats")]
    class SemesterStats : ActiveRecord
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
        public int Semester { get; set; }

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
        /// 學期週排名次加總
        /// </summary>
        [Field(Field = "rank_total", Indexed = false)]
        public int RankTotal { get; set; }

        /// <summary>
        /// 學期週總分平均
        /// </summary>
        [Field(Field = "average_score", Indexed = false)]
        public int AverageScore { get; set; }

        /// <summary>
        /// 學年期週排前3名總次數
        /// </summary>
        //[Field(Field = "top3_count", Indexed = false)]
        //public int RefWeeklyStatsID { get; set; }

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
    }
}
