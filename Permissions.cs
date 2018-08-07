using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.discipline_competition
{
    class Permissions
    {
        public static string 管理管理員 { get { return "B8AF8DCD-4B1F-401B-86DC-9CC6CC349EEC"; } }
        public static bool 管理管理員權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[管理管理員].Executable;
            }
        }

        public static string 管理評分員 { get { return "2D8CBAC3-25B8-4B29-BA99-D73E81F026D6"; } }
        public static bool 管理評分員權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[管理評分員].Executable;
            }
        }

        public static string 管理評分時段 { get { return "F7443940-A4F3-4E5D-AD43-3008ED4CF6A8"; } }
        public static bool 管理評分時段權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[管理評分時段].Executable;
            }
        }

        public static string 管理評分項目 { get { return "023A57E4-C30D-4732-A494-30CEBC0EE468"; } }
        public static bool 管理評分項目權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[管理評分項目].Executable;
            }
        }

        public static string 管理評分紀錄 { get { return "D8326986-7570-4E33-9B56-2B164CEFF0CD"; } }
        public static bool 管理評分紀錄權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[管理評分紀錄].Executable;
            }
        }

        public static string 計算週排名 { get { return "C35D6BF0-8DFD-4180-B08E-8F49CE03658C"; } }
        public static bool 計算週排名權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[計算週排名].Executable;
            }
        }

        public static string 計算學期排名 { get { return "5BAF9935-713B-44FB-82C9-2357C69C3CB7"; } }
        public static bool 計算學期排名權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[計算學期排名].Executable;
            }
        }

        public static string 週排名報表 { get { return "17414565-15CC-431B-B6F4-1621DAD7F2C2"; } }
        public static bool 週排名報表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[週排名報表].Executable;
            }
        }

        public static string 學期排名報表 { get { return "9D39A466-D169-426E-8B5F-CB7B2E4C5111"; } }
        public static bool 學期排名報表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[學期排名報表].Executable;
            }
        }
    }
}
