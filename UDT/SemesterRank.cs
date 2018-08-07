using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition.UDT
{
    /// <summary>
    /// 發佈的學期排行(發佈後不能再修改)
    /// </summary>
    [TableName("ischool.discipline_competition.semester_rank")]
    class SemesterRank : ActiveRecord
    {
        /// <summary>
        /// 學期統計編號
        /// </summary>
        [Field(Field = "ref_semester_stats_id", Indexed = false)]
        public int RefSemesterStatsID { get; set; }

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
        /// 學期排名
        /// </summary>
        [Field(Field = "rank", Indexed = false)]
        public int Rank { get; set; }

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
