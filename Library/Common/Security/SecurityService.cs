using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Security
{
    public class SecurityService
    {
        // Methods
        static SecurityService()
        {
            SecurityService.DefaultKey = "kt123456";
        }
        public SecurityService()
        { }
        public static string Decrypt(string source, SymmProvEnum type)
        {
            string text1 = SecurityService.DefaultKey;
            return SecurityService.Decrypt(source, text1, type);
        }
        public static string Decrypt(string source, string key, SymmProvEnum type)
        {
            return SecurityService.Decrypt(source, key, key, type);
        }

        public static string Decrypt(string source, string key, string iv, SymmProvEnum type)
        {
            SecurityHelper helper1 = new SecurityHelper();
            return helper1.Decrypt(source, key, iv, type);
        }
        public static string Encrypt(string source, SymmProvEnum type)
        {
            string text1 = SecurityService.DefaultKey;
            return SecurityService.Encrypt(source, text1, type);
        }
        public static string Encrypt(string source, string key, SymmProvEnum type)
        {
            return SecurityService.Encrypt(source, key, key, type);
        }
        public static string Encrypt(string source, string key, string iv, SymmProvEnum type)
        {
            SecurityHelper helper1 = new SecurityHelper();
            return helper1.Encrypt(source, key, iv, type);
        }
        public static string Hash(string source, SymmProvEnum type)
        {
            SecurityHelper helper1 = new SecurityHelper();
            return helper1.Hash(source, type);
        }

        // Fields
        private static string DefaultKey;
    }
}
