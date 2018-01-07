using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using System.IO;
using ChaHuoBaoWeb.Filters;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class CaoZuoJiLuController : Controller
    {
        //操作记录首页
        // GET: /CaoZuoJiLu/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string UserName, string UserCity, string YunDanBianHao, string CaoZuoLeiXing, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<CaoZuoJiLu> yewuModel = accountdb.CaoZuoJiLu.Include("userModelt");

            if (!string.IsNullOrEmpty(UserName))
            {
                yewuModel = yewuModel.Where(P => P.userModelt.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                yewuModel = yewuModel.Where(P => P.userModelt.UserCity.Contains(UserCity));
            }
            if (!string.IsNullOrEmpty(CaoZuoLeiXing))
            {
                yewuModel = yewuModel.Where(P => P.CaoZuoLeiXing.Contains(CaoZuoLeiXing));
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                yewuModel = yewuModel.Where(x => x.CaoZuoTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                yewuModel = yewuModel.Where(x => x.CaoZuoTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            yewuModel = yewuModel.OrderByDescending(p => p.CaoZuoTime);
            var total = yewuModel.Count();

            var currentPersonList = yewuModel
                                            .Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<CaoZuoJiLulist> yewuModels = new List<CaoZuoJiLulist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                CaoZuoJiLulist yewuone = new CaoZuoJiLulist();
                yewuone.xuhao = n;
                yewuone.UserName = obj.userModelt.UserName;
                yewuone.UserCity = obj.userModelt.UserCity;
                yewuone.CaoZuoLeiXing = obj.CaoZuoLeiXing;
                yewuone.CaoZuoRemark = obj.CaoZuoRemark;
                yewuone.CaoZuoTime = obj.CaoZuoTime;
                yewuone.CaoZuoNeiRong = obj.CaoZuoNeiRong;
                yewuModels.Add(yewuone);

            }
            var rows = yewuModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                UserCity = p.UserCity,
                CaoZuoLeiXing = p.CaoZuoLeiXing,
                CaoZuoRemark = p.CaoZuoRemark,
                CaoZuoTime = p.CaoZuoTime.ToString(),
                CaoZuoNeiRong = p.CaoZuoNeiRong

            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }


        //////导出excel操作记录表内容
        //////导出excel操作记录表内容
        [PermissionAuthorize]
        public FileResult Export(string UserName, string UserCity, string YunDanBianHao, string CaoZuoLeiXing, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder)
        {

            //获取list数据
            IEnumerable<CaoZuoJiLu> yewuModel = accountdb.CaoZuoJiLu.Include("userModelt");

            if (!string.IsNullOrEmpty(UserName))
            {
                yewuModel = yewuModel.Where(P => P.userModelt.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                yewuModel = yewuModel.Where(P => P.userModelt.UserCity.Contains(UserCity));
            }
            if (!string.IsNullOrEmpty(CaoZuoLeiXing))
            {
                yewuModel = yewuModel.Where(P => P.CaoZuoLeiXing.Contains(CaoZuoLeiXing));
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                yewuModel = yewuModel.Where(x => x.CaoZuoTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                yewuModel = yewuModel.Where(x => x.CaoZuoTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            yewuModel = yewuModel.OrderByDescending(p => p.CaoZuoTime);
            List<CaoZuoJiLulist> yewuModels = new List<CaoZuoJiLulist>();
            foreach (var obj in yewuModel)
            {

                CaoZuoJiLulist yewuone = new CaoZuoJiLulist();

                yewuone.UserName = obj.userModelt.UserName;
                yewuone.UserCity = obj.userModelt.UserCity;
                yewuone.CaoZuoLeiXing = obj.CaoZuoLeiXing;
                yewuone.CaoZuoRemark = obj.CaoZuoRemark;
                yewuone.CaoZuoTime = obj.CaoZuoTime;
                yewuone.CaoZuoNeiRong = obj.CaoZuoNeiRong;
                yewuModels.Add(yewuone);

            }

            var list = yewuModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("用户名");
            row1.CreateCell(2).SetCellValue("所在区域");
            row1.CreateCell(3).SetCellValue("操作类型");
            row1.CreateCell(4).SetCellValue("操作时间");
            row1.CreateCell(5).SetCellValue("操作内容");
            row1.CreateCell(6).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserCity);
                rowtemp.CreateCell(3).SetCellValue(list[i].CaoZuoLeiXing);
                rowtemp.CreateCell(4).SetCellValue(list[i].CaoZuoTime.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].CaoZuoNeiRong);
                rowtemp.CreateCell(5).SetCellValue(list[i].CaoZuoRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "操作记录.xls");

        }

        public class CaoZuoJiLulist
        {
            public int xuhao { get; set; }

            public string UserName { get; set; }

            public string UserCity { get; set; }

            public string CaoZuoNeiRong { get; set; }

            public string CaoZuoLeiXing { get; set; }

            public DateTime CaoZuoTime { get; set; }

            public string CaoZuoRemark { get; set; }
        }
    }
}
