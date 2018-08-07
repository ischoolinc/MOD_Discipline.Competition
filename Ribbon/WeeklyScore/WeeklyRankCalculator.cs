using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;
using FISCA.Data;
using K12.Data;

namespace Ischool.discipline_competition
{
    /// <summary>
    /// 計算每週各班排名的類別
    /// </summary>
    class WeeklyRankCalculator
    {
        private string _schoolYear;
        private string _semester;
        private int _weekNo;
        private DateTime _startDate;
        private DateTime _endDate;

        private Dictionary<string, UDT.WeeklyStats> _dicWeeklyStatsByClassID;

        private Dictionary<string, UDT.WeeklyRank> _dicLastWeeklyRankByClassID;

        private Dictionary<int,List<ClassRecord>> _dicAllClassByGrade;

        private List<UDT.WeeklyRank> _listUpdateWeeklyRank;

        public WeeklyRankCalculator(string schoolYear, string semester, int weekNumber, DateTime startDate, DateTime endDate)
        {
            _schoolYear = schoolYear;
            _semester = semester;
            _weekNo = weekNumber;
            _startDate = startDate;
            _endDate = endDate;

            this._dicWeeklyStatsByClassID = new Dictionary<string, UDT.WeeklyStats>();
            this._dicAllClassByGrade = new Dictionary<int, List<ClassRecord>>();
            this._listUpdateWeeklyRank = new List<UDT.WeeklyRank>();
            this._dicLastWeeklyRankByClassID = new Dictionary<string, UDT.WeeklyRank>();
            this.getAllClassData();

            // 1.取得目標週統計資料
            this.getWeeklyStatsData();
            
            this.getLastWeeklyRank();

        }

        // 取得目標週統計資料
        private void getWeeklyStatsData()
        {
            AccessHelper access = new AccessHelper();
            List<UDT.WeeklyStats> listWeeklyStats = access.Select<UDT.WeeklyStats>(string.Format("school_year = {0} AND semester = {1} AND week_number = {2} AND start_date = '{3}' AND end_date = '{4}'", this._schoolYear, this._semester,this._weekNo,this._startDate,this._endDate));

            foreach (UDT.WeeklyStats weeklyStats in listWeeklyStats)
            {
                if (!_dicWeeklyStatsByClassID.ContainsKey("" + weeklyStats.RefClassID))
                {
                    _dicWeeklyStatsByClassID.Add("" + weeklyStats.RefClassID, weeklyStats);
                }
            }
        }

        // 取得上週週排名資料
        private void getLastWeeklyRank()
        {
            int lastWeekNo = this._weekNo - 1;
            AccessHelper access = new AccessHelper();
            List<UDT.WeeklyRank> listLastWeeklyRank = access.Select<UDT.WeeklyRank>(string.Format("school_year = {0} AND semester = {1} AND week_number = {2} ", this._schoolYear, this._semester, lastWeekNo));

            foreach (UDT.WeeklyRank rank in listLastWeeklyRank)
            {
                if (!_dicLastWeeklyRankByClassID.ContainsKey("" + rank.RefClassID))
                {
                    _dicLastWeeklyRankByClassID.Add("" + rank.RefClassID, rank);
                }
            }
        }

        // 取得所有班級資料
        private void getAllClassData()
        {
            List<ClassRecord> listClass = Class.SelectAll();

            foreach (ClassRecord _class in listClass)
            {
                if (_class.GradeYear == 1 || _class.GradeYear == 2 || _class.GradeYear == 3)
                {
                    if (!this._dicAllClassByGrade.ContainsKey((int)_class.GradeYear))
                    {
                        _dicAllClassByGrade.Add((int)_class.GradeYear, new List<ClassRecord>());
                    }
                    _dicAllClassByGrade[(int)_class.GradeYear].Add(_class);
                }
            }
                
        }

        /// <summary>
        /// 按年級計算各班該年級排名
        /// </summary>
        public void Execute()
        {
            AccessHelper access = new AccessHelper();

            // 0.刪除學年度、學期、週次的週排行資料
            List<UDT.WeeklyRank> listRank = access.Select<UDT.WeeklyRank>(string.Format("school_year = {0} AND semester = {1} AND week_number = {2}",this._schoolYear,this._semester,this._weekNo));
            access.DeletedValues(listRank);

            // 1.對於每個年級
            foreach (int grade in _dicAllClassByGrade.Keys)
            {
                List<UDT.WeeklyStats> weeklyStats = new List<UDT.WeeklyStats>();

                // 建立分數名次清單
                List<int> listScoreRank = new List<int>();

                // 2.找出該年級所有班級
                foreach (ClassRecord _class in _dicAllClassByGrade[grade])
                {
                    // 3.找出班級週統計成績
                    weeklyStats.Add(this._dicWeeklyStatsByClassID[_class.ID]);
                    listScoreRank.Add(this._dicWeeklyStatsByClassID[_class.ID].WeekTotal);
                }

                // 4.計算排名
                // 4.0 排序分數名次清單
                //listScoreRank.OrderByDescending(score => score).ToList();
                //listScoreRank.Sort();
                listScoreRank = listScoreRank.OrderByDescending(x => x).ToList();

                //List<UDT.WeeklyStats> result =  weeklyStats.OrderByDescending(x => x.WeekTotal).ToList();
                //int n = 1;

                foreach (UDT.WeeklyStats ws in weeklyStats)
                {
                    // 找出分數所對應的排名
                    int rank = listScoreRank.IndexOf(ws.WeekTotal) + 1;
                    // 4.1 建立排名物件
                    UDT.WeeklyRank wr = this.createRankObject(ws, rank, grade);

                    // 5. 判斷是否前5週連2
                    checkIfTop2In5Weeks(wr);

                    // 6. 判斷是否連8週前三
                    checkIfTop3In8Weeks(wr);

                    _listUpdateWeeklyRank.Add(wr);
                    //n++;
                    
                }
            }

            // 5.寫入資料庫
            access.InsertValues(_listUpdateWeeklyRank);
        }

