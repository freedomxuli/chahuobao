using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Extensions;

namespace Common
{
    /// <summary>
    /// 字符串操作
    /// </summary>
    public class StringHelper
    {
        /**************************************************************************/
        #region 字符操作函数

        #region Splice(拼接集合元素)

        /// <summary>
        /// 拼接集合元素
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Splice<T>(IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            var result = new System.Text.StringBuilder();
            foreach (var each in list)
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            return result.ToString().TrimEnd(separator.ToCharArray());
        }

        #endregion

        #region Split（分割字符串）
        /// <summary>分割字符串</summary>
        /// <param name="text">要分割的字符串</param>
        /// <param name="separator">分隔符，默认为逗号</param>
        /// <returns>string[]</returns>
        public static string[] Split(string text, string separator = ",")
        {
            var value = text.ToStr();
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(separator))
            {
                return new string[0] { };
            }
            else
            {
                if (value.IndexOf(separator) < 0)
                {
                    string[] tmp = { value };
                    return tmp;
                }
                return Regex.Split(value, Regex.Escape(separator), RegexOptions.IgnoreCase);
            }
        }
        #endregion

        #region Distinct(去除重复)

        /// <summary>
        /// 去除重复
        /// </summary>
        /// <param name="value">值，范例1："5555",返回"5",范例2："4545",返回"45"</param>
        public static string Distinct(string value)
        {
            var array = value.ToCharArray();
            return new string(array.Distinct().ToArray());
        }

        #endregion

        #region 全角与半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSbc(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDbc(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        #region 从字符串右边返回指定数目的字符
        /// <summary>从字符串右边返回指定数目的字符。</summary>
        /// <param name="text">字符串</param>
        /// <param name="length">截取的长度(字数)</param>
        /// <param name="isFilterXss">是否去除特殊符号</param>
        /// <returns></returns>
        public static string Right(string text, int length, bool isFilterXss = true)
        {
            text = FilterSql(text.ToStr(), isFilterXss);

            if (length > 0)
            {
                if (text.Length > length)
                {
                    int pos = text.Length - length;
                    return text.Substring(pos);
                }
                return text;
            }
            return text;
        }
        #endregion

        #region 从字符串左边截取指定数目的字符(区分中英文长度)

        /// <summary>截取字符串,从左边算起 n 个字,并经过 SQL注入过滤 处理(不区分中英文长度)，默认去左右两边空格与过滤XSS攻击字符</summary>
        /// <param name="text">字符串</param>
        /// <param name="length">截取的长度(字数)，长度为0或小于0时，则返回过滤后的全部字条串</param>
        /// <param name="isFilterXss">是否去除特殊符号</param>
        /// <param name="isAddEndStr">当字符串长度超出指定长度时，是否在字符串尾部添加“...”字符</param>
        /// <returns></returns>
        public static string Left(string text, int length, bool isFilterXss = true, bool isAddEndStr = false)
        {
            text = text.ToStr();

            //过滤字符串
            text = FilterSql(text, isFilterXss);

            //长度为0或小于0时，则返回全部字条串
            if (text.Length <= 0 || length <= 0)
                return text;

            //取得字符串长度
            var textLength = GetLength(text);
            //判断字符串是否超出指定长度
            if (textLength > length)
            {
                var sb = new StringBuilder();
                var len = 0;
                for (int i = 0; i < textLength; i++)
                {
                    //判断是否是中文字符，汉字长度为2要多加一个1
                    if (Math.Abs(text[i]) > 255) len++;
                    len++;
                    //判断是否超出指定长度，是的话退出循环
                    if (len > length)
                    {
                        break;
                    }
                    //将字符添加进容器
                    sb.Append(text[i].ToString());
                }

                text = sb.ToString();
                //字数超过未尾加符号
                if (isAddEndStr)
                    text += "...";
            }
            return text;
        }
        #endregion

        #region 返回字符串真实长度(字节数), 1个汉字长度为2
        /// <summary> 返回字符串真实长度(字节数), 1个汉字长度为2</summary>
        /// <param name="text">字符串</param>
        /// <returns>字符字节数</returns>
        public static int GetLength(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            return Encoding.Default.GetBytes(text).Length;
        }
        #endregion

