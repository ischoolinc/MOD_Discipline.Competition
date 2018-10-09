﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using System.Data;

namespace Ischool.discipline_competition.DAO
{
    class Actor
    {
        private string _userAccount;

        private string _userName;

        private string _roleAdminID;

        private bool _isAdmin;

        private static Actor _actor;

        public static Actor Instance
        {
            get
            {
                if (_actor == null)
                {
                    _actor = new Actor();
                }
                return _actor;
            }
        }

        private Actor()
        {
            // 使用者帳號
            this._userAccount = FISCA.Authentication.DSAServices.UserAccount.Replace("'", "''");

            QueryHelper qh = new QueryHelper();

            #region 使用者姓名
            {
                string sql = string.Format(@"
SELECT
    *
FROM
    teacher
WHERE
    st_login_name = '{0}'
            ", this._userAccount);

                DataTable dt = qh.Select(sql);

                if (dt.Rows.Count > 0)
                {
                    this._userName = "" + dt.Rows[0]["teacher_name"];
                }
                else
                {
                    this._userName = this._userAccount;
                }
            }
            #endregion

            #region 秩序競賽管理員角色編號
            {
                string sql = @"SELECT * FROM _role WHERE role_name = '秩序競賽管理員'";

                DataTable dt = qh.Select(sql);

                if (dt.Rows.Count > 0)
                {
                    this._roleAdminID = "" + dt.Rows[0]["id"];
                }
            }
            #endregion

            #region 檢查使用者是否為管理員角色
            {
                string sql = string.Format(@"
SELECT 
    _login.*
FROM
    _login
    LEFT OUTER JOIN _lr_belong
        ON _login.id = _lr_belong._login_id
WHERE
    _login.login_name = '{0}'
    AND _lr_belong._role_id = {1}
                ", this._userAccount, this._roleAdminID);

                DataTable dt = qh.Select(sql);

                this._isAdmin = dt.Rows.Count > 0;
            }
            #endregion
        }

        /// <summary>
        /// 取得_LoginID
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public string GetLoginIDByAccount(string Account)
        {
            string loginID;
            string sql = string.Format("SELECT * FROM _login WHERE login_name = '{0}'", Account);
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            if (dt.Rows.Count > 0)
            {
                loginID = "" + dt.Rows[0]["id"];
            }
            else
            {
                loginID = "";
            }

            return loginID;
        }

        /// <summary>
        /// 取得使用者帳號
        /// </summary>
        /// <returns></returns>
        public string GetUserAccount()
        {
            return _userAccount;
        }

        /// <summary>
        /// 取得使用者姓名
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return _userName;

        }

        public bool CheckAdmin()
        {
            return this._isAdmin;
        }

        public string GetRoleAdminID()
        {
            return this._roleAdminID;
        }
    }
}
