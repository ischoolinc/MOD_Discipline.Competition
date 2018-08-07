using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.UDT;
using K12.Data.Configuration;
using FISCA.Presentation;
using FISCA.Permission;
using FISCA.Data;
using System.Data;
using K12.Data;
using FISCA.Presentation.Controls;

namespace Ischool.discipline_competition
{
    public class Program
    {
        [MainMethod("秩序競賽模組")]
        static public void Main()
        {
            #region Init UDT
            ConfigData cd = K12.Data.School.Configuration["秩序比賽模組載入設定"];

            bool checkUDT = false;
            string name = "秩序比賽UDT是否已載入";

            //如果尚無設定值,預設為
            if (string.IsNullOrEmpty(cd[name]))
            {
                cd[name] = "false";
            }

            //檢查是否為布林
            bool.TryParse(cd[name], out checkUDT);

            if (!checkUDT)
            {
                AccessHelper access = new AccessHelper();
                access.Select<UDT.Admin>("UID = '00000'");
                access.Select<UDT.CheckItem>("UID = '00000'");
                access.Select<UDT.Period>("UID = '00000'");
                access.Select<UDT.Scorer>("UID = '00000'");
                access.Select<UDT.ScoreSheet>("UID = '00000'");
                access.Select<UDT.SemesterStats>("UID = '00000'");
                access.Select<UDT.SemesterRank>("UID = '00000'");
                access.Select<UDT.SnapshotScoreSheet>("UID = '00000'");
                access.Select<UDT.WeeklyRank>("UID = '00000'");
                access.Select<UDT.WeeklyStats>("UID = '00000'");

                cd[name] = "true";
                cd.Save();
            }
            #endregion

            #region Init Admin Role

            CreateMoudulAdmin();

            #endregion

            bool isAdmin = DAO.Actor.Instance.CheckAdmin();

            #region 秩序競賽
            // 秩序競賽模組頁面
            DisciplineCompetitionPanel panel = new DisciplineCompetitionPanel();
            MotherForm.AddPanel(DisciplineCompetitionPanel.Instance);

            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"].Image = Properties.Resources.network_lock_64;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"].Image = Properties.Resources.Report;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理評分紀錄"].Image = Properties.Resources.blacklist_write_128;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理評分紀錄"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"].Image = Properties.Resources.calc_fav_64;
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"].Size = RibbonBarButton.MenuButtonSize.Large;

            #region 管理管理員
            {
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理管理員"].Enable = Permissions.管理管理員權限;
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理管理員"].Click += delegate
                {
                    if (isAdmin)
                    {
                        (new frmSetAdmin()).ShowDialog();
                    }
                    else
                    {
                        MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                    }
                };
            }
            #endregion

            #region 管理評分員
            {
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理評分員"].Enable = Permissions.管理評分員權限;
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理評分員"].Click += delegate 
                {
                    if (isAdmin)
                    {
                        (new frmSetScorer()).ShowDialog();
                    }
                    else
                    {
                        MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                    }
                };
            }
            #endregion

            #region 管理評分時段
            {
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理評分時段"].Enable = Permissions.管理評分時段權限;
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理評分時段"].Click += delegate
                {
                    if (isAdmin)
                    {
                        (new frmSetScorePeriod()).ShowDialog();
                    }
                    else
                    {
                        MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                    }
                };
            }
            #endregion

            #region 管理評分項目
            {
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理評分項目"].Enable = Permissions.管理評分項目權限;
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理"]["管理評分項目"].Click += delegate
                {
                    if (isAdmin)
                    {
                        (new frmSetCheckItem()).ShowDialog();
                    }
                    else
                    {
                        MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                    }
                };
            }
            #endregion

            #region 管理評分紀錄
            {
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理評分紀錄"].Enable = Permissions.管理評分紀錄權限;
                MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理評分紀錄"].Click += delegate
                {
                    if (isAdmin)
                    {
                        (new frmScoreSheet()).ShowDialog();
                    }
                    else
                    {
                        MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                    }
                };
            }
            #endregion

            #region 計算週排名
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"]["計算週排名"].Enable = Permissions.計算週排名權限;
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"]["計算週排名"].Click += delegate
            {
                if (isAdmin)
                {
                    (new frmWeeklyScore()).ShowDialog();
                }
                else
                {
                    MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                }
            };
            #endregion

            #region 計算學期排名
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"]["計算學期排名"].Enable = Permissions.計算學期排名權限;
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"]["計算學期排名"].Click += delegate
            {
                if (isAdmin)
                {
                    (new frmSemesterScore()).ShowDialog();
                }
                else
                {
                    MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                }
            };
            #endregion

