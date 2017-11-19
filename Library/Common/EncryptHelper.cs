using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>���ܲ�����</summary>
    public class EncryptHelper
    {
        #region Rijndael(AES)
        private static byte[] bIv = new byte[] { 51, 97, 57, 49, 53, 48, 100, 52, 57, 54, 57, 52, 101, 100, 50, 50 };

        /// <summary> Rijndael(AES)����</summary>
        /// <param name="str">ԭ��</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string Rijndael_Encode(string str, string key)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(key);

            return Rijndael_Encode(str, bKey, bIv);
        }

        /// <summary> Rijndael(AES)����</summary>
        /// <param name="str">ԭ��</param>
        /// <param name="key">key</param>
        /// <param name="iv">IV</param>
        /// <returns></returns>
        private static string Rijndael_Encode(string str, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (str == null || str.Length <= 0)
                return "";
            if (key == null || key.Length <= 0)
                return "";
            if (iv == null || iv.Length <= 0)
                return "";

            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(str);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);

        }

        /// <summary> Rijndael(AES)����</summary>
        /// <param name="str">����</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string Rijndael_Decode(string str, string key)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(key);

            //�ж��Ƿ�Ϊ base64
            if ((str.Length % 4) != 0)
            {
                return "";
            }
            return Rijndael_Decode(str, bKey, bIv);
        }

        /// <summary> Rijndael(AES)����</summary>
        /// <param name="str">����</param>
        /// <param name="key">key</param>
        /// <param name="iv">IV</param>
        /// <returns></returns>
        private static string Rijndael_Decode(string str, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (str == null || str.Length <= 0)
                return "";
            if (key == null || key.Length <= 0)
                return "";
            if (iv == null || iv.Length <= 0)
                return "";

            byte[] bytes = null;
            try
            {
                bytes = Convert.FromBase64String(str);
            }
            catch (Exception)
            {

                return "";
            }

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
        #endregion

        #region MD5
        /// <summary>MD5����(32bit or 16bit)</summary>
        /// <param name="str">ԭʼ�ַ���</param>
        /// <param name="type">16 or 32</param>
        /// <returns>MD5���</returns>
        public static string Md5(string str, int type = 32)
        {
            string result = "";
            result = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(str)));

            if (type == 16)
            {
                result = result.Substring(8, 16);
            }
            return result.Replace("-", "");
        }
        #endregion

        #region EnCode ����
        /// <summary>EnCode ����    
        /// </summary>    
        /// <param name="data">Ҫ���ܵ��ַ���</param>    
        /// <param name="IV_64">Կ��(����Ϊ8�ֽ�)</param>
        /// <param name="KEY_64">��(����Ϊ8�ֽ�)</param>
        /// <returns>���ܺ���ַ���</returns>    
        public static string EnCode(string data, String KEY_64, String IV_64)
        {
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey,

        byIV), CryptoStreamMode.Write);

                StreamWriter sw = new StreamWriter(cst);
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            catch (Exception)
            {

                return data;
            }

        }
        #endregion

        #region DeCode ����
        /// <summary>DeCode ����    
        /// </summary>    
        /// <param name="data">Ҫ���ܵ��ַ���</param>  
        /// <param name="IV_64">Կ��(����Ϊ8�ֽ�)</param>
        /// <param name="KEY_64">��(����Ϊ8�ֽ�)</param>
        /// <returns>���ܺ���ַ���</returns>    
        public static string DeCode(string data, String KEY_64, String IV_64)
        {
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

                byte[] byEnc;
                try
                {
                    byEnc = Convert.FromBase64String(data);
                }
                catch
                {
                    return null;
                }

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey,

        byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                return sr.ReadToEnd();
            }
            catch (Exception)
            {

                return "";
            }

        }
        #endregion

        #region base64�㷨����
        /// <summary>���ַ���ʹ��base64�㷨����
        /// </summary>
        /// <param name="source">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string Base64_Encrypt(string source)
        {
            return Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(source));
        }
        #endregion

        #region base64�㷨����
        /// <summary>��Base64������ַ����л�ԭ�ַ�����֧������
        /// </summary>
        /// <param name="source">��Ҫ�����ַ���</param>
        /// <returns>�������ַ���</returns>
        public static string Base64_Decrypt(string source)
        {
            return System.Text.Encoding.Default.GetString(Convert.FromBase64String(source));
        }
        #endregion

    }
}
