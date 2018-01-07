using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using System.IO;
using ChaHuoBaoWeb.Filters;
using System.Collections;
using Common;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class ChongZhiDingDanController : Controller
    {
        //充值首页
        // GET: /ChongZhiDingDan/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string UserName, string OrderDenno, string ZhiFuZhuangTai, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<ChongZhi> chongzhiModel = accountdb.ChongZhi.Include("uesrmodell");
            if (!string.IsNullOrEmpty(UserName))
            {
                chongzhiModel = chongzhiModel.Where(p => p.uesrmodell.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(OrderDenno))
            {
                chongzhiModel = chongzhiModel.Where(p => p.OrderDenno == OrderDenno);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                chongzhiModel = chongzhiModel.Where(x => x.ChongZhiTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                chongzhiModel = chongzhiModel.Where(x => x.ChongZhiTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            if (ZhiFuZhuangTai != "0")
            {
                bool zhuangtai = true;
                if (ZhiFuZhuangTai == "1") { zhuangtai = true; }
                if (ZhiFuZhuangTai == "2") { zhuangtai = false; }
                chongzhiModel = chongzhiModel.Where(p => p.ZhiFuZhuangTai == zhuangtai);
            }
            chongzhiModel = chongzhiModel.OrderByDescending(p => p.ChongZhiTime);
            var total = chongzhiModel.Count();
            var currentPersonList = chongzhiModel
                               .Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<ChongZhilist> chongzhiModels = new List<ChongZhilist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                string zhifuzhuangtai = "";
                ChongZhilist chongzhione = new ChongZhilist();
                if (obj.ZhiFuZhuangTai == true)
                {
                    zhifuzhuangtai = "已支付";
                }
                else
                {
                    zhifuzhuangtai = "未支付";

                }

                chongzhione.xuhao = n;
                chongzhione.OrderDenno = obj.OrderDenno;
                chongzhione.UserName = obj.uesrmodell.UserName;
                chongzhione.ChongZhiCiShu = obj.ChongZhiCiShu;
                chongzhione.ChongZhiTime = obj.ChongZhiTime;
                chongzhione.ZhiFuZhuangTai = zhifuzhuangtai;
                chongzhione.ChongZhiRemark = obj.ChongZhiRemark;
                chongzhione.ChongZhiJinE = obj.ChongZhiJinE;
                chongzhiModels.Add(chongzhione);
            }
            var rows = chongzhiModels.Select(p => new
            {
                id = p.xuhao,
                OrderDenno = p.OrderDenno,
                UserName = p.UserName,
                ChongZhiTime = p.ChongZhiTime.ToString(),
                ChongZhiCiShu = p.ChongZhiCiShu,
                ZhiFuZhuangTai = p.ZhiFuZhuangTai,
                ChongZhiRemark = p.ChongZhiRemark,
                ChongZhiJinE = p.ChongZhiJinE

            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SHIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SHIndex(string UserName, string OrderDenno, string ZhiFuZhuangTai,string ChongZhiSH, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<ChongZhi> chongzhiModel = accountdb.ChongZhi.Include("uesrmodell");
            chongzhiModel = chongzhiModel.Where(p => p.ChongZhiDescribe == "公对公");
            if (!string.IsNullOrEmpty(UserName))
            {
                chongzhiModel = chongzhiModel.Where(p => p.uesrmodell.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(OrderDenno))
            {
                chongzhiModel = chongzhiModel.Where(p => p.OrderDenno == OrderDenno);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                chongzhiModel = chongzhiModel.Where(x => x.ChongZhiTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                chongzhiModel = chongzhiModel.Where(x => x.ChongZhiTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            if (ZhiFuZhuangTai != "0")
            {
                bool zhuangtai = true;
                if (ZhiFuZhuangTai == "1") { zhuangtai = true; }
                if (ZhiFuZhuangTai == "2") { zhuangtai = false; }
                chongzhiModel = chongzhiModel.Where(p => p.ZhiFuZhuangTai == zhuangtai);
            }
            if (ChongZhiSH != "0")
            {
                bool zhuangtai = true;
                if (ChongZhiSH == "1") { zhuangtai = true; }
                if (ChongZhiSH == "2") { zhuangtai = false; }
                chongzhiModel = chongzhiModel.Where(p => p.ChongZhiSH == zhuangtai);
            }
            chongzhiModel = chongzhiModel.OrderByDescending(p => p.ChongZhiTime);
            var total = chongzhiModel.Count();
            var currentPersonList = chongzhiModel
                               .Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<ChongZhilist> chongzhiModels = new List<ChongZhilist>();
            foreach (var obj in currentPersonList)
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.ChongZhiGDG gdgmodel = db.ChongZhiGDG.Where(g => g.OrderDenno == obj.OrderDenno).First();

                n = n + 1;
                string zhifuzhuangtai = "";
                ChongZhilist chongzhione = new ChongZhilist();
                if (obj.ZhiFuZhuangTai == true)
                {
                    zhifuzhuangtai = "已支付";
                }
                else
                {
                    zhifuzhuangtai = "未支付";
                }

                string shzhuangtai = "";
                if (obj.ChongZhiSH == true)
                {
                    shzhuangtai = "已审核";
                }
                else
                {
                    shzhuangtai = "未审核";
                }
                string IsShPass = "";
                if (!string.IsNullOrEmpty(obj.IsShPass.ToString()))
                {
                    if (obj.IsShPass == 1)
                        IsShPass = "通过";
                    else
                        IsShPass = "不通过";
                }

                chongzhione.xuhao = n;
                chongzhione.OrderDenno = obj.OrderDenno;
                chongzhione.UserName = obj.uesrmodell.UserName;
                chongzhione.ChongZhiCiShu = obj.ChongZhiCiShu;
                chongzhione.ChongZhiTime = obj.ChongZhiTime;
                chongzhione.ZhiFuZhuangTai = zhifuzhuangtai;
                chongzhione.ChongZhiRemark = obj.ChongZhiRemark;
                chongzhione.ChongZhiJinE = obj.ChongZhiJinE;
                chongzhione.ChongZhiSH = shzhuangtai;
                chongzhione.IsShPass = IsShPass;
                chongzhione.DGZZCompany = gdgmodel.DGZZCompany;
                chongzhione.DGZH = gdgmodel.DGZH;
                chongzhione.DKPZH = gdgmodel.DKPZH;
                chongzhiModels.Add(chongzhione);
            }
            var rows = chongzhiModels.Select(p => new
            {
                id = p.xuhao,
                OrderDenno = p.OrderDenno,
                UserName = p.UserName,
                ChongZhiTime = p.ChongZhiTime.ToString(),
                ChongZhiCiShu = p.ChongZhiCiShu,
                ZhiFuZhuangTai = p.ZhiFuZhuangTai,
                ChongZhiRemark = p.ChongZhiRemark,
                ChongZhiJinE = p.ChongZhiJinE,
                ChongZhiSH = p.ChongZhiSH,
                IsShPass = p.IsShPass,
                DGZZCompany = p.DGZZCompany,
                DGZH = p.DGZH,
                DKPZH = p.DKPZH
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);

        }

        //审核
        //审核
        //审核
        [PermissionAuthorize]
        public string DataShenhe()
        {
            string OrderDenno = HttpContext.Request["OrderDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.ChongZhi chongzhimodel = db.ChongZhi.Where(g => g.OrderDenno == OrderDenno).First();
                if (chongzhimodel.ChongZhiSH == true)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "此单据已审核！";
                }
                else
                {
                    chongzhimodel.ChongZhiSH = true;

                    chongzhimodel.IsShPass = Int32.Parse(HttpContext.Request["IsShPass"]);

                    db.SaveChanges();

                    if (Int32.Parse(HttpContext.Request["IsShPass"]) == 1) {
                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db2 = new ChaHuoBaoModels();
                        ChaHuoBaoWeb.Models.User use = db2.User.Where(x => x.UserID == chongzhimodel.UserID).First();

                        use.UserRemainder = use.UserRemainder + chongzhimodel.ChongZhiCiShu;

                        db2.SaveChanges();
                    }
                    
                    hash["sign"] = "1";
                    hash["msg"] = "审核成功";
                }
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = ex.Message;
            }

            return JsonHelper.ToJson(hash);
        }

        ////导出excel充值订单表内容
        [PermissionAuthorize]
        public FileResult Export(string UserName, string OrderDenno, string ZhiFuZhuangTai, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder)
        {

            //获取list数据
            IEnumerable<ChongZhi> chongzhiModel = accountdb.ChongZhi.Include("uesrmodell");
            if (!string.IsNullOrEmpty(UserName))
            {
                chongzhiModel = chongzhiModel.Where(p => p.uesrmodell.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(OrderDenno))
            {
                chongzhiModel = chongzhiModel.Where(p => p.OrderDenno == OrderDenno);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                chongzhiModel = chongzhiModel.Where(x => x.ChongZhiTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                chongzhiModel = chongzhiModel.Where(x => x.ChongZhiTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            if (ZhiFuZhuangTai != "0")
            {
                bool zhuangtai = true;
                if (ZhiFuZhuangTai == "1") { zhuangtai = true; }
                if (ZhiFuZhuangTai == "2") { zhuangtai = false; }
                chongzhiModel = chongzhiModel.Where(p => p.ZhiFuZhuangTai == zhuangtai);
            }
            chongzhiModel = chongzhiModel.OrderByDescending(p => p.ChongZhiTime);

            List<ChongZhilist> chongzhiModels = new List<ChongZhilist>();
            foreach (var obj in chongzhiModel)
            {

                string zhifuzhuangtai = "";
                ChongZhilist chongzhione = new ChongZhilist();
                if (obj.ZhiFuZhuangTai == true)
                {
                    zhifuzhuangtai = "已支付";
                }
                else
                {
                    zhifuzhuangtai = "未支付";

                }

                chongzhione.OrderDenno = obj.OrderDenno;
                chongzhione.UserName = obj.uesrmodell.UserName;
                chongzhione.ChongZhiCiShu = obj.ChongZhiCiShu;
                chongzhione.ChongZhiTime = obj.ChongZhiTime;
                chongzhione.ZhiFuZhuangTai = zhifuzhuangtai;
                chongzhione.ChongZhiRemark = obj.ChongZhiRemark;
                chongzhione.ChongZhiJinE = obj.ChongZhiJinE;
                chongzhiModels.Add(chongzhione);
            }

            var list = chongzhiModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("充值用户");
            row1.CreateCell(2).SetCellValue("充值金额");
            row1.CreateCell(3).SetCellValue("充值次数");
            row1.CreateCell(4).SetCellValue("充值时间");
            row1.CreateCell(5).SetCellValue("充值单号");
            row1.CreateCell(6).SetCellValue("支付状态");
            row1.CreateCell(7).SetCellValue("备注");

            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(2).SetCellValue(list[i].ChongZhiJinE.ToString());
                rowtemp.CreateCell(3).SetCellValue(list[i].ChongZhiCiShu);
                rowtemp.CreateCell(4).SetCellValue(list[i].ChongZhiTime.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].OrderDenno);
                rowtemp.CreateCell(6).SetCellValue(list[i].ZhiFuZhuangTai);
                rowtemp.CreateCell(7).SetCellValue(list[i].ChongZhiRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "充值订单.xls");

        }


        public class ChongZhilist
        {
            public int xuhao { get; set; }

            public string OrderDenno { get; set; }

            public string UserName { get; set; }

            public int ChongZhiCiShu { get; set; }

            public DateTime ChongZhiTime { get; set; }

            public string ZhiFuZhuangTai { get; set; }

            public string ChongZhiRemark { get; set; }

            public decimal ChongZhiJinE { get; set; }

            public string ChongZhiSH { get; set; }

            public string IsShPass { get; set; }

            public string DGZZCompany { get; set; }

            public string DGZH { get; set; }

            public string DKPZH { get; set; }
        }
    }
}