            #region 週排名報表
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"]["週排名報表"].Enable = Permissions.週排名報表權限;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"]["週排名報表"].Click += delegate
            {
                if (isAdmin)
                {
                    (new frmWeeklyRankReport()).ShowDialog();
                }
                else
                {
                    MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                }
            };
            #endregion

            #region 學期排名報表
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"]["學期排名報表"].Enable = Permissions.學期排名報表權限;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"]["學期排名報表"].Click += delegate
            {
                if (isAdmin)
                {
                    (new frmSemesterRankReport()).ShowDialog();
                }
                else
                {
                    MsgBox.Show("此帳號沒有秩序競賽管理權限!");
                }
            };
            #endregion

            #region 權限管理

            Catalog detail = new Catalog();
            detail = RoleAclSource.Instance["秩序競賽"]["功能按鈕"];
            detail.Add(new RibbonFeature(Permissions.管理管理員, "管理管理員"));
            detail.Add(new RibbonFeature(Permissions.管理評分員, "管理評分員"));
            detail.Add(new RibbonFeature(Permissions.管理評分時段, "管理評分時段"));
            detail.Add(new RibbonFeature(Permissions.管理評分項目, "管理評分項目"));
            detail.Add(new RibbonFeature(Permissions.管理評分紀錄, "管理評分紀錄"));
            detail.Add(new RibbonFeature(Permissions.計算週排名, "計算週排名"));
            detail.Add(new RibbonFeature(Permissions.計算學期排名, "計算學期排名"));
            detail.Add(new RibbonFeature(Permissions.週排名報表, "週排名報表"));
            detail.Add(new RibbonFeature(Permissions.學期排名報表, "學期排名報表"));
            #endregion

            #endregion
        }

        static private bool CheckRoleISExist(string roleName)
        {
            string sql = string.Format("SELECT * FROM _role WHERE role_name = '{0}'",roleName);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            if (dt.Rows.Count > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static private void CreateMoudulAdmin()
        {
            string roleName = "秩序競賽管理員";

            // 檢查_role 是否已經存在秩序競賽管理員腳色
            // 如果不存在建立角色
            if (!CheckRoleISExist(roleName))
            {
                #region permission
                string permission = @"
<Permissions>
	<Feature Code=""Button0010"" Permission=""None""/>

	<Feature Code=""Button0020"" Permission=""None""/>
	<Feature Code=""Button0030"" Permission=""None""/>
	<Feature Code=""Button0085"" Permission=""None""/>
	<Feature Code=""Button0040"" Permission=""None""/>
	<Feature Code=""Button0050"" Permission=""None""/>
	<Feature Code=""Button0090"" Permission=""None""/>
	<Feature Code=""Button0092"" Permission=""None""/>
	<Feature Code=""Button0095"" Permission=""None""/>
	<Feature Code=""Button0110"" Permission=""None""/>
	<Feature Code=""Button0113"" Permission=""None""/>
	<Feature Code=""Button0116"" Permission=""None""/>
	<Feature Code=""Button0120"" Permission=""None""/>
	<Feature Code=""Button0130"" Permission=""None""/>
	<Feature Code=""Button0140"" Permission=""None""/>
	<Feature Code=""Button0150"" Permission=""None""/>
	<Feature Code=""Button0160"" Permission=""None""/>
	<Feature Code=""Button0170"" Permission=""None""/>
	<Feature Code=""Button0175"" Permission=""None""/>
	<Feature Code=""Button0210"" Permission=""None""/>
	<Feature Code=""Button0220"" Permission=""None""/>
	<Feature Code=""Button0230"" Permission=""None""/>
	<Feature Code=""Button0240"" Permission=""None""/>
	<Feature Code=""Button0250"" Permission=""None""/>
	<Feature Code=""Button0300"" Permission=""None""/>
	<Feature Code=""Button0310"" Permission=""None""/>
	<Feature Code=""SHSchool.Student.Ribbon0169"" Permission=""None""/>
	<Feature Code=""SHSchool.Student.Ribbon0170"" Permission=""None""/>
	<Feature Code=""SHSchool.Student.Ribbon0171"" Permission=""None""/>
	<Feature Code=""高中系統/匯出評量成績"" Permission=""None""/>
	<Feature Code=""高中系統/匯入評量成績"" Permission=""None""/>
	<Feature Code=""K12.Student.MeritEditForm"" Permission=""None""/>
	<Feature Code=""K12.Student.DemeritEditForm"" Permission=""None""/>
	<Feature Code=""Button0190"" Permission=""None""/>
	<Feature Code=""Button0180"" Permission=""None""/>
	<Feature Code=""Button0270"" Permission=""None""/>
	<Feature Code=""Button0260"" Permission=""None""/>
	<Feature Code=""K12.Behavior.BatchClearDemerit.010"" Permission=""None""/>
	<Feature Code=""K12.Student.AttendanceForm"" Permission=""None""/>
	<Feature Code=""K12.Student.TestSingleEditor5"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.Export.MoralScore"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.Import.MoralScore"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.Export.Comment"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.Import.Comment"" Permission=""None""/>
	<Feature Code=""SmartSchool.Evaluation.Reports.Retake.RetakeWithCourseList"" Permission=""None""/>
	<Feature Code=""K12DeleteStudentData"" Permission=""None""/>
	<Feature Code=""K12DeleteStudentPhoto"" Permission=""None""/>
	<Feature Code=""K12.Student.SpeedAddToTemp.0412"" Permission=""None""/>
	<Feature Code=""K12BatchStudentSemesterHistory"" Permission=""None""/>
	<Feature Code=""SH_SemesterScoreReport.6D7169EF-5BAD-41C5-B999-0946F37A6F92"" Permission=""None""/>
	<Feature Code=""定期評量成績單.D04E7F02-89C1-4412-81FA-8D87B96BF847"" Permission=""None""/>
	<Feature Code=""Content0010"" Permission=""None""/>
	<Feature Code=""Content0020"" Permission=""None""/>
	<Feature Code=""Content0030"" Permission=""None""/>
	<Feature Code=""Content0040"" Permission=""None""/>
	<Feature Code=""Content0050"" Permission=""None""/>
	<Feature Code=""Content0090"" Permission=""None""/>
	<Feature Code=""Content0100"" Permission=""None""/>
	<Feature Code=""Content0110"" Permission=""None""/>
	<Feature Code=""Content0120"" Permission=""None""/>
	<Feature Code=""Content0130"" Permission=""None""/>
	<Feature Code=""Content0135"" Permission=""None""/>
	<Feature Code=""Content0145"" Permission=""None""/>
	<Feature Code=""Content0165.5"" Permission=""None""/>
	<Feature Code=""Content0155"" Permission=""None""/>
	<Feature Code=""K12.Student.MeritItem"" Permission=""None""/>
	<Feature Code=""K12.Student.DemeritItem"" Permission=""None""/>
	<Feature Code=""K12.Student.AttendanceItem"" Permission=""None""/>
	<Feature Code=""Report0040"" Permission=""None""/>
	<Feature Code=""Report0045"" Permission=""None""/>
	<Feature Code=""Report0050"" Permission=""None""/>
	<Feature Code=""Report0055"" Permission=""None""/>
	<Feature Code=""Report0060"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.SelectMeritForm"" Permission=""None""/>
	<Feature Code=""Report0030"" Permission=""None""/>
	<Feature Code=""SHEvaluation.Report1000011"" Permission=""None""/>
	<Feature Code=""K12.Behavior.Student.AbsenceNotificationSelectDateRangeForm"" Permission=""None""/>
	<Feature Code=""JHSchool.Student.Report0030"" Permission=""None""/>
	<Feature Code=""Behavior.Student.ClearDemeritReport"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.DisciplineNotificationForm"" Permission=""None""/>
	<Feature Code=""K12.Student.SelectAttendanceForm"" Permission=""None""/>
	<Feature Code=""K12.Student.SelectMeritDemeritForm"" Permission=""None""/>
	<Feature Code=""SHEvaluation.SchoolYearScoreReportNew.Student"" Permission=""None""/>
	<Feature Code=""SHSchool.DailyManifestation"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Student.KeptInSchoolAnAdviceNote"" Permission=""None""/>
	<Feature Code=""Student.Field.身分證號"" Permission=""None""/>
	<Feature Code=""Student.Field.戶籍電話"" Permission=""None""/>
	<Feature Code=""Student.Field.聯絡電話"" Permission=""None""/>
	<Feature Code=""Button0330"" Permission=""None""/>
	<Feature Code=""Button0340"" Permission=""None""/>
	<Feature Code=""Button0350"" Permission=""None""/>
	<Feature Code=""Button0360"" Permission=""None""/>
	<Feature Code=""Button0365"" Permission=""None""/>
	<Feature Code=""Button0370"" Permission=""None""/>
	<Feature Code=""Button0375"" Permission=""None""/>
	<Feature Code=""Button0380"" Permission=""None""/>
	<Feature Code=""Button0390"" Permission=""None""/>
	<Feature Code=""Button0400"" Permission=""None""/>
	<Feature Code=""Button0410"" Permission=""None""/>
	<Feature Code=""Button0420"" Permission=""None""/>
	<Feature Code=""Button0430"" Permission=""None""/>
	<Feature Code=""SH.Class.ClassSemesterScoreReportFixed_SH"" Permission=""None""/>
	<Feature Code=""JHBehavior.Class.Ribbon0210"" Permission=""None""/>
	<Feature Code=""Content0150"" Permission=""None""/>
	<Feature Code=""Content0160"" Permission=""None""/>
	<Feature Code=""Report0070"" Permission=""None""/>
	<Feature Code=""Report0080"" Permission=""None""/>
	<Feature Code=""Report0090"" Permission=""None""/>
	<Feature Code=""Report0100"" Permission=""None""/>
	<Feature Code=""Report0110"" Permission=""None""/>
	<Feature Code=""Report0120"" Permission=""None""/>
	<Feature Code=""Report0130"" Permission=""None""/>
	<Feature Code=""Report0140"" Permission=""None""/>
	<Feature Code=""Report0145"" Permission=""None""/>
	<Feature Code=""Report0150"" Permission=""None""/>
	<Feature Code=""Report0155"" Permission=""None""/>
	<Feature Code=""Report0160"" Permission=""None""/>
	<Feature Code=""Report0170"" Permission=""None""/>
	<Feature Code=""Report0180"" Permission=""None""/>
	<Feature Code=""Report0220"" Permission=""None""/>
	<Feature Code=""Report0270"" Permission=""None""/>
	<Feature Code=""Report0280"" Permission=""None""/>
	<Feature Code=""Report0190"" Permission=""None""/>
	<Feature Code=""SHEvaluation.Report1000010"" Permission=""None""/>
	<Feature Code=""K12.Behavior.Class.AbsenceNotificationSelectDateRangeForm"" Permission=""None""/>
	<Feature Code=""JHSchool.Class.Report0040"" Permission=""None""/>
	<Feature Code=""Report0230"" Permission=""None""/>
	<Feature Code=""Behavior.Class.ClearDemeritReport"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Class.DisciplineNotificationForm"" Permission=""None""/>
	<Feature Code=""Report0260"" Permission=""None""/>
	<Feature Code=""Report0250"" Permission=""None""/>
	<Feature Code=""Report0240"" Permission=""None""/>
	<Feature Code=""SHEvaluation.SchoolYearScoreReportNew.Class"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.Class.KeptInSchoolAnAdviceNote"" Permission=""None""/>
	<Feature Code=""SHSchool.SemesterMoralScoreCalcForm"" Permission=""None""/>
	<Feature Code=""Button0450"" Permission=""None""/>
	<Feature Code=""Button0460"" Permission=""None""/>
	<Feature Code=""Button0470"" Permission=""None""/>
	<Feature Code=""Button0480"" Permission=""None""/>
	<Feature Code=""Button0490"" Permission=""None""/>
	<Feature Code=""Button0500"" Permission=""None""/>
	<Feature Code=""K12DeleteTeacher"" Permission=""None""/>
	<Feature Code=""Content0170"" Permission=""None""/>
	<Feature Code=""Content0180"" Permission=""None""/>
	<Feature Code=""Content0190"" Permission=""None""/>
	<Feature Code=""Teacher.Field.身分證號"" Permission=""None""/>
	<Feature Code=""Teacher.Field.聯絡電話"" Permission=""None""/>
	<Feature Code=""Button0520"" Permission=""None""/>
	<Feature Code=""Button0530"" Permission=""None""/>
	<Feature Code=""Button0540"" Permission=""None""/>
	<Feature Code=""Button0550"" Permission=""None""/>
	<Feature Code=""Button0560"" Permission=""None""/>
	<Feature Code=""Button0570"" Permission=""None""/>
	<Feature Code=""Button0580"" Permission=""None""/>
	<Feature Code=""Button0590"" Permission=""None""/>
	<Feature Code=""Button0600"" Permission=""None""/>
	<Feature Code=""Button0610"" Permission=""None""/>
	<Feature Code=""Sunset.Ribbon0130"" Permission=""None""/>
	<Feature Code=""Content0200"" Permission=""None""/>
	<Feature Code=""Content0210"" Permission=""None""/>
	<Feature Code=""Report0290"" Permission=""None""/>
	<Feature Code=""Button0660"" Permission=""None""/>
	<Feature Code=""Button0670"" Permission=""None""/>
	<Feature Code=""Button0680"" Permission=""None""/>
	<Feature Code=""Button0690"" Permission=""None""/>
	<Feature Code=""Button0790"" Permission=""None""/>
	<Feature Code=""Button0795"" Permission=""None""/>
	<Feature Code=""Button0800"" Permission=""None""/>
	<Feature Code=""Button0810"" Permission=""None""/>
	<Feature Code=""Button0720"" Permission=""None""/>
	<Feature Code=""Button0820"" Permission=""None""/>
	<Feature Code=""Button08201"" Permission=""None""/>
	<Feature Code=""Button0830"" Permission=""None""/>
	<Feature Code=""Button0840"" Permission=""None""/>
	<Feature Code=""Button0850"" Permission=""None""/>
	<Feature Code=""Button0860"" Permission=""None""/>
	<Feature Code=""Button0870"" Permission=""None""/>
	<Feature Code=""SHSchool.EduAdmin.Ribbon0070"" Permission=""None""/>
	<Feature Code=""K12.EduAdminDataMapping.NationalityMappingForm"" Permission=""None""/>
	<Feature Code=""K12SwapAttendStudent"" Permission=""None""/>
	<Feature Code=""SHStudentCourseScoreCheck100_SH"" Permission=""None""/>
	<Feature Code=""StudentDuplicateSubjectCheck"" Permission=""None""/>
	<Feature Code=""SHSchool.SHCourseScoreInputStatus"" Permission=""None""/>
	<Feature Code=""Button0705"" Permission=""None""/>
	<Feature Code=""Button0760"" Permission=""None""/>
	<Feature Code=""Button0765"" Permission=""None""/>
	<Feature Code=""Button0780"" Permission=""None""/>
	<Feature Code=""K12.Student.AbsenceConfigForm"" Permission=""None""/>
	<Feature Code=""K12.Student.PeriodConfigForm"" Permission=""None""/>
	<Feature Code=""K12.Student.ReduceForm"" Permission=""None""/>
	<Feature Code=""K12.Student.DisciplineForm"" Permission=""None""/>
	<Feature Code=""K12.Student.AttendanceEditForm"" Permission=""None""/>
	<Feature Code=""JHSchool.StuAdmin.Ribbon0060"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.StuAdmin.DisciplineStatistics"" Permission=""None""/>
	<Feature Code=""SHSchool.Behavior.StuAdmin.TeacherDiffOpenConfig"" Permission=""None""/>
	<Feature Code=""K12.缺曠統計表"" Permission=""None""/>
	<Feature Code=""K12.Student.StudentDemeritClear"" Permission=""None""/>
	<Feature Code=""K12.獎懲統計表"" Permission=""None""/>
	<Feature Code=""SelectFlagStudent.DDA9F584-1303-4A04-851F-CBC093F24ADA"" Permission=""None""/>
	<Feature Code=""System0030"" Permission=""None""/>
	<Feature Code=""System0040"" Permission=""None""/>
	<Feature Code=""System0050"" Permission=""None""/>
	<Feature Code=""System0060"" Permission=""None""/>
	<Feature Code=""ischool.System.UDQ.CreateQuery"" Permission=""None""/>
	<Feature Code=""ischool.System.DataRationality"" Permission=""None""/>
	<Feature Code=""2D8CBAC3-25B8-4B29-BA99-D73E81F026D6"" Permission=""Execute""/>
	<Feature Code=""F7443940-A4F3-4E5D-AD43-3008ED4CF6A8"" Permission=""Execute""/>
	<Feature Code=""023A57E4-C30D-4732-A494-30CEBC0EE468"" Permission=""Execute""/>
	<Feature Code=""D8326986-7570-4E33-9B56-2B164CEFF0CD"" Permission=""Execute""/>
	<Feature Code=""C35D6BF0-8DFD-4180-B08E-8F49CE03658C"" Permission=""Execute""/>
	<Feature Code=""5BAF9935-713B-44FB-82C9-2357C69C3CB7"" Permission=""Execute""/>
	<Feature Code=""17414565-15CC-431B-B6F4-1621DAD7F2C2"" Permission=""Execute""/>
	<Feature Code=""9D39A466-D169-426E-8B5F-CB7B2E4C5111"" Permission=""Execute""/>
	<Feature Code=""ischool.DiagnosticsMode"" Permission=""None""/>
	<Feature Code=""ischool.AdvancedToolSet"" Permission=""None""/>
</Permissions>
";
                #endregion

                string description = "";
                string sql = string.Format(@"
    INSERT INTO _role(
        role_name 
        , description
        , permission
    ) 
    VALUES (
        '{0}'
        ,'{1}'
        ,'{2}' 
    )
                    ", roleName, description, permission);

                UpdateHelper up = new UpdateHelper();
                up.Execute(sql);
            }
        }
    }
}