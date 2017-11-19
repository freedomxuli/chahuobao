using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using Jayrock.Json.Conversion;

namespace ChaHuoBaoWeb.PublickFunction
{
    public class Map
    {
        public Hashtable getmapinfobyaddress(string address, string city)
        {
            Hashtable hashTable = new Hashtable();

            try
            {
                string url = null;
                url = "http://restapi.amap.com/v3/geocode/geo?key=eeaa068dfa76612008db1232f98ae753&address=" + System.Web.HttpUtility.UrlEncode(address) + "&city=" + System.Web.HttpUtility.UrlEncode(city) + "";

                WebRequest request = WebRequest.Create(url);
                Encoding encode = Encoding.GetEncoding("utf-8");

                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, encode);
                string responseFromServer = reader.ReadToEnd();
                string outStr = responseFromServer;
                reader.Close();
                dataStream.Close();
                response.Close();

                hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(outStr);
                if (hashTable["status"].ToString() == "1")
                {
                    string geocodes = hashTable["geocodes"].ToString().Trim();
                    geocodes = geocodes.Substring(1, geocodes.Length - 3);
                    hashTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Hashtable>(geocodes);
                    hashTable["sign"] = "1";
                    hashTable["msg"] = "success";

                }
                return hashTable;
            }
            catch (Exception ex)
            {
                //AppLog.Error(ex.Message);
                hashTable["sign"] = "0";
                hashTable["msg"] = "请求失败，可能的原因是：" + ex.Message;
                return hashTable;
            }

        }
    }
}