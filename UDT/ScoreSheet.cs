using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition.UDT
{
    /// <summary>
    /// 評分登記總表
    /// </summary>
    [TableName("ischool.discipline_competition.score_sheet")]
    class ScoreSheet : ActiveRecord
    {
        /// <summary>
        /// 評分員帳號
        /// </summary>
        [Field(Field = "account", Indexed = false)]
        public string Account { get; set; }

        /// <summary>
        /// 評分項目編號
        /// </summary>
        [Field(Field = "ref_check_item_id", Indexed = false)]
        public int RefCheckItemID { get; set; }

        /// <summary>
        /// 班級編號
        /// </summary>
        [Field(Field = "ref_class_id", Indexed = false)]
        public int RefClassID { get; set; }

        /// <summary>
        /// 違規之作號(二選一)
        /// </summary>
        [Field(Field = "seat_no", Indexed = false)]
        public string SeatNo { get; set; }

        /// <summary>
        /// 違規之座標(二選一)
        /// </summary>
        [Field(Field = "coordinate", Indexed = false)]
        public string Coordinate { get; set; }

        /// <summary>
        /// 補充說明
        /// </summary>
        [Field(Field = "remark", Indexed = false)]
        public string Remark { get; set; }

        /// <summary>
        /// 照片1
        /// </summary>
        [Field(Field = "picture1", Indexed = false)]
        public string Picture1 { get; set; }

        /// <summary>
        /// 照片1的說明
        /// </summary>
        [Field(Field = "pic1_comment", Indexed = false)]
        public string Pic1Comment { get; set; }

        /// <summary>
        /// 檔案大小(KB)
        /// </summary>
        [Field(Field = "pic1_size", Indexed = false)]
        public int Pic1Size { get; set; }

        /// <summary>
        /// 照片2
        /// </summary>
        [Field(Field = "picture2", Indexed = false)]
        public string Picture2 { get; set; }

        /// <summary>
        /// 照片1的說明
        /// </summary>
        [Field(Field = "pic2_comment", Indexed = false)]
        public string Pic2Comment { get; set; }

        /// <summary>
        /// 檔案大小(KB)
        /// </summary>
        [Field(Field = "pic2_size", Indexed = false)]
        public int Pic2Size { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Field(Field = "create_time", Indexed = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 評分員最後修改日期
        /// </summary>
        [Field(Field = "scorer_last_update", Indexed = false)]
        public DateTime ScoreLastUpdate { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        [Field(Field = "is_canceled", Indexed = false)]
        public bool IsCancel { get; set; }

        /// <summary>
        /// 取消時間
        /// </summary>
        [Field(Field = "canceled_time", Indexed = false)]
        public DateTime CanceledTime { get; set; }

        /// <summary>
        /// 取消者姓名
        /// </summary>
        [Field(Field = "canceled_name", Indexed = false)]
        public string CanceledName { get; set; }

        /// <summary>
        /// 取消者教師帳號
        /// </summary>
        [Field(Field = "canceled_by", Indexed = false)]
        public string CanceledBy { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Field(Field = "cancel_reason", Indexed = false)]
        public string CancelReason { get; set; }

        /// <summary>
        /// 評分加減分
        /// </summary>
        [Field(Field = "score", Indexed = false)]
        public int Score { get; set; }

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
    }
}
