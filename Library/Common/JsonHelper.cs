using System.Data;
using System.Text;
using Newtonsoft.Json;
using Common.Extensions;

namespace Common
{
    /// <summary>Json的封装函数</summary>
    public class JsonHelper
    {
        #region 检查字符串是否json格式
        /// <summary>检查字符串是否json格式</summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsJson(string text)
        {
            if (text.ToStr() == "" || text.Length < 3)
            {
                return false;
            }

            if (text.Substring(0, 2) == "{\"" || text.Substring(0, 3) == "[{\"")
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Json序列化
        /// <summary>Json序列化</summary>
        /// <param name="obj">object </param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            var idtc = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

            return JsonConvert.SerializeObject(obj, idtc);
        }
        #endregion

        #region Json反序列化
        /// <summary>反序列化</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="text">json字符串</param>
        /// <returns>类型数据</returns>
        public static T ToObject<T>(string text)
        {
            return (T)JsonConvert.DeserializeObject(text, typeof(T));
        }
        #endregion

        #region DataTable 转换为数组
        /// <summary>
        /// 反回JSON数据到前台
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>JSON字符串</returns>
        public static string CreateJsonParameters(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();

            JsonString.Append("[ ");
            //Exception Handling        
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    /**/
                    /*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
            }
            JsonString.Append("]");
            return JsonString.ToString();
        }
        #endregion 
    }
}