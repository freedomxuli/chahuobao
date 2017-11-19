using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace ChaHuoBaoWeb.Models
{
    public class CookieModels
    {
        public class CustomPrincipal : IPrincipal
        {
            public IIdentity Identity { get; private set; }
            public UserData UserData { get; set; }
            public bool IsInRole(string role)
            {
                if (string.IsNullOrEmpty(role))
                {
                    return true;
                }
                if (this.UserData == null)
                {
                    return false;
                }
                else
                {
                    return role.Split(',').Any(q => UserData.Roles.Contains(q));
                }
            }
            public CustomPrincipal(IIdentity identity, UserData UserData)
            {
                this.Identity = identity;
                this.UserData = UserData;

            }
        }
        public class myidentity : System.Security.Principal.IIdentity
        {
            #region IIdentity 成员
            // 摘要:
            //     获取所使用的身份验证的类型。
            //
            // 返回结果:
            //     用于标识用户的身份验证的类型。
            public string AuthenticationType
            {
                get { throw new NotImplementedException(); }
            }
            //
            // 摘要:
            //     获取一个值，该值指示是否验证了用户。
            //
            // 返回结果:
            //     如果用户已经过验证，则为 true；否则为 false。
            public bool IsAuthenticated
            {
                get;
                set;
            }
            //
            // 摘要:
            //     获取当前用户的名称。
            //
            // 返回结果:
            //     用户名，代码当前即以该用户的名义运行。
            public string Name
            {
                get;
                set;
            }
            public string yonghuming { get; set; }
            public string mima { get; set; }
            public string UserID { get; set; }
            #endregion
        }
        public class UserData
        {
            /// <summary>
            /// ID
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 角色ID列表
            /// </summary>
            public List<string> Roles { get; set; }
        }
    }
}