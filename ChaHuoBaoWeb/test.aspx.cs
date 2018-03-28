using ChaHuoBaoWeb.PublickFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChaHuoBaoWeb
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetYanZhengMa getyanzhenma = new GetYanZhengMa();
            string yanzhengma = getyanzhenma.testmessage();
            int aa = 0;
        }
    }
}