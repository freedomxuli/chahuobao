using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Common.Security
{
    public class SymmCrypto
    {
        public SymmCrypto(SymmProvEnum NetSelected)
        {
            switch (NetSelected)
            {
                case SymmProvEnum.DES:
                    this.mobjCryptoService = new DESCryptoServiceProvider();
                    this.mobjCryptoService.Mode = CipherMode.CBC;
                    this.mobjCryptoService.Padding = PaddingMode.Zeros;
                    return;

                case SymmProvEnum.RC2:
                    this.mobjCryptoService = new RC2CryptoServiceProvider();
                    this.mobjCryptoService.Mode = CipherMode.CBC;
                    this.mobjCryptoService.Padding = PaddingMode.Zeros;
                    return;

                case SymmProvEnum.Rijndael:
                    this.mobjCryptoService = new RijndaelManaged();
                    this.mobjCryptoService.Mode = CipherMode.CBC;
                    this.mobjCryptoService.Padding = PaddingMode.Zeros;
                    return;

                case SymmProvEnum.TripleDES:
                    this.mobjCryptoService = new TripleDESCryptoServiceProvider();
                    this.mobjCryptoService.Mode = CipherMode.CBC;
                    this.mobjCryptoService.Padding = PaddingMode.Zeros;
                    return;

                case SymmProvEnum.MD5:
                    this.hashobjCryptoService = new MD5CryptoServiceProvider();
                    return;

                case SymmProvEnum.SHA1:
                    this.hashobjCryptoService = new SHA1Managed();
                    return;
            }
        }
        public string Decrypt(byte[] bytIn, string key, string iv)
        {
            MemoryStream stream1 = new MemoryStream(bytIn, 0, bytIn.Length);
            byte[] buffer1 = this.GetLegalKey(key);
            byte[] buffer2 = this.GetLegalIV(iv);
            this.mobjCryptoService.Key = buffer1;
            this.mobjCryptoService.IV = buffer2;
            ICryptoTransform transform1 = this.mobjCryptoService.CreateDecryptor();
            CryptoStream stream2 = new CryptoStream(stream1, transform1, CryptoStreamMode.Read);
            StreamReader reader1 = new StreamReader(stream2);
            return reader1.ReadToEnd();
        }
        public string Decrypting(string source, string key)
        {
            return this.Decrypting(source, key, key);
        }
        public string Decrypting(string source, string key, string iv)
        {
            byte[] buffer1 = Convert.FromBase64String(source);
            return this.Decrypt(buffer1, key, iv);
        }
        public byte[] Encrypt(string source, string key, string iv)
        {
            byte[] buffer1 = Encoding.UTF8.GetBytes(source);
            MemoryStream stream1 = new MemoryStream();
            byte[] buffer2 = this.GetLegalKey(key);
            byte[] buffer3 = this.GetLegalIV(iv);
            this.mobjCryptoService.Key = buffer2;
            this.mobjCryptoService.IV = buffer3;
            ICryptoTransform transform1 = this.mobjCryptoService.CreateEncryptor();
            CryptoStream stream2 = new CryptoStream(stream1, transform1, CryptoStreamMode.Write);
            stream2.Write(buffer1, 0, buffer1.Length);
            stream2.FlushFinalBlock();
            byte[] buffer4 = stream1.GetBuffer();
            long num1 = (((buffer1.Length - 1) / buffer3.Length) + 1) * buffer3.Length;
            byte[] buffer5 = new byte[num1];
            Array.Copy(buffer4, buffer5, num1);
            return buffer5;
        }
        public string Encrypting(string source, string key)
        {
            return this.Encrypting(source, key, key);
        }
        public string Encrypting(string source, string key, string iv)
        {
            byte[] buffer1 = this.Encrypt(source, key, iv);
            return Convert.ToBase64String(buffer1, 0, buffer1.Length);
        }
        public byte[] GetLegalIV(string iv)
        {
            string text1;
            byte[] buffer1 = this.mobjCryptoService.IV;
            if (iv.Length <= buffer1.Length)
            {
                text1 = iv.PadRight(buffer1.Length, ' ');
            }
            else
            {
                text1 = iv.Substring(0, buffer1.Length);
            }
            return Encoding.ASCII.GetBytes(text1);
        }
        private byte[] GetLegalKey(string Key)
        {
            string text1;
            if (this.mobjCryptoService.LegalKeySizes.Length > 0)
            {
                int num1 = 0;
                int num2 = this.mobjCryptoService.LegalKeySizes[0].MinSize;
                while ((Key.Length * 8) > num2)
                {
                    num1 = num2;
                    num2 += this.mobjCryptoService.LegalKeySizes[0].SkipSize;
                }
                text1 = Key.PadRight(num2 / 8, ' ');
            }
            else
            {
                text1 = Key;
            }
            return Encoding.ASCII.GetBytes(text1);
        }
        public byte[] Hash(string source)
        {
            byte[] buffer1 = Encoding.UTF8.GetBytes(source);
            return this.hashobjCryptoService.ComputeHash(buffer1);
        }
        public string HashAsBase64(string source)
        {
            byte[] buffer1 = this.Hash(source);
            return Convert.ToBase64String(buffer1, 0, buffer1.Length);
        }

        // Fields
        private HashAlgorithm hashobjCryptoService;
        private SymmetricAlgorithm mobjCryptoService;

    }
}
