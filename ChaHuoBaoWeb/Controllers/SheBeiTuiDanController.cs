using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using Common;
using System.Collections;
using System.IO;
using ChaHuoBaoWeb.Filters;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    public class SheBeiTuiDanController : Controller
    {
        //设备退单表
        // GET: /SheBeiTuiDan/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string GpsTuiDanDenno,string GpsTuiDanTuiHuanZhuangTai, string GpsTuiDanShenHeZhuangTai,string UserName, string UserCity, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<GpsTuiDan> tuidanModel = accountdb.GpsTuiDan.Include("userModels");
            if (!string.IsNullOrEmpty(UserName))
            {
                tuidanModel = tuidanModel.Where(p => p.userModels.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(GpsTuiDanDenno))
            {
                tuidanModel = tuidanModel.Where(p => p.GpsTuiDanDenno == GpsTuiDanDenno);
            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                tuidanModel = tuidanModel.Where(p => p.userModels.UserCity.Contains(UserCity));
            }
            if (GpsTuiDanShenHeZhuangTai != "0")
            {
                bool shenhezhuangtai = true;

                if (GpsTuiDanShenHeZhuangTai == "1") { shenhezhuangtai = true; }
                if (GpsTuiDanShenHeZhuangTai == "2") { shenhezhuangtai = false; }

                tuidanModel = tuidanModel.Where(p => p.GpsTuiDanShenHeZhuangTai == shenhezhuangtai);


            }
            if (GpsTuiDanTuiHuanZhuangTai != "0")
            {
                bool dakuanzhuangtai = true;

                if (GpsTuiDanTuiHuanZhuangTai == "1") { dakuanzhuangtai = true; }
                if (GpsTuiDanTuiHuanZhuangTai == "2") { dakuanzhuangtai = false; }

                tuidanModel = tuidanModel.Where(p => p.GpsTuiDanTuiHuanZhuangTai == dakuanzhuangtai);


            }
            tuidanModel = tuidanModel.OrderByDescending(p => p.GpsTuiDanTime);
            var total = tuidanModel.Count();
            var currentPersonList = tuidanModel
                                          .Skip((pageIndex - 1) * pageSize)
                                          .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsTuiDanlist> tuidanModels = new List<GpsTuiDanlist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                string shenhezhuangtai = "";
                string tuihuanzhuantai = "";
                GpsTuiDanlist tuidanone = new GpsTuiDanlist();
                if (obj.GpsTuiDanShenHeZhuangTai == true)
                {
                    shenhezhuangtai = "已审核";
                }
                else
                {
                    shenhezhuangtai = "未审核";
                }

                if (obj.GpsTuiDanTuiHuanZhuangTai == true)
                {
                    tuihuanzhuantai = "已打款";
                }
                else
                {
                    tuihuanzhuantai = "未打款";
                }


                tuidanone.xuhao = n;
                tuidanone.GpsTuiDanRemark = obj.GpsTuiDanRemark;
                tuidanone.GpsTuiDanDenno = obj.GpsTuiDanDenno;
                tuidanone.GpsTuiDanJinE = obj.GpsTuiDanJinE;
                tuidanone.GpsTuiDanShenHeZhuangTai = shenhezhuangtai;
                tuidanone.GpsTuiDanShuLiang = obj.GpsTuiDanShuLiang;
                tuidanone.GpsTuiDanTime = obj.GpsTuiDanTime;
                tuidanone.GpsTuiDanZhangHao = obj.GpsTuiDanZhangHao;
                tuidanone.GpsTuiDanTuiHuanZhuangTai = tuihuanzhuantai;
                tuidanone.UserName = obj.userModels.UserName;
                tuidanModels.Add(tuidanone);
            }

            var rows = tuidanModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsTuiDanRemark = p.GpsTuiDanRemark,
                GpsTuiDanDenno = p.GpsTuiDanDenno,
                GpsTuiDanJinE = p.GpsTuiDanJinE,
                GpsTuiDanShenHeZhuangTai = p.GpsTuiDanShenHeZhuangTai,
                GpsTuiDanShuLiang = p.GpsTuiDanShuLiang,
                GpsTuiDanTime = p.GpsTuiDanTime.ToString(),
                GpsTuiDanZhangHao = p.GpsTuiDanZhangHao,
                GpsTuiDanTuiHuanZhuangTai = p.GpsTuiDanTuiHuanZhuangTai

            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        //审核
        //审核
        //审核
        [PermissionAuthorize]
        public string DataShenhe()
        {
            string GpsTuiDanDenno = HttpContext.Request["GpsTuiDanDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsTuiDan tuidanmodel = db.GpsTuiDan.Where(g => g.GpsTuiDanDenno == GpsTuiDanDenno).First();
                if (tuidanmodel.GpsTuiDanShenHeZhuangTai == true)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "此单据已审核！";
                }
                else
                {
                    tuidanmodel.GpsTuiDanShenHeZhuangTai = true;
                    tuidanmodel.GpsTuiDanShenHeShiJian = DateTime.Now;
                    db.SaveChanges();

                    IEnumerable<GpsTuiDanMingXi> mx = accountdb.GpsTuiDanMingXi.Where(x => x.GpsTuiDanDenno == tuidanmodel.GpsTuiDanDenno);

                    if (mx.Count() > 0)
                    {
                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db2 = new ChaHuoBaoModels();
                        foreach (var dr in mx)
                        {
                            IEnumerable<GpsDevice> device = db2.GpsDevice.Where(y => y.GpsDeviceID == dr.GpsDeviceID);
                            db2.GpsDevice.Remove(device.First());
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

        //打款
        //打款
        //打款
        [PermissionAuthorize]
        public string DataDakuan()
        {
            string GpsTuiDanDenno = HttpContext.Request["GpsTuiDanDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsTuiDan tuidanmodel = db.GpsTuiDan.Where(g => g.GpsTuiDanDenno == GpsTuiDanDenno).First();
                if (tuidanmodel.GpsTuiDanTuiHuanZhuangTai == true)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "此单据已打款！";
                }
                else
                {
                    tuidanmodel.GpsTuiDanTuiHuanZhuangTai = true;
                    tuidanmodel.GpsTuiDanTuiHuanShiJian = DateTime.Now;
                    db.SaveChanges();

                    hash["sign"] = "1";
                    hash["msg"] = "打款成功";
                }
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = ex.Message;
            }

            return JsonHelper.ToJson(hash);
        }


        //导出excel设备退单表内容
        //导出excel

        ////导出excel设备退单表内容
        public FileResult Export(string GpsTuiDanDenno, string GpsTuiDanTuiHuanZhuangTai, string GpsTuiDanShenHeZhuangTai,string UserName, string UserCity, string sortName, string sortOrder)
        {
            //YeWuGuanLi/GetSheBeiTuiDanViewList_toExcel
            //string tongfei=Request["tongfei"];
            //return Content(GpsTuiDanTuiHuanZhuangTai);

            //获取list数据
            IEnumerable<GpsTuiDan> tuidanModel = accountdb.GpsTuiDan.Include("userModels");
            if (!string.IsNullOrEmpty(UserName))
            {
                tuidanModel = tuidanModel.Where(p => p.userModels.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(GpsTuiDanDenno))
            {
                tuidanModel = tuidanModel.Where(p => p.GpsTuiDanDenno == GpsTuiDanDenno);
            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                tuidanModel = tuidanModel.Where(p => p.userModels.UserCity.Contains(UserCity));
            }
            if (GpsTuiDanShenHeZhuangTai != "0")
            {
                bool shenhezhuangtai = true;

                if (GpsTuiDanShenHeZhuangTai == "1") { shenhezhuangtai = true; }
                if (GpsTuiDanShenHeZhuangTai == "2") { shenhezhuangtai = false; }

                tuidanModel = tuidanModel.Where(p => p.GpsTuiDanShenHeZhuangTai == shenhezhuangtai);


            }
            if (GpsTuiDanTuiHuanZhuangTai != "0")
            {
                bool dakuanzhuangtai = true;

                if (GpsTuiDanTuiHuanZhuangTai == "1") { dakuanzhuangtai = true; }
                if (GpsTuiDanTuiHuanZhuangTai == "2") { dakuanzhuangtai = false; }

                tuidanModel = tuidanModel.Where(p => p.GpsTuiDanTuiHuanZhuangTai == dakuanzhuangtai);


            }
            tuidanModel = tuidanModel.OrderByDescending(p => p.GpsTuiDanTime);

            List<GpsTuiDanlist> tuidanModels = new List<GpsTuiDanlist>();
            foreach (var obj in tuidanModel)
            {

                string shenhezhuangtai = "";
                string tuihuanzhuantai = "";
                GpsTuiDanlist tuidanone = new GpsTuiDanlist();
                if (obj.GpsTuiDanShenHeZhuangTai == true)
                {
                    shenhezhuangtai = "已审核";
                }
                else
                {
                    shenhezhuangtai = "未审核";
                }

                if (obj.GpsTuiDanTuiHuanZhuangTai == true)
                {
                    tuihuanzhuantai = "已打款";
                }
                else
                {
                    tuihuanzhuantai = "未打款";
                }

                tuidanone.GpsTuiDanRemark = obj.GpsTuiDanRemark;
                tuidanone.GpsTuiDanDenno = obj.GpsTuiDanDenno;
                tuidanone.GpsTuiDanJinE = obj.GpsTuiDanJinE;
                tuidanone.GpsTuiDanShenHeZhuangTai = shenhezhuangtai;
                tuidanone.GpsTuiDanShuLiang = obj.GpsTuiDanShuLiang;
                tuidanone.GpsTuiDanTime = obj.GpsTuiDanTime;
                tuidanone.GpsTuiDanZhangHao = obj.GpsTuiDanZhangHao;
                tuidanone.GpsTuiDanTuiHuanZhuangTai = tuihuanzhuantai;
                tuidanone.UserName = obj.userModels.UserName;
                tuidanModels.Add(tuidanone);
            }

            var list = tuidanModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("退单编号");
            row1.CreateCell(2).SetCellValue("退单用户");
            row1.CreateCell(3).SetCellValue("退单数量");
            row1.CreateCell(4).SetCellValue("退还金额");
            row1.CreateCell(5).SetCellValue("退还日期");
            row1.CreateCell(6).SetCellValue("退还账号");
            row1.CreateCell(7).SetCellValue("审核状态");
            row1.CreateCell(8).SetCellValue("打款状态");
            row1.CreateCell(9).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].GpsTuiDanDenno);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(3).SetCellValue(list[i].GpsTuiDanShuLiang);
                rowtemp.CreateCell(4).SetCellValue(list[i].GpsTuiDanJinE.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].GpsTuiDanTime.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].GpsTuiDanZhangHao);
                rowtemp.CreateCell(7).SetCellValue(list[i].GpsTuiDanShenHeZhuangTai);
                rowtemp.CreateCell(8).SetCellValue(list[i].GpsTuiDanTuiHuanZhuangTai);
                rowtemp.CreateCell(9).SetCellValue(list[i].GpsTuiDanRemark);
                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "设备退单.xls");

        }


        public class GpsTuiDanlist
        {
            public int xuhao { get; set; }

            public string GpsTuiDanDenno { get; set; }

            public string UserName { get; set; }

            public int GpsTuiDanShuLiang { get; set; }

            public decimal GpsTuiDanJinE { get; set; }

            public string GpsTuiDanZhangHao { get; set; }

            public DateTime GpsTuiDanTime { get; set; }

            public string GpsTuiDanShenHeZhuangTai { get; set; }

            public string GpsTuiDanTuiHuanZhuangTai { get; set; }

            public string GpsTuiDanRemark { get; set; }
        }
    }
}
