using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Security
{
    public class HZHSecurity
    {
        /// <summary>
        /// 加密XML
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEncryptString(string xml, string key)
        {
            string source = xml;
            xml = xml + key;
            return SecurityService.Encrypt(xml, SymmProvEnum.MD5) + source;
        }
    }
}
