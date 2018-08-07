using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition
{
    class SemesterStatsCalculator
    {
        private string _schoolYear;
        private string _semester;
        private string _userAccount = DAO.Actor.Instance.GetUserAccount();

        private Dictionary<string, List<UDT.WeeklyRank>> _dicWeeklyRankByClassID;
        private List<UDT.SemesterStats> listInsertSemesterStats;

        public SemesterStatsCalculator(string schoolYear,string semester)
        {
            this._schoolYear = schoolYear;
            this._semester = semester;
            this._dicWeeklyRankByClassID = new Dictionary<string, List<UDT.WeeklyRank>>();
            this.listInsertSemesterStats = new List<UDT.SemesterStats>();
        }

        public void Execute()
        {
            // 1.取得學期週排名紀錄
            this.getRawRecords();

            // 2.統計班級秩序競賽週排名名次、總分平均
            this.calculateScore();
        }

        /// <summary>
        /// 取得學期週排名資料，併按照班級整理
        /// </summary>
        private void getRawRecords()
        {
            AccessHelper access = new AccessHelper();
            // 1.取得學年度學期 週排名資料
            List<UDT.WeeklyRank> listWeeklyRank = access.Select<UDT.WeeklyRank>(string.Format("school_year = {0} AND semester = {1}", this._schoolYear, this._semester));

            foreach (UDT.WeeklyRank weeklyRank in listWeeklyRank)
            {
                string classID = "" + weeklyRank.RefClassID;

                if (!_dicWeeklyRankByClassID.ContainsKey(classID))
                {
                    _dicWeeklyRankByClassID.Add(classID, new List<UDT.WeeklyRank>());
                }
                _dicWeeklyRankByClassID[classID].Add(weeklyRank);
            }
        }

        private void calculateScore()
        {
            // 1.刪除學年度學期的學期統計資料
            AccessHelper access = new AccessHelper();
            List<UDT.SemesterStats>listSemesterStats = access.Select<UDT.SemesterStats>(string.Format("school_year = {0} AND semester = {1}",_schoolYear,_semester));
            access.DeletedValues(listSemesterStats);

            // 2.針對每個班級
            foreach (string classID in this._dicWeeklyRankByClassID.Keys)
            {
                int rankTotal = 0;
                int totalScore = 0;
                int averageScore = 0;
                int weekNo = 0;
                int gradeYear = 0;
                // 2.1 找出該班級所有週排名資料
                foreach (UDT.WeeklyRank weeklyRank in this._dicWeeklyRankByClassID[classID])
                {
                    // 2.2 計算學期週排名次加總
                    rankTotal += weeklyRank.Rank;
                    // 2.3 計算學期週排總分平均
                    totalScore += weeklyRank.WeekTotal;

                    gradeYear = weeklyRank.GradeYear;
                    weekNo++;
                }
                averageScore = totalScore / weekNo;
                UDT.SemesterStats stats = new UDT.SemesterStats();
                stats.SchoolYear = int.Parse(_schoolYear);
                stats.Semester = int.Parse(_semester);
                stats.RefClassID = int.Parse(classID);
                stats.GradeYear = gradeYear;
                stats.RankTotal = rankTotal;
                stats.AverageScore = averageScore;
                stats.CreateTime = DateTime.Now;
                stats.CreatedBy = _userAccount;


                listInsertSemesterStats.Add(stats);
            }

            // 寫入資料庫
            access.InsertValues(listInsertSemesterStats);
        }
    }
}
