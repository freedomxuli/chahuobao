using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using System.IO;
using ChaHuoBaoWeb.Filters;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class BackControlController : Controller
    {
        //运单查询首页
        // GET: /YunDanChaXun/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string UserName, string QiShiZhan, string DaoDaZhan, string SuoShuGongSi, string UserDenno, string GpsDeviceID, string IsBangding, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<YunDan> yundanModel = accountdb.YunDan.Include("userModelss");
            if (!string.IsNullOrEmpty(UserName))
            {
                yundanModel = yundanModel.Where(p => p.userModelss.UserName.Contains(UserName));
            }
            if (!string.IsNullOrEmpty(QiShiZhan))
            {
                yundanModel = yundanModel.Where(p => p.QiShiZhan.Contains(QiShiZhan));
            }
            if (!string.IsNullOrEmpty(DaoDaZhan))
            {
                yundanModel = yundanModel.Where(p => p.DaoDaZhan.Contains(DaoDaZhan));
            }
            if (!string.IsNullOrEmpty(GpsDeviceID))
            {
                yundanModel = yundanModel.Where(p => p.GpsDeviceID == GpsDeviceID);
            }
            if (!string.IsNullOrEmpty(SuoShuGongSi))
            {
                yundanModel = yundanModel.Where(p => p.SuoShuGongSi.Contains(SuoShuGongSi));
            }
            if (!string.IsNullOrEmpty(UserDenno))
            {
                yundanModel = yundanModel.Where(p => p.UserDenno == UserDenno);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                yundanModel = yundanModel.Where(x => x.BangDingTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                yundanModel = yundanModel.Where(x => x.BangDingTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }

            yundanModel = yundanModel.Where(x => x.Gps_lasttime < DateTime.Now.AddHours(-1));

            if (IsBangding != "0")
            {
                bool Bangding = true;
                if (IsBangding == "1") { Bangding = true; }
                if (IsBangding == "2") { Bangding = false; }
                yundanModel = yundanModel.Where(p => p.IsBangding == Bangding);
            }
            yundanModel = yundanModel.OrderByDescending(p => p.BangDingTime);
            var total = yundanModel.Count();
            var currentPersonList = yundanModel
                               .Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<YunDanlist> yundanModels = new List<YunDanlist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                string bnagding = "";
                YunDanlist yundanone = new YunDanlist();
                if (obj.IsBangding == true)
                {
                    bnagding = "已绑定";
                }
                else
                {
                    bnagding = "未绑定";

                }

                yundanone.xuhao = n;
                yundanone.YunDanDenno = obj.YunDanDenno;
                yundanone.QiShiZhan = obj.QiShiZhan;
                yundanone.DaoDaZhan = obj.DaoDaZhan;
                yundanone.UserDenno = obj.UserDenno;
                yundanone.UserName = obj.userModelss.UserName;
                yundanone.SuoShuGongSi = obj.SuoShuGongSi;
                yundanone.BangDingTime = obj.BangDingTime;
                yundanone.JieBangTime = obj.JieBangTime;
                yundanone.GpsDeviceID = obj.GpsDeviceID;
                yundanone.Gps_lastinfo = obj.Gps_lastinfo;
                yundanone.bnagding = bnagding;
                yundanone.YunDanRemark = obj.YunDanRemark;
                yundanModels.Add(yundanone);
            }
            var rows = yundanModels.Select(p => new
            {
                id = p.xuhao,
                UserDenno = p.UserDenno,
                UserName = p.UserName,
                SuoShuGongSi = p.SuoShuGongSi,

                QiShiZhan = p.QiShiZhan,
                DaoDaZhan = p.DaoDaZhan,
                BangDingTime = p.BangDingTime.ToString(),
                JieBangTime = p.JieBangTime.ToString(),
                GpsDeviceID = p.GpsDeviceID,
                Gps_lastinfo = p.Gps_lastinfo,
                bnagding = p.bnagding,
                YunDanRemark = p.YunDanRemark
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);

        }

        [PermissionAuthorize]
        public FileResult Export(string UserName, string QiShiZhan, string DaoDaZhan, string SuoShuGongSi, string UserDenno, string GpsDeviceID, string IsBangding, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder)
        {

            //获取list数据
            IEnumerable<YunDan> yundanModel = accountdb.YunDan.Include("userModelss");
            if (!string.IsNullOrEmpty(UserName))
            {
                yundanModel = yundanModel.Where(p => p.userModelss.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(QiShiZhan))
            {
                yundanModel = yundanModel.Where(p => p.QiShiZhan.Contains(QiShiZhan));
            }
            if (!string.IsNullOrEmpty(DaoDaZhan))
            {
                yundanModel = yundanModel.Where(p => p.DaoDaZhan.Contains(DaoDaZhan));
            }
            if (!string.IsNullOrEmpty(GpsDeviceID))
            {
                yundanModel = yundanModel.Where(p => p.GpsDeviceID == GpsDeviceID);
            }
            if (!string.IsNullOrEmpty(SuoShuGongSi))
            {
                yundanModel = yundanModel.Where(p => p.SuoShuGongSi.Contains(SuoShuGongSi));
            }
            if (!string.IsNullOrEmpty(UserDenno))
            {
                yundanModel = yundanModel.Where(p => p.UserDenno == UserDenno);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                yundanModel = yundanModel.Where(x => x.BangDingTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                yundanModel = yundanModel.Where(x => x.BangDingTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            yundanModel = yundanModel.Where(x => x.Gps_lasttime < DateTime.Now.AddHours(-1));
            if (IsBangding != "0")
            {
                bool Bangding = true;
                if (IsBangding == "1") { Bangding = true; }
                if (IsBangding == "2") { Bangding = false; }
                yundanModel = yundanModel.Where(p => p.IsBangding == Bangding);
            }
            yundanModel = yundanModel.OrderByDescending(p => p.BangDingTime);

            List<YunDanlist> yundanModels = new List<YunDanlist>();
            foreach (var obj in yundanModel)
            {

                string bnagding = "";
                YunDanlist yundanone = new YunDanlist();
                if (obj.IsBangding == true)
                {
                    bnagding = "已绑定";
                }
                else
                {
                    bnagding = "未绑定";

                }
                yundanone.UserDenno = obj.UserDenno;
                yundanone.SuoShuGongSi = obj.SuoShuGongSi;

                yundanone.QiShiZhan = obj.QiShiZhan;
                yundanone.DaoDaZhan = obj.DaoDaZhan;

                yundanone.BangDingTime = obj.BangDingTime;
                yundanone.JieBangTime = obj.JieBangTime;
                yundanone.GpsDeviceID = obj.GpsDeviceID;
                yundanone.Gps_lastinfo = obj.Gps_lastinfo;
                yundanone.bnagding = bnagding;
                yundanone.YunDanRemark = obj.YunDanRemark;
                yundanModels.Add(yundanone);
            }

            var list = yundanModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("运单编号");
            row1.CreateCell(2).SetCellValue("所属公司");

            row1.CreateCell(3).SetCellValue("起始站");
            row1.CreateCell(4).SetCellValue("到达站");

            row1.CreateCell(5).SetCellValue("设备标识");
            row1.CreateCell(6).SetCellValue("绑定时间");
            row1.CreateCell(7).SetCellValue("解绑时间");
            row1.CreateCell(8).SetCellValue("绑定状态");
            row1.CreateCell(9).SetCellValue("最新记录地址");
            row1.CreateCell(10).SetCellValue("备注");

            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].UserDenno);
                rowtemp.CreateCell(2).SetCellValue(list[i].SuoShuGongSi);

                rowtemp.CreateCell(3).SetCellValue(list[i].QiShiZhan);
                rowtemp.CreateCell(4).SetCellValue(list[i].DaoDaZhan);


                rowtemp.CreateCell(5).SetCellValue(list[i].GpsDeviceID);
                rowtemp.CreateCell(6).SetCellValue(list[i].BangDingTime.ToString());
                rowtemp.CreateCell(7).SetCellValue(list[i].JieBangTime.ToString());
                rowtemp.CreateCell(8).SetCellValue(list[i].bnagding);
                rowtemp.CreateCell(9).SetCellValue(list[i].Gps_lastinfo);
                rowtemp.CreateCell(10).SetCellValue(list[i].YunDanRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "运单列表.xls");

        }

        public class YunDanlist
        {
            public int xuhao { get; set; }

            public string YunDanDenno { get; set; }
            public string UserDenno { get; set; }
            public string UserName { get; set; }
            public string SuoShuGongSi { get; set; }
            public string QiShiZhan { get; set; }
            public string DaoDaZhan { get; set; }

            public DateTime BangDingTime { get; set; }

            public DateTime? JieBangTime { get; set; }

            public string GpsDeviceID { get; set; }

            public string Gps_lastinfo { get; set; }

            public string bnagding { get; set; }

            public string YunDanRemark { get; set; }
        }
    }
}
