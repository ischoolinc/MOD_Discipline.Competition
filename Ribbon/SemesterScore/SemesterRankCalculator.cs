using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.discipline_competition
{
    class SemesterRankCalculator
    {
        private string _schoolYear;
        private string _semester;
        private Dictionary<int, List<UDT.SemesterStats>> _dicSemesterStatsByGradeYear;
        private string _userAccount = DAO.Actor.Instance.GetUserAccount();
        private List<UDT.SemesterRank> listInsertSemesterRank;

        public SemesterRankCalculator(string schoolYear,string semester)
        {
            this._schoolYear = schoolYear;
            this._semester = semester;
            this._dicSemesterStatsByGradeYear = new Dictionary<int, List<UDT.SemesterStats>>();
            this.listInsertSemesterRank = new List<UDT.SemesterRank>();
        }

        public void Execute()
        {
            // 1.取得學期統計資料
            this.getSemesterStats();

            // 2.按照年級1.2.3依序計算年級學期排名
            this.calculateRank();
        }

        // 取得學期統計資料
        private void getSemesterStats()
        {
            AccessHelper access = new AccessHelper();
            List<UDT.SemesterStats>listSemesterStats = access.Select<UDT.SemesterStats>(string.Format("school_year = {0} AND semester = {1}",this._schoolYear,this._semester));

            // 資料整理: 依照年級做分類
            foreach (UDT.SemesterStats semesterStats in listSemesterStats)
            {
                if (!this._dicSemesterStatsByGradeYear.ContainsKey(semesterStats.GradeYear))
                {
                    this._dicSemesterStatsByGradeYear.Add(semesterStats.GradeYear,new List<UDT.SemesterStats>());
                }
                this._dicSemesterStatsByGradeYear[semesterStats.GradeYear].Add(semesterStats);
            }
        }

        private void calculateRank()
        {
            // 1.刪除學年度學期 學期排名資料
            AccessHelper access = new AccessHelper();
            List<UDT.SemesterRank> listSemesterRank = access.Select<UDT.SemesterRank>(string.Format("school_year ={0} AND semester = {1}",this._schoolYear,this._semester));
            access.DeletedValues(listSemesterRank);

            // 2.按年級排名
            for (int gradeYear = 1;gradeYear <= 3 ;gradeYear++)
            {
                List<UDT.SemesterStats> listSemesterStats = _dicSemesterStatsByGradeYear[gradeYear];
                
                //建立分數名次清單
                // 計算規則: 名次加總*100 - 週總分平均
                List<int> listScore = new List<int>(); 

                foreach (UDT.SemesterStats semesterStats in _dicSemesterStatsByGradeYear[gradeYear])
                {
                    int score = semesterStats.RankTotal * 100 - semesterStats.AverageScore;
                    listScore.Add(score);
                }
                listScore = listScore.OrderBy(e => e).ToList(); // 遞增排: 最小的為第一名

                foreach (UDT.SemesterStats ss in listSemesterStats)
                {
                    int rank = listScore.IndexOf(ss.RankTotal*100 - ss.AverageScore) + 1;

                    // 建立排名物件
                    UDT.SemesterRank sr = new UDT.SemesterRank();
                    sr.RefSemesterStatsID = int.Parse(ss.UID);
                    sr.SchoolYear = ss.SchoolYear;
                    sr.Semester = ss.Semester;
                    sr.RefClassID = ss.RefClassID;
                    sr.GradeYear = ss.GradeYear;
                    sr.CreateTime = DateTime.Now;
                    sr.RankTotal = ss.RankTotal;
                    sr.AverageScore = ss.AverageScore;
                    sr.Rank = rank;
                    sr.CreatedBy = this._userAccount;

                    this.listInsertSemesterRank.Add(sr);
                }
            }

            // 資料庫更新
            access.InsertValues(listInsertSemesterRank);
        }
    }
}
