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
    public class SheBeiDingDanController : Controller
    {
        //设备订单首页
        // GET: /SheBeiDingDan/

        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaleIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string GpsDingDanDenno, string UserName, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<GpsDingDan> dingdanModel = accountdb.GpsDingDan.Include("userModel");
            if (!string.IsNullOrEmpty(GpsDingDanDenno))
            {
                dingdanModel = dingdanModel.Where(p => p.OrderDenno == GpsDingDanDenno);
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                dingdanModel = dingdanModel.Where(p => p.userModel.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            dingdanModel = dingdanModel.OrderByDescending(p => p.GpsDingDanTime);
            var total = dingdanModel.Count();
            var currentPersonList = dingdanModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDingDanlist> dingdanModels = new List<GpsDingDanlist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                string zhuangtai = "";
                GpsDingDanlist dingdanone = new GpsDingDanlist();
                if (obj.GpsDingDanZhiFuZhuangTai == true)
                {
                    zhuangtai = "已支付";
                }
                else
                {
                    zhuangtai = "未支付";
                }
                dingdanone.xuhao = n;
                dingdanone.UserName = obj.userModel.UserName;
                dingdanone.GpsDingDanDenno = obj.OrderDenno;
                dingdanone.GpsDingDanJinE = obj.GpsDingDanJinE;
                dingdanone.GpsDingDanShuLiang = obj.GpsDingDanShuLiang;
                dingdanone.GpsDingDanTime = obj.GpsDingDanTime;
                dingdanone.GpsDingDanRemark = obj.GpsDingDanRemark;
                dingdanone.GpsDingDanZhiFuZhuangTai = zhuangtai;
                dingdanModels.Add(dingdanone);

            }
            var rows = dingdanModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsDingDanDenno = p.GpsDingDanDenno,
                GpsDingDanTime = p.GpsDingDanTime.ToString(),
                GpsDingDanJinE = p.GpsDingDanJinE,
                GpsDingDanShuLiang = p.GpsDingDanShuLiang,
                GpsDingDanRemark = p.GpsDingDanRemark,
                GpsDingDanZhiFuZhuangTai = p.GpsDingDanZhiFuZhuangTai
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaleIndex(string GpsDingDanDenno, string UserName, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<GpsDingDanSale> dingdanModel = accountdb.GpsDingDanSale.Include("userModel");
            if (!string.IsNullOrEmpty(GpsDingDanDenno))
            {
                dingdanModel = dingdanModel.Where(p => p.OrderDenno == GpsDingDanDenno);
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                dingdanModel = dingdanModel.Where(p => p.userModel.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            dingdanModel = dingdanModel.OrderByDescending(p => p.GpsDingDanTime);
            var total = dingdanModel.Count();
            var currentPersonList = dingdanModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDingDanlist> dingdanModels = new List<GpsDingDanlist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                string zhuangtai = "";
                GpsDingDanlist dingdanone = new GpsDingDanlist();
                if (obj.GpsDingDanZhiFuZhuangTai == true)
                {
                    zhuangtai = "已支付";
                }
                else
                {
                    zhuangtai = "未支付";
                }
                dingdanone.xuhao = n;
                dingdanone.UserName = obj.userModel.UserName;
                dingdanone.GpsDingDanDenno = obj.OrderDenno;
                dingdanone.GpsDingDanJinE = obj.GpsDingDanJinE;
                dingdanone.GpsDingDanShuLiang = obj.GpsDingDanShuLiang;
                dingdanone.GpsDingDanTime = obj.GpsDingDanTime;
                dingdanone.GpsDingDanRemark = obj.GpsDingDanRemark;
                dingdanone.GpsDingDanZhiFuZhuangTai = zhuangtai;
                dingdanModels.Add(dingdanone);

            }
            var rows = dingdanModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsDingDanDenno = p.GpsDingDanDenno,
                GpsDingDanTime = p.GpsDingDanTime.ToString(),
                GpsDingDanJinE = p.GpsDingDanJinE,
                GpsDingDanShuLiang = p.GpsDingDanShuLiang,
                GpsDingDanRemark = p.GpsDingDanRemark,
                GpsDingDanZhiFuZhuangTai = p.GpsDingDanZhiFuZhuangTai
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }


        //导出excel设备订单表内容
        //导出excel
        [PermissionAuthorize]
        public FileResult Export(string GpsDingDanDenno, string UserName, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder)
        {
            //获取list数据
            IEnumerable<GpsDingDan> dingdanModel = accountdb.GpsDingDan.Include("userModel");
            if (!string.IsNullOrEmpty(GpsDingDanDenno))
            {
                dingdanModel = dingdanModel.Where(p => p.OrderDenno == GpsDingDanDenno);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                dingdanModel = dingdanModel.Where(p => p.userModel.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            dingdanModel = dingdanModel.OrderByDescending(p => p.GpsDingDanTime);
            List<GpsDingDanlist> dingdanModels = new List<GpsDingDanlist>();

            foreach (var obj in dingdanModel)
            {

                string zhuangtai = "";
                GpsDingDanlist dingdanone = new GpsDingDanlist();
                if (obj.GpsDingDanZhiFuZhuangTai == true)
                {
                    zhuangtai = "已支付";
                }
                else
                {
                    zhuangtai = "未支付";
                }

                dingdanone.UserName = obj.userModel.UserName;
                dingdanone.GpsDingDanDenno = obj.OrderDenno;
                dingdanone.GpsDingDanJinE = obj.GpsDingDanJinE;
                dingdanone.GpsDingDanShuLiang = obj.GpsDingDanShuLiang;
                dingdanone.GpsDingDanTime = obj.GpsDingDanTime;
                dingdanone.GpsDingDanRemark = obj.GpsDingDanRemark;
                dingdanone.GpsDingDanZhiFuZhuangTai = zhuangtai;
                dingdanModels.Add(dingdanone);

            }

            var list = dingdanModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("订单编号");
            row1.CreateCell(2).SetCellValue("订单用户");
            row1.CreateCell(3).SetCellValue("设备数量");
            row1.CreateCell(4).SetCellValue("订单押金");
            row1.CreateCell(5).SetCellValue("订单日期");
            row1.CreateCell(6).SetCellValue("支付状态");
            row1.CreateCell(7).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].OrderDenno);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(3).SetCellValue(list[i].GpsDingDanShuLiang);
                rowtemp.CreateCell(4).SetCellValue(list[i].GpsDingDanJinE.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].GpsDingDanTime.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].GpsDingDanZhiFuZhuangTai);
                rowtemp.CreateCell(7).SetCellValue(list[i].GpsDingDanRemark);
                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "设备订单.xls");
        }


        //导出excel设备订单表内容
        //导出excel
        [PermissionAuthorize]
        public FileResult ExportSale(string GpsDingDanDenno, string UserName, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder)
        {
            //获取list数据
            IEnumerable<GpsDingDanSale> dingdanModel = accountdb.GpsDingDanSale.Include("userModel");
            if (!string.IsNullOrEmpty(GpsDingDanDenno))
            {
                dingdanModel = dingdanModel.Where(p => p.OrderDenno == GpsDingDanDenno);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                dingdanModel = dingdanModel.Where(p => p.userModel.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            dingdanModel = dingdanModel.OrderByDescending(p => p.GpsDingDanTime);
            List<GpsDingDanlist> dingdanModels = new List<GpsDingDanlist>();

            foreach (var obj in dingdanModel)
            {

                string zhuangtai = "";
                GpsDingDanlist dingdanone = new GpsDingDanlist();
                if (obj.GpsDingDanZhiFuZhuangTai == true)
                {
                    zhuangtai = "已支付";
                }
                else
                {
                    zhuangtai = "未支付";
                }

                dingdanone.UserName = obj.userModel.UserName;
                dingdanone.GpsDingDanDenno = obj.OrderDenno;
                dingdanone.GpsDingDanJinE = obj.GpsDingDanJinE;
                dingdanone.GpsDingDanShuLiang = obj.GpsDingDanShuLiang;
                dingdanone.GpsDingDanTime = obj.GpsDingDanTime;
                dingdanone.GpsDingDanRemark = obj.GpsDingDanRemark;
                dingdanone.GpsDingDanZhiFuZhuangTai = zhuangtai;
                dingdanModels.Add(dingdanone);

            }

            var list = dingdanModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("订单编号");
            row1.CreateCell(2).SetCellValue("订单用户");
            row1.CreateCell(3).SetCellValue("设备数量");
            row1.CreateCell(4).SetCellValue("订单押金");
            row1.CreateCell(5).SetCellValue("订单日期");
            row1.CreateCell(6).SetCellValue("支付状态");
            row1.CreateCell(7).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].OrderDenno);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(3).SetCellValue(list[i].GpsDingDanShuLiang);
                rowtemp.CreateCell(4).SetCellValue(list[i].GpsDingDanJinE.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].GpsDingDanTime.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].GpsDingDanZhiFuZhuangTai);
                rowtemp.CreateCell(7).SetCellValue(list[i].GpsDingDanRemark);
                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "销售设备订单.xls");
        }


        public class GpsDingDanlist
        {
            public int xuhao { get; set; }

            public string UserName { get; set; }

            public string GpsDingDanDenno { get; set; }

            public string OrderDenno { get; set; }

            public string UserID { get; set; }

            public string GpsDingDanZhiFuZhuangTai { get; set; }

            public decimal GpsDingDanJinE { get; set; }

            public int GpsDingDanShuLiang { get; set; }

            public DateTime GpsDingDanTime { get; set; }

            public string GpsDingDanRemark { get; set; }

            public string GpsDingDanSH { get; set; }

            public string DGZZCompany { get; set; }

            public string DGZH { get; set; }

            public string DKPZH { get; set; }
        }

        public ActionResult SHIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SHIndex(string GpsDingDanDenno, string UserName, DateTime? startDate, DateTime? endDate, string GpsDingDanSH, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {

            IEnumerable<GpsDingDan> dingdanModel = accountdb.GpsDingDan.Include("userModel");
            dingdanModel = dingdanModel.Where(p => p.GpsDingDanZhiFuLeiXing == "公对公");
            if (!string.IsNullOrEmpty(GpsDingDanDenno))
            {
                dingdanModel = dingdanModel.Where(p => p.OrderDenno == GpsDingDanDenno);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                dingdanModel = dingdanModel.Where(p => p.userModel.UserName == UserName);
            }
            if (GpsDingDanSH != "0")
            {
                bool shenhezhuangtai = true;

                if (GpsDingDanSH == "1") { shenhezhuangtai = true; }
                if (GpsDingDanSH == "2") { shenhezhuangtai = false; }

                dingdanModel = dingdanModel.Where(p => p.GpsDingDanSH == shenhezhuangtai);

            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            dingdanModel = dingdanModel.OrderByDescending(p => p.GpsDingDanTime);
            var total = dingdanModel.Count();
            var currentPersonList = dingdanModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDingDanlist> dingdanModels = new List<GpsDingDanlist>();
            foreach (var obj in currentPersonList)
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsDingDanGDG gdgmodel = db.GpsDingDanGDG.Where(g => g.OrderDenno == obj.OrderDenno).First();

                n = n + 1;
                string zhuangtai = "";
                GpsDingDanlist dingdanone = new GpsDingDanlist();
                if (obj.GpsDingDanSH == true)
                {
                    zhuangtai = "已审核";
                }
                else
                {
                    zhuangtai = "未审核";
                }
                dingdanone.xuhao = n;
                dingdanone.UserName = obj.userModel.UserName;
                dingdanone.GpsDingDanDenno = obj.OrderDenno;
                dingdanone.GpsDingDanJinE = obj.GpsDingDanJinE;
                dingdanone.GpsDingDanShuLiang = obj.GpsDingDanShuLiang;
                dingdanone.GpsDingDanTime = obj.GpsDingDanTime;
                dingdanone.GpsDingDanRemark = obj.GpsDingDanRemark;
                dingdanone.GpsDingDanSH = zhuangtai;
                dingdanone.DGZZCompany = gdgmodel.DGZZCompany;
                dingdanone.DGZH = gdgmodel.DGZH;
                dingdanone.DKPZH = gdgmodel.DKPZH;
                dingdanModels.Add(dingdanone);

            }
            var rows = dingdanModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsDingDanDenno = p.GpsDingDanDenno,
                GpsDingDanTime = p.GpsDingDanTime.ToString(),
                GpsDingDanJinE = p.GpsDingDanJinE,
                GpsDingDanShuLiang = p.GpsDingDanShuLiang,
                GpsDingDanRemark = p.GpsDingDanRemark,
                GpsDingDanSH = p.GpsDingDanSH,
                DGZZCompany = p.DGZZCompany,
                DGZH = p.DGZH,
                DKPZH = p.DKPZH
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaleSHIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaleSHIndex(string GpsDingDanDenno, string UserName, DateTime? startDate, DateTime? endDate, string GpsDingDanSH, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {

            IEnumerable<GpsDingDanSale> dingdanModel = accountdb.GpsDingDanSale.Include("userModel");
            dingdanModel = dingdanModel.Where(p => p.GpsDingDanZhiFuLeiXing == "公对公");
            if (!string.IsNullOrEmpty(GpsDingDanDenno))
            {
                dingdanModel = dingdanModel.Where(p => p.OrderDenno == GpsDingDanDenno);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                dingdanModel = dingdanModel.Where(p => p.userModel.UserName == UserName);
            }
            if (GpsDingDanSH != "0")
            {
                bool shenhezhuangtai = true;

                if (GpsDingDanSH == "1") { shenhezhuangtai = true; }
                if (GpsDingDanSH == "2") { shenhezhuangtai = false; }

                dingdanModel = dingdanModel.Where(p => p.GpsDingDanSH == shenhezhuangtai);

            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                dingdanModel = dingdanModel.Where(p => p.GpsDingDanTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            dingdanModel = dingdanModel.OrderByDescending(p => p.GpsDingDanTime);
            var total = dingdanModel.Count();
            var currentPersonList = dingdanModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDingDanlist> dingdanModels = new List<GpsDingDanlist>();
            foreach (var obj in currentPersonList)
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsDingDanGDG gdgmodel = db.GpsDingDanGDG.Where(g => g.OrderDenno == obj.OrderDenno).First();

                n = n + 1;
                string zhuangtai = "";
                GpsDingDanlist dingdanone = new GpsDingDanlist();
                if (obj.GpsDingDanSH == true)
                {
                    zhuangtai = "已审核";
                }
                else
                {
                    zhuangtai = "未审核";
                }
                dingdanone.xuhao = n;
                dingdanone.UserName = obj.userModel.UserName;
                dingdanone.GpsDingDanDenno = obj.OrderDenno;
                dingdanone.GpsDingDanJinE = obj.GpsDingDanJinE;
                dingdanone.GpsDingDanShuLiang = obj.GpsDingDanShuLiang;
                dingdanone.GpsDingDanTime = obj.GpsDingDanTime;
                dingdanone.GpsDingDanRemark = obj.GpsDingDanRemark;
                dingdanone.GpsDingDanSH = zhuangtai;
                dingdanone.DGZZCompany = gdgmodel.DGZZCompany;
                dingdanone.DGZH = gdgmodel.DGZH;
                dingdanone.DKPZH = gdgmodel.DKPZH;
                dingdanModels.Add(dingdanone);

            }
            var rows = dingdanModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsDingDanDenno = p.GpsDingDanDenno,
                GpsDingDanTime = p.GpsDingDanTime.ToString(),
                GpsDingDanJinE = p.GpsDingDanJinE,
                GpsDingDanShuLiang = p.GpsDingDanShuLiang,
                GpsDingDanRemark = p.GpsDingDanRemark,
                GpsDingDanSH = p.GpsDingDanSH,
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
            string GpsDingDanDenno = HttpContext.Request["GpsDingDanDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsDingDan dingdanmodel = db.GpsDingDan.Where(g => g.OrderDenno == GpsDingDanDenno).First();
                if (dingdanmodel.GpsDingDanSH == true)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "此单据已审核！";
                }
                else
                {
                    dingdanmodel.GpsDingDanSH = true;
                    //dingdanmodel.GpsTuiDanShenHeShiJian = DateTime.Now;
                    db.SaveChanges();

                    IEnumerable<GpsDingDanMingXi> mx = accountdb.GpsDingDanMingXi.Where(x => x.GpsDingDanDenno == dingdanmodel.GpsDingDanDenno);

                    if (mx.Count() > 0)
                    {
                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db2 = new ChaHuoBaoModels();
                        foreach (var dr in mx)
                        {
                            Models.GpsDevice GpsDevice = new Models.GpsDevice();
                            GpsDevice.UserID = dingdanmodel.UserID;
                            GpsDevice.GpsDeviceID = dr.GpsDeviceID;
                            db2.GpsDevice.Add(GpsDevice);
                        }
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

        //审核
        //审核
        //审核
        [PermissionAuthorize]
        public string DataShenheSale()
        {
            string GpsDingDanDenno = HttpContext.Request["GpsDingDanDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsDingDanSale dingdanmodel = db.GpsDingDanSale.Where(g => g.OrderDenno == GpsDingDanDenno).First();
                if (dingdanmodel.GpsDingDanSH == true)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "此单据已审核！";
                }
                else
                {
                    dingdanmodel.GpsDingDanSH = true;
                    //dingdanmodel.GpsTuiDanShenHeShiJian = DateTime.Now;
                    db.SaveChanges();

                    IEnumerable<GpsDingDanSaleMingXi> mx = accountdb.GpsDingDanSaleMingXi.Where(x => x.GpsDingDanDenno == dingdanmodel.GpsDingDanDenno);

                    if (mx.Count() > 0)
                    {
                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db2 = new ChaHuoBaoModels();
                        foreach (var dr in mx)
                        {
                            Models.GpsDevice GpsDevice = new Models.GpsDevice();
                            GpsDevice.UserID = dingdanmodel.UserID;
                            GpsDevice.GpsDeviceID = dr.GpsDeviceID;
                            db2.GpsDevice.Add(GpsDevice);

                            Models.GpsDeviceSale GpsDeviceSale = new Models.GpsDeviceSale();
                            GpsDeviceSale.UserID = dingdanmodel.UserID;
                            GpsDeviceSale.GpsDeviceID = dr.GpsDeviceID;
                            db2.GpsDeviceSale.Add(GpsDeviceSale);
                        }
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
    }
}
