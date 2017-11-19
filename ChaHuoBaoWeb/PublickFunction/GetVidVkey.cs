using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ChaHuoBaoWeb.PublickFunction
{
    public class GetVidVkey
    {
        public Hashtable gvk(string GpsDeviceID) 
        {
            LocationJob locajob = new LocationJob();
            Hashtable gpsinfo = locajob.Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + GpsDeviceID + "&pwd=123456");
            if (gpsinfo["success"].ToString().ToUpper() == "True".ToUpper())
            {
                gpsinfo["sign"] = "1";
            }
            else
            {
                gpsinfo["sign"] = "0";
            }
            return gpsinfo;
        }
    }
}