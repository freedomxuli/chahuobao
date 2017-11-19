using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChaHuoBaoWeb.PublickFunction
{
    public class GetTableID
    {
        public string gettableid()
        {
            DateTime dttime = DateTime.Now;
            string TableID_str = Guid.NewGuid().ToString();
            //Int64 TableID = Convert.ToInt64(TableID_str);
            return TableID_str;
        }
        public string getdenno()
        {
            DateTime dttime = DateTime.Now;
            string TableID_str = dttime.ToString("yyyyMMddHHmmssfff");
            //Int64 TableID = Convert.ToInt64(TableID_str);
            return TableID_str;
        }
    }
}