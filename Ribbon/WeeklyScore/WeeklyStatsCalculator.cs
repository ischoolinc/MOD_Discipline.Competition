using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;
using K12.Data;
using System.Data;
using FISCA.Data;

namespace Ischool.discipline_competition
{
    /// <summary>
    /// 計算每班週統計成績的類別
    /// </summary>
    class WeeklyStatsCalculator
    {
        private string _schoolYear;
        private string _semester;
        private int _weekNo;
        private DateTime _startDate;
        private DateTime _endDate;
        //private List<ClassRecord> listClass;
        private Dictionary<string,DataRow> dicClassDataByID;

        private Dictionary<string, List<UDT.ScoreSheet>> dicRecordsByClassID;   //各班級該週的評分紀錄

        private List<UDT.WeeklyStats> _listUpdateWeeklyStats; // 週統計

        private decimal _baseScore = 80; // 基準分

        public WeeklyStatsCalculator(string schoolYear,string semester,int weekNumber,DateTime startDate,DateTime endDate)
        {
            this._schoolYear = schoolYear;
            this._semester = semester;
            this._weekNo = weekNumber;
            this._startDate = startDate;
            this._endDate = endDate;
            this.dicClassDataByID = new Dictionary<string, DataRow>();

            this.dicRecordsByClassID = new Dictionary<string, List<UDT.ScoreSheet>>();
            this._listUpdateWeeklyStats = new List<UDT.WeeklyStats>();

            //0. 取得全校班級(一、二、三年級)
            getClassData();
        }

        private void getClassData()
        {
            string sql = @"
SELECT
    *
FROM
    class
WHERE
    class.grade_year IN (1,2,3)
";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                string classID = "" + row["id"];
                if (!dicClassDataByID.ContainsKey(classID))
                {
                    dicClassDataByID.Add(classID,row);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            // 1.取得日期區間評分紀錄
            this.getRawRecords();

            // 2.統計班級秩序競賽評分
            this.calculateScore();
        }

        /// <summary>
        /// 取回所有班級的評分紀錄，並按照班級收集起來
        /// </summary>
        private void getRawRecords()
        {
            AccessHelper access = new AccessHelper();
            List<UDT.ScoreSheet> listScoreSheet = access.Select<UDT.ScoreSheet>(string.Format("create_time > '{0}' AND create_time <= '{1}'",this._startDate,this._endDate));
            foreach(UDT.ScoreSheet sheet in listScoreSheet)
            {
                string key = "" + sheet.RefClassID;
                if (!dicRecordsByClassID.ContainsKey(key))
                {
                    dicRecordsByClassID.Add(key, new List<UDT.ScoreSheet>());
                }
                dicRecordsByClassID[key].Add(sheet);
            }
        }

        private void calculateScore()
        {
            // 0. 刪除日期區間週統計
            AccessHelper access = new AccessHelper();
            List<UDT.WeeklyStats> listWeeklyStats = access.Select<UDT.WeeklyStats>(string.Format("school_year = {0} AND semester = {1} AND week_number = {2}", this._schoolYear, this._semester, this._weekNo));
            access.DeletedValues(listWeeklyStats);

            // 1. 針對每個班級
            foreach (string classID in dicClassDataByID.Keys)
            {
                decimal score = _baseScore;
                //  1.1 找出該班級的所有評分紀錄
                if (dicRecordsByClassID.ContainsKey(classID))
                {
                    //  1.2 計算總分
                    foreach (UDT.ScoreSheet sheet in dicRecordsByClassID[classID])
                    {
                        score += sheet.Score;
                    }
                }
                UDT.WeeklyStats weeklyStats = new UDT.WeeklyStats();
                weeklyStats.SchoolYear = int.Parse(this._schoolYear);
                weeklyStats.semester = int.Parse(this._semester);
                weeklyStats.RefClassID = int.Parse(classID);
                weeklyStats.GradeYear = int.Parse("" + dicClassDataByID[classID]["grade_year"]);
                weeklyStats.WeekTotal = (int)score;
                weeklyStats.WeekNumber = this._weekNo;
                weeklyStats.CreateTime = DateTime.Now;
                weeklyStats.CreatedBy = "";
                weeklyStats.StartDate = this._startDate;
                weeklyStats.EndDate = this._endDate;

                _listUpdateWeeklyStats.Add(weeklyStats);
            }
            //  1.3 更新資料庫
            access.InsertValues(_listUpdateWeeklyStats);

        }
    }
}
