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
using System.Net;

namespace Ischool.discipline_competition
{
    public class Program
    {
        /// <summary>
        /// 秩序競賽模組專用角色名稱
        /// </summary>
        public static string _roleName = "秩序競賽管理員";

        /// <summary>
        /// 整潔競賽模組專用角色功能權限
        /// </summary>
        public static string _permission = @"
<Permissions>
    <Feature Code=""B8AF8DCD-4B1F-401B-86DC-9CC6CC349EEC"" Permission=""Execute""/>
	<Feature Code=""2D8CBAC3-25B8-4B29-BA99-D73E81F026D6"" Permission=""Execute""/>
	<Feature Code=""F7443940-A4F3-4E5D-AD43-3008ED4CF6A8"" Permission=""Execute""/>
	<Feature Code=""023A57E4-C30D-4732-A494-30CEBC0EE468"" Permission=""Execute""/>
	<Feature Code=""D8326986-7570-4E33-9B56-2B164CEFF0CD"" Permission=""Execute""/>
	<Feature Code=""C35D6BF0-8DFD-4180-B08E-8F49CE03658C"" Permission=""Execute""/>
	<Feature Code=""5BAF9935-713B-44FB-82C9-2357C69C3CB7"" Permission=""Execute""/>
	<Feature Code=""17414565-15CC-431B-B6F4-1621DAD7F2C2"" Permission=""Execute""/>
	<Feature Code=""9D39A466-D169-426E-8B5F-CB7B2E4C5111"" Permission=""Execute""/>
</Permissions>
";

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

            if (true) //!checkUDT
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

            MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["人員設定"].Image = Properties.Resources.foreign_language_config_64;
            MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["人員設定"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["評分設定"].Image = Properties.Resources.presentation_a_config_64;
            MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["評分設定"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"].Image = Properties.Resources.Report;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理評分紀錄"].Image = Properties.Resources.blacklist_zoom_128;
            MotherForm.RibbonBarItems["秩序競賽", "評分管理 / 統計報表"]["管理評分紀錄"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"].Image = Properties.Resources.calc_fav_64;
            MotherForm.RibbonBarItems["秩序競賽", "排名作業"]["計算排名"].Size = RibbonBarButton.MenuButtonSize.Large;

            #region 設定管理員
            {
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["人員設定"]["設定管理員"].Enable = Permissions.設定管理員權限;
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["人員設定"]["設定管理員"].Click += delegate
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

            #region 設定評分員
            {
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["人員設定"]["設定評分員"].Enable = Permissions.設定評分員權限;
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["人員設定"]["設定評分員"].Click += delegate 
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

            #region 設定評分時段
            {
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["評分設定"]["設定評分時段"].Enable = Permissions.設定評分時段權限;
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["評分設定"]["設定評分時段"].Click += delegate
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

            #region 設定評分項目
            {
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["評分設定"]["設定評分項目"].Enable = Permissions.設定評分項目權限;
                MotherForm.RibbonBarItems["秩序競賽", "基本設定"]["評分設定"]["設定評分項目"].Click += delegate
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
            detail.Add(new RibbonFeature(Permissions.設定管理員, "設定管理員"));
            detail.Add(new RibbonFeature(Permissions.設定評分員, "設定評分員"));
            detail.Add(new RibbonFeature(Permissions.設定評分時段, "設定評分時段"));
            detail.Add(new RibbonFeature(Permissions.設定評分項目, "設定評分項目"));
            detail.Add(new RibbonFeature(Permissions.管理評分紀錄, "管理評分紀錄"));
            detail.Add(new RibbonFeature(Permissions.計算週排名, "計算週排名"));
            detail.Add(new RibbonFeature(Permissions.計算學期排名, "計算學期排名"));
            detail.Add(new RibbonFeature(Permissions.週排名報表, "週排名報表"));
            detail.Add(new RibbonFeature(Permissions.學期排名報表, "學期排名報表"));
            #endregion

            #endregion
        }

        /// <summary>
        /// 檢查秩序競賽模組專用角色是否存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 建立秩序競賽模組專用角色
        /// </summary>
        static private void CreateMoudulAdmin()
        {
            string sql = "";

            // 如果不存在建立角色
            if (!CheckRoleISExist(_roleName)) // 檢查_role 是否已經存在秩序競賽管理員腳色
            {
                string description = "";
                sql = string.Format(@"
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
                    ", _roleName, description, _permission);
            }
            else // 更新角色權限
            {
                sql = string.Format(@"
UPDATE _role SET
    permission = '{0}'
WHERE
    role_name = '{1}'
                ",_permission,_roleName);
            }

            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);
        }
    }
}