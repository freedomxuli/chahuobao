using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChaHuoBaoWeb.Models;

namespace ChaHuoBaoWeb.PublickFunction
{
    public class SearchQuanXian
    {
        /// <summary>
        /// 通过用户名和检测是否具有权限路径，实际是通过角色检索。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public static bool CheckUserHasPermision(string Username, string Actionname)
        {
            ChaHuoBaoModels db = new ChaHuoBaoModels();
            string[] roleids = db.User_Role.Where(g => g.User.UserName == Username).Select(e => e.RoleID).ToArray();

            if (db.Role_QuanXian.Where(g => roleids.Contains(g.RoleID)).Select(e => e.QuanXian).Where(g => g.QuanXianName == Actionname).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}