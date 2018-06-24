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
    public class SheBeiGuanLiController : Controller
    {
        //设备管理首页
        // GET: /SheBeiGuanLi/

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
        public ActionResult Index(string GpsDeviceID, string UserName, string UserCity,string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<GpsDevice> shebeiModel = accountdb.GpsDevice.Include("usermodel");
            if (!string.IsNullOrEmpty(GpsDeviceID))
            {
                shebeiModel = shebeiModel.Where(p => p.GpsDeviceID == GpsDeviceID);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserName == UserName);

            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserCity.Contains(UserCity));

            }
          
            var total = shebeiModel.Count();
            var currentPersonList = shebeiModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDevicelist> shebeiModels = new List<GpsDevicelist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                GpsDevicelist shebeione = new GpsDevicelist();
                shebeione.xuhao = n;
                shebeione.GpsDeviceID = obj.GpsDeviceID;
                shebeione.GpsDeviceRemark = obj.GpsDeviceRemark;
                shebeione.UserName = obj.usermodel.UserName;
                shebeione.UserCity = obj.usermodel.UserCity;
             
                shebeiModels.Add(shebeione);
            }
            var rows = shebeiModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsDeviceID = p.GpsDeviceID,
                GpsDeviceRemark = p.GpsDeviceRemark,
                UserCity = p.UserCity,
                IsBangding = p.IsBangding

            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult SaleIndex(string GpsDeviceID, string UserName, string UserCity, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<GpsDeviceSale> shebeiModel = accountdb.GpsDeviceSale.Include("usermodel");
            if (!string.IsNullOrEmpty(GpsDeviceID))
            {
                shebeiModel = shebeiModel.Where(p => p.GpsDeviceID == GpsDeviceID);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserName == UserName);

            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserCity.Contains(UserCity));

            }

            var total = shebeiModel.Count();
            var currentPersonList = shebeiModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDevicelist> shebeiModels = new List<GpsDevicelist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                GpsDevicelist shebeione = new GpsDevicelist();
                shebeione.xuhao = n;
                shebeione.GpsDeviceID = obj.GpsDeviceID;
                shebeione.GpsDeviceRemark = obj.GpsDeviceRemark;
                shebeione.UserName = obj.usermodel.UserName;
                shebeione.UserCity = obj.usermodel.UserCity;

                shebeiModels.Add(shebeione);
            }
            var rows = shebeiModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                GpsDeviceID = p.GpsDeviceID,
                GpsDeviceRemark = p.GpsDeviceRemark,
                UserCity = p.UserCity,
                IsBangding = p.IsBangding

            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        [PermissionAuthorize]
        public string JieChu(string GpsDeviceID)
        {
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "解绑失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.GpsDevice dingdanmodel = db.GpsDevice.Where(g => g.GpsDeviceID == GpsDeviceID).First();
                ChaHuoBaoWeb.Models.GpsDeviceSale dingdansalemodel = db.GpsDeviceSale.Where(g => g.GpsDeviceID == GpsDeviceID).First();
                db.GpsDevice.Remove(dingdanmodel);
                db.SaveChanges();
                db.GpsDeviceSale.Remove(dingdansalemodel);
                db.SaveChanges();

                hash["sign"] = "1";
                hash["msg"] = "解绑成功";
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = ex.Message;
            }

            return JsonHelper.ToJson(hash);
        }

        public class GpsDevicelist
        {
            public int xuhao { get; set; }

            public string GpsDeviceID { get; set; }

            public string GpsDeviceRemark { get; set; }

            public string UserName { get; set; }

            public string UserCity { get; set; }

            public string IsBangding { get; set; }

        }


        //导出我的设备表内容
        //导出excel
        [PermissionAuthorize]
        public FileResult Export(string GpsDeviceID, string UserName, string UserCity,string sortName, string sortOrder)
        {
            //获取list数据
            IEnumerable<GpsDevice> shebeiModel = accountdb.GpsDevice.Include("usermodel");
            if (!string.IsNullOrEmpty(GpsDeviceID))
            {
                shebeiModel = shebeiModel.Where(p => p.GpsDeviceID == GpsDeviceID);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserName == UserName);

            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserCity.Contains(UserCity));

            }

            var total = shebeiModel.Count();
       
            List<GpsDevicelist> shebeiModels = new List<GpsDevicelist>();
            foreach (var obj in shebeiModel)
            {
                
                GpsDevicelist shebeione = new GpsDevicelist();
                
                shebeione.GpsDeviceID = obj.GpsDeviceID;
                shebeione.GpsDeviceRemark = obj.GpsDeviceRemark;
                shebeione.UserName = obj.usermodel.UserName;
                shebeione.UserCity = obj.usermodel.UserCity;

                shebeiModels.Add(shebeione);
            }
            var list = shebeiModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("设备标识");
            row1.CreateCell(2).SetCellValue("绑定用户");
            row1.CreateCell(3).SetCellValue("所属区域"); 
            row1.CreateCell(4).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].GpsDeviceID);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(3).SetCellValue(list[i].UserCity);;
                rowtemp.CreateCell(4).SetCellValue(list[i].GpsDeviceRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "设备管理.xls");
        }

        //导出我的设备表内容
        //导出excel
        [PermissionAuthorize]
        public FileResult ExportSale(string GpsDeviceID, string UserName, string UserCity, string sortName, string sortOrder)
        {
            //获取list数据
            IEnumerable<GpsDeviceSale> shebeiModel = accountdb.GpsDeviceSale.Include("usermodel");
            if (!string.IsNullOrEmpty(GpsDeviceID))
            {
                shebeiModel = shebeiModel.Where(p => p.GpsDeviceID == GpsDeviceID);

            }
            if (!string.IsNullOrEmpty(UserName))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserName == UserName);

            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                shebeiModel = shebeiModel.Where(p => p.usermodel.UserCity.Contains(UserCity));

            }

            var total = shebeiModel.Count();

            List<GpsDevicelist> shebeiModels = new List<GpsDevicelist>();
            foreach (var obj in shebeiModel)
            {

                GpsDevicelist shebeione = new GpsDevicelist();

                shebeione.GpsDeviceID = obj.GpsDeviceID;
                shebeione.GpsDeviceRemark = obj.GpsDeviceRemark;
                shebeione.UserName = obj.usermodel.UserName;
                shebeione.UserCity = obj.usermodel.UserCity;

                shebeiModels.Add(shebeione);
            }
            var list = shebeiModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("设备标识");
            row1.CreateCell(2).SetCellValue("绑定用户");
            row1.CreateCell(3).SetCellValue("所属区域");
            row1.CreateCell(4).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].GpsDeviceID);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(3).SetCellValue(list[i].UserCity); ;
                rowtemp.CreateCell(4).SetCellValue(list[i].GpsDeviceRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "设备管理.xls");
        }

        [PermissionAuthorize]
        public string DataZhiHuan()
        {
            string GpsDeviceID = HttpContext.Request["GpsDeviceID"];
            string GpsDeviceNewID = HttpContext.Request["GpsDeviceNewID"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "设备置换失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<GpsDevice> devicelist = db.GpsDevice.Where(g => g.GpsDeviceID == GpsDeviceID);
                if (devicelist.Count() == 0)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "未找到该置换设备！";
                }
                else
                {
                    ChaHuoBaoWeb.Models.ChaHuoBaoModels db3 = new ChaHuoBaoModels();
                    IEnumerable<GpsDevice> devicenewlist = db3.GpsDevice.Where(g => g.GpsDeviceID == GpsDeviceNewID);
                    if (devicenewlist.Count() == 0)
                    {
                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db2 = new ChaHuoBaoModels();
                        ChaHuoBaoWeb.Models.GpsDevice device = db2.GpsDevice.Where(x => x.GpsDeviceID == GpsDeviceID).First();
                        string ChangeGpsDeviceID = GpsDeviceNewID;
                        string UserID = device.UserID;

                        db2.GpsDevice.Remove(device);
                        db2.SaveChanges();

                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db4 = new ChaHuoBaoModels();
                        GpsDevice gpsnew = new GpsDevice();
                        gpsnew.GpsDeviceID = ChangeGpsDeviceID;
                        gpsnew.UserID = UserID;
                        gpsnew.GpsDeviceRemark = null;
                        db4.GpsDevice.Add(gpsnew);
                        db4.SaveChanges();

                        hash["sign"] = "1";
                        hash["msg"] = "置换成功";
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "新设备号已和用户绑定，不可重复绑定！";
                    }
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
