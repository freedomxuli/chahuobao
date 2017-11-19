using System;
using System.Net;
using System.Web;
using Common.Extensions;

namespace Common {
    /// <summary>
    /// 网络操作
    /// </summary>
    public class NetHelper {

        #region Ip(获取Ip)

        /// <summary>
        /// 获取Ip
        /// </summary>
        public static string Ip {
            get {
                try {
                    var ip = string.Empty;
                    if ( HttpContext.Current != null )
                        ip = HttpContext.Current.Request.UserHostAddress;
                    if ( !ip.IsEmpty() && !ip.Contains( ":" ) )
                        return ip;
                    return GetLanIp();
                }
                catch {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        private static string GetLanIp() {
            var addressList = Dns.GetHostEntry( Dns.GetHostName() ).AddressList;
            foreach( var address in addressList ) {
                if ( address.ToString().Contains( "%" ) )
                    continue;
                return address.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region 获取IP值，将IP地址转换为对应的数值
        /// <summary>
        /// 获取IP值，将IP地址转换为对应的数值
        /// </summary>
        /// <returns></returns>
        public static long GetIpValue(String ip)
        {
            string[] ipArr = StringHelper.Split(ip, ".");

            long value = 0;

            try
            {
                value = Convert.ToInt64(ipArr[0]) * 256 * 256 * 256 + Convert.ToInt64(ipArr[1]) * 256 * 256 + Convert.ToInt64(ipArr[2]) * 256 + Convert.ToInt64(ipArr[3]);
            }
            catch
            {
                value = 0;
            }

            return value;
        }
        #endregion

        #region Host(获取主机名)

        /// <summary>
        /// 获取主机名
        /// </summary>
        public static string Host {
            get {
                return Dns.GetHostName();
            }
        }

        #endregion
    }
}