        /// <summary>
        /// 判斷是否連五周前2。
        /// 如果本州沒有前2，則次數歸零。
        /// 如果本週有前二：
        ///    a. 如果上週沒有資料，則上週次數視為 0
        ///    b. 如果上週的 needReset 欄位為 true，表示上周已經完成，所以上週次數視為 0
        ///    c. 否則：
        ///         1. 如果上週已經連四週二，則本週會連五週達成，所以次數為5，且 needReset 欄位為 true。
        ///         2.否則，就上週的次數 + 1
        /// </summary>
        /// <param name="wr"></param>
        private void checkIfTop2In5Weeks(UDT.WeeklyRank wr)
        {
            // 如果本週沒有前2
            if (wr.Rank > 2)
            {
                wr.Top2InARow = 0;
            }
            else
            {
                // 找出此班級上週的排名
                UDT.WeeklyRank wrLast = null;

                if (_dicLastWeeklyRankByClassID.ContainsKey("" + wr.RefClassID))
                {
                    wrLast = this._dicLastWeeklyRankByClassID["" + wr.RefClassID];
                }

                // 如果上週沒有資料，或上週的 needReset 欄位為 true，則上週次數視為 0
                if (wrLast == null || wrLast.NeedReset )
                {
                    wr.Top2InARow = 1;
                    return;
                }

                wr.Top2InARow = wrLast.Top2InARow + 1;

                // 如果上週已經連四週二，則本週會連五週達成，所以次數為5，且 needReset 欄位為 true。
                if (wrLast.Top2InARow >=4)
                {
                    wr.NeedReset = true; 
                }
            }
        }

        /// <summary>
        /// 判斷是否連續八周前3。
        /// 如果本週沒有前3，則次數歸零。
        /// 如果本週有前三：
        ///    a. 如果上週沒有資料，則上週次數視為 0
        ///    b. 如果上週的 needReset 欄位為 true，表示上周已經完成，所以上週次數視為 0
        ///    c. 否則：
        ///         1. 如果上週已經連七週三，則本週會連八週達成，所以次數為8，且 needReset 欄位為 true。
        ///         2.否則，就上週的次數 + 1
        /// </summary>
        /// <param name="wr"></param>
        private void checkIfTop3In8Weeks(UDT.WeeklyRank wr)
        {
            // 如果本週沒有前3
            if (wr.Rank > 3)
            {
                wr.Top3InARow = 0;
            }
            else
            {
                // 找出此班級上週的排名
                UDT.WeeklyRank wrLast = null;

                if (_dicLastWeeklyRankByClassID.ContainsKey("" + wr.RefClassID))
                {
                    wrLast = this._dicLastWeeklyRankByClassID["" + wr.RefClassID];
                }

                // 如果上週沒有資料，或上週的 needReset 欄位為 true，則上週次數視為 0
                if (wrLast == null || wrLast.NeedReset)
                {
                    wr.Top3InARow = 1;
                    return;
                }

                wr.Top3InARow = wrLast.Top3InARow + 1;

                // 如果上週已經連四週二，則本週會連五週達成，所以次數為5，且 needReset 欄位為 true。
                if (wrLast.Top3InARow >= 7)
                {
                    wr.NeedReset = true;
                }
            }
        }

        private UDT.WeeklyRank createRankObject(UDT.WeeklyStats ws, int rank, int grade)
        {
            UDT.WeeklyRank wr = new UDT.WeeklyRank();
            wr.RefWeeklyStatsID = int.Parse(ws.UID);
            wr.SchoolYear = int.Parse(this._schoolYear);
            wr.semester = int.Parse(this._semester);
            wr.RefClassID = ws.RefClassID;
            wr.GradeYear = grade;
            wr.WeekTotal = ws.WeekTotal;
            wr.WeekNumber = this._weekNo;
            wr.CreateTime = DateTime.Now;
            wr.CreatedBy = "";
            wr.Rank = rank;

            return wr;
        }

    }
}
