using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Common.Security
{
    public class SecurityHelper
    {
        public SecurityHelper()
        { }
        public string Decrypt(string source, string key, string iv, SymmProvEnum type)
        {
            switch (type)
            {
                case SymmProvEnum.DES:
                    return this.DecryptDES(source, key, iv);

                case SymmProvEnum.RC2:
                    return this.DecryptRC2(source, key, iv);

                case SymmProvEnum.Rijndael:
                    return this.DecryptRijndael(source, key, iv);

                case SymmProvEnum.TripleDES:
                    return this.DecryptTripleDES(source, key, iv);
            }
            throw new Exception("\u627e\u4e0d\u5230\u6307\u5b9a\u7684\u89e3\u5bc6\u51fd\u6570\uff01");
        }

        private string DecryptDES(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.DES);
            return crypto1.Decrypting(Source, Key, IV);
        }
        private string DecryptRC2(string Source, string Key, string IV) 
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.RC2);
            return crypto1.Decrypting(Source, Key, IV);
        }

        private string DecryptRijndael(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.Rijndael);
            return crypto1.Decrypting(Source, Key, IV);
        }
        private string DecryptTripleDES(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.TripleDES);
            return crypto1.Decrypting(Source, Key, IV);
        }

        public string Encrypt(string source, string key, string iv, SymmProvEnum type)
        {
            switch (type)
            {
                case SymmProvEnum.DES:
                    return this.EncryptDES(source, key, iv);

                case SymmProvEnum.RC2:
                    return this.EncryptRC2(source, key, iv);

                case SymmProvEnum.Rijndael:
                    return this.EncryptRijndael(source, key, iv);

                case SymmProvEnum.TripleDES:
                    return this.EncryptTripleDES(source, key, iv);
                case SymmProvEnum.MD5:
                    return this.EncryptMD5(source);
            }
            throw new Exception("\u627e\u4e0d\u5230\u6307\u5b9a\u7684\u52a0\u5bc6\u51fd\u6570\uff01");
        }

        private string EncryptDES(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.DES);
            return crypto1.Encrypting(Source, Key, IV);
        }
        private string EncryptRC2(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.RC2);
            return crypto1.Encrypting(Source, Key, IV);
        }
        private string EncryptRijndael(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.Rijndael);
            return crypto1.Encrypting(Source, Key, IV);
        }
        private string EncryptTripleDES(string Source, string Key, string IV)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.TripleDES);
            return crypto1.Encrypting(Source, Key, IV);
        }

        private string EncryptMD5(string source)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(Encoding.UTF8.GetBytes(source))).Replace("-", "");
        }

        public string Hash(string source, SymmProvEnum type)
        {
            switch (type)
            {
                case SymmProvEnum.MD5:
                    return this.MD5hash(source);

                case SymmProvEnum.SHA1:
                    return this.SHA1Managed(source);
            }
            throw new Exception("\u627e\u4e0d\u5230Hash\u51fd\u6570");
        }
        private string MD5hash(string Source)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.MD5);
            return crypto1.HashAsBase64(Source);
        }
        private string SHA1Managed(string Source)
        {
            SymmCrypto crypto1 = new SymmCrypto(SymmProvEnum.SHA1);
            return crypto1.HashAsBase64(Source);
        }

    }
}