        #region 替换字符串
        /// <summary>替换字符串</summary>
        /// <param name="sourceString">原字符串</param>
        /// <param name="searchString">查找的字符串</param>
        /// <param name="replaceString">替换的字符串</param>
        /// <param name="isCaseInse">是否区分大小写,true=不区分,false=区分大小写</param>
        /// <returns></returns>
        public static string ReplaceString(string sourceString, string searchString, string replaceString, bool isCaseInse)
        {
            return Regex.Replace(sourceString, Regex.Escape(searchString), replaceString, isCaseInse ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        #endregion

        #endregion
        /**************************************************************************/

        /**************************************************************************/
        #region 安全操作函数

        #region 过滤 Sql 语句字符串中的注入脚本
        /// <summary>过滤 Sql 语句字符串中的注入脚本
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <param name="isFilterXss">是否去除特殊符号</param>
        /// <returns></returns>
        public static string FilterSql(string source, bool isFilterXss = false)
        {
            source = source.ToStr();

            if (source.IsEmpty())
            {
                return "";
            }

            //去除'
            source = source.Replace("'", "");

            //防止16进制注入
            source = source.Replace(new string((char)0, 1), "");
            source = source.Replace("0x", "0");

            if (isFilterXss)
            {
                source = XssTextClear(source);
            }
            return source;

        }
        #endregion

        #region 清除输入字符串中的特殊字符，防XSS攻击
        /// <summary>清除输入字符串中的特殊字符
        /// </summary>
        /// <param name="xssText">输入字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string XssTextClear(string xssText)
        {
            xssText = xssText.ToStr().Replace("<", "").Replace("%3C", "");//去除<
            xssText = xssText.Replace(">", "").Replace("%3E", "");//去除>
            xssText = xssText.Replace("'", "").Replace("%27", "");//去除'
            xssText = xssText.Replace("&", "");
            xssText = xssText.Replace("\\", "");
            return xssText;
        }
        #endregion

        #endregion
        /**************************************************************************/

        /**************************************************************************/
        #region 验证操作函数

        #region 检测是否符合email格式
        /// <summary>检测是否符合email格式</summary>
        /// <param name="text">要判断的email字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string text)
        {
            return RegexHelper.IsMatch(text.ToStr(), @"^([a-zA-Z0-9_.-]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 是否由字母组成
        /// <summary>检查输入,是否由字母组成</summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsEnglish(string text)
        {
            return RegexHelper.IsMatch(text.ToStr(), @"^[a-zA-Z]+[a-zA-z]$+");
        }
        #endregion

        #region 检查用户注册账号是否合法字符(由字母,数字或"_"组成)，且第一个字符为字母
        /// <summary>检查输入,检查用户注册账号是否合法字符(由字母,数字或"_"组成)，且第一个字符是否为字母</summary>
        /// <param name="text">字符串</param>
        /// <param name="firstStringIsEnglist">第一个字符是否为字母</param>
        /// <returns></returns>
        public static bool IsRegeditName(string text, bool firstStringIsEnglist = true)
        {
            if (!firstStringIsEnglist)
            {
                return RegexHelper.IsMatch(text.ToStr(), @"^[a-zA-Z]+[\d_a-zA-z]*$+");
            }
            else
            {
                return RegexHelper.IsMatch(text.ToStr(), @"^[a-zA-Z]+[\da-zA-z]*$+");
            }
        }
        #endregion

        #region 检查是否为正常的手机号码
        /// <summary>检查是否为正常的手机号码.</summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsMobile(string text)
        {
            return RegexHelper.IsMatch(text.ToStr(), @"^1[3|4|5|8][0-9]\d{8}$");
        }
        #endregion

        #region 验证身份证是否合法
        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="idCard">字符串</param>
        /// <returns></returns>
        public static bool IsIdCard(string idCard)
        {
            //15位和18位身份证号码的正则表达式
            var regIdCard = @"^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$";

            //如果通过该验证，说明身份证格式正确，但准确性还需计算
            if (RegexHelper.IsMatch(idCard.ToStr(), regIdCard))
            {
                if (idCard.Length == 18)
                {
                    //将前17位加权因子保存在数组里
                    var idCardWi = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                    //这是除以11后，可能产生的11位余数、验证码，也保存成数组
                    var idCardY = new int[] { 1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                    //用来保存前17位各自乖以加权因子后的总和
                    var idCardWiSum = 0;

                    for (var i = 0; i < 17; i++)
                    {
                        idCardWiSum += idCard.Substring(i, 1).ToInt0() * idCardWi[i];
                    }
                    //计算出校验码所在数组的位置
                    var idCardMod = idCardWiSum % 11;
                    //得到最后一位身份证号码
                    var idCardLast = idCard.Substring(17);

                    //如果等于2，则说明校验码是10，身份证号码最后一位应该是X
                    if (idCardMod == 2)
                    {
                        if (idCardLast == "X" || idCardLast == "x")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //用计算出的验证码与最后一位身份证号码匹配，如果一致，说明通过，否则是无效的身份证号码
                        if (idCardLast.ToInt0() == idCardY[idCardMod])
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
            return false;
        }
        #endregion

        #region 是否包含中文字符

        /// <summary>
        /// 是否包含中文字符
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsChinese(string text)
        {
            const string pattern = "[\u4e00-\u9fa5]+";
            return Regex.IsMatch(text, pattern);
        }


        /// <summary>
        /// 是否只有中文字符
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsOnlyChinese(string text)
        {
            char[] words = text.ToCharArray();

            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString()))   // it is a GB2312 or GBK chinese word    
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word">文本</param>
        private static bool IsGBCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);

            if (bytes.Length <= 1)   // if there is only one byte, it is ASCII code or other code    
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];

                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)     //判断是否是GB2312   
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断一个word是否为GBK编码的汉字  
        /// </summary>
        /// <param name="word">文本</param>
        private static bool IsGBKCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());

            if (bytes.Length <= 1)   // if there is only one byte, it is ASCII code   
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];

                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码   
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region 验证密码是否是6位以上字符
        /// <summary>验证密码是否是6位以上字符.</summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsCode(string text)
        {
            //模式字符串
            var pattern = @"\S{6,}";

            return RegexHelper.IsMatch(text, pattern);
        }
        #endregion

        #region 是否正确的车牌号码

        /// <summary>
        /// 是否正确的车牌号码
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsPlates(string text)
        {
            //模式字符串
            var pattern = @"\d{5}$|[A-Z]{1}\d{4}|\d{1}[A-Z]{1}\d{3}|[A-Z]{2}\d{3}$";

            return RegexHelper.IsMatch(text, pattern);
        }
        #endregion

        #endregion
        /**************************************************************************/
    }
}
