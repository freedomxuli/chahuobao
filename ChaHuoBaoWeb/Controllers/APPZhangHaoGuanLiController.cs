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
    public class APPZhangHaoGuanLiController : Controller
    {
        //
        // GET: /APPZhangHaoGuanLi/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string UserName, string UserCity, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<User> zhanghaoModel = accountdb.User.Where(x => x.UserLeiXing == "APP");

            if (!string.IsNullOrEmpty(UserName))
            {
                zhanghaoModel = zhanghaoModel.Where(P => P.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                zhanghaoModel = zhanghaoModel.Where(P => P.UserCity.Contains(UserCity));
            }
            var total = zhanghaoModel.Count();

            if (pageSize == 0)
            {
                pageSize = total;
            }


            var currentPersonList = zhanghaoModel
                                            .Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;



            List<UserList> zhanghaoModels = new List<UserList>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                UserList zhanghaoone = new UserList();
                zhanghaoone.xuhao = n;
                zhanghaoone.UserID = obj.UserID;
                zhanghaoone.UserName = obj.UserName;
                zhanghaoone.UserPassword = obj.UserPassword;
                zhanghaoone.UserCity = obj.UserCity;
                zhanghaoone.UserRemainder = obj.UserRemainder;
                zhanghaoone.UserRemark = obj.UserRemark;
                zhanghaoone.UserCreateTime = obj.UserCreateTime;
                zhanghaoone.UserWxEnable = obj.UserWxEnable;
                int zuyongshebei = 0;
                int zaiyongshebei = 0;
                ChaHuoBaoModels chbdb = new ChaHuoBaoModels();
                zuyongshebei = chbdb.GpsDevice.Where(x => x.UserID == obj.UserID).Count();
                zaiyongshebei = chbdb.YunDan.Where(x => x.UserID == obj.UserID && x.IsBangding == true).Count();
                zhanghaoone.zuyongshebei = zuyongshebei;
                zhanghaoone.zaiyongshebei = zaiyongshebei;
                zhanghaoModels.Add(zhanghaoone);

            }
            var rows = zhanghaoModels.Select(p => new
            {
                id = p.xuhao,
                UserID = p.UserID,
                UserName = p.UserName,
                UserPassword = p.UserPassword,
                UserCity = p.UserCity,
                UserRemainder = p.UserRemainder,
                UserWxEnable=p.UserWxEnable,
                zuyongshebei = p.zuyongshebei,
                zaiyongshebei = p.zaiyongshebei,

                UserRemark = p.UserRemark,
                UserCreateTime = p.UserCreateTime.ToString()

            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }


        ///导出excel账号管理表表内容
        //////导出excel账号管理表表内容
        [PermissionAuthorize]
        public FileResult Export(string UserName, string UserCity, string sortName, string sortOrder)
        {

            //获取list数据
            IEnumerable<User> zhanghaoModel = accountdb.User.Where(x => x.UserLeiXing == "APP");

            if (!string.IsNullOrEmpty(UserName))
            {
                zhanghaoModel = zhanghaoModel.Where(P => P.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(UserCity))
            {
                zhanghaoModel = zhanghaoModel.Where(P => P.UserCity.Contains(UserCity));
            }

            List<UserList> zhanghaoModels = new List<UserList>();
            foreach (var obj in zhanghaoModel)
            {

                UserList zhanghaoone = new UserList();

                zhanghaoone.UserName = obj.UserName;
                zhanghaoone.UserPassword = obj.UserPassword;
                zhanghaoone.UserCity = obj.UserCity;
                zhanghaoone.UserRemainder = obj.UserRemainder;
                zhanghaoone.UserRemark = obj.UserRemark;
                zhanghaoone.UserCreateTime = obj.UserCreateTime;

                int zuyongshebei = 0;
                int zaiyongshebei = 0;

                ChaHuoBaoModels chbdb = new ChaHuoBaoModels();
                zuyongshebei = chbdb.GpsDevice.Where(x => x.UserID == obj.UserID).Count();
                zaiyongshebei = chbdb.YunDan.Where(x => x.UserID == obj.UserID && x.IsBangding == true).Count();
                zhanghaoone.zuyongshebei = zuyongshebei;
                zhanghaoone.zaiyongshebei = zaiyongshebei;

                zhanghaoModels.Add(zhanghaoone);

            }

            var list = zhanghaoModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("用户名");
            row1.CreateCell(2).SetCellValue("密码");
            row1.CreateCell(3).SetCellValue("所属区域");
            row1.CreateCell(4).SetCellValue("创建时间");
            row1.CreateCell(5).SetCellValue("余额（剩余次数）");

            row1.CreateCell(6).SetCellValue("租用设备");
            row1.CreateCell(7).SetCellValue("在用设备");

            row1.CreateCell(8).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserPassword);
                rowtemp.CreateCell(3).SetCellValue(list[i].UserCity);
                rowtemp.CreateCell(4).SetCellValue(list[i].UserCreateTime.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].UserRemainder);

                rowtemp.CreateCell(6).SetCellValue(list[i].zuyongshebei);
                rowtemp.CreateCell(7).SetCellValue(list[i].zaiyongshebei);

                rowtemp.CreateCell(8).SetCellValue(list[i].UserRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "账号管理.xls");

        }


        //用户管理充值
        public ActionResult ChongZhi(string UserID)
        {
            User userone = accountdb.User.Where(x => x.UserLeiXing == "APP" && x.UserID == UserID).First();
            return View(userone);
        }

        [HttpPost]
        public ActionResult ChongZhi(string UserID, string UserName, string UserRemainder, string chongzhicishu, string beizhu)
        {
            User userone = accountdb.User.Where(x => x.UserLeiXing == "APP" && x.UserID == UserID).First();
            if (!string.IsNullOrEmpty(chongzhicishu))
            {
                //if (Convert.ToInt32(chongzhicishu) < 0)
                //{
                //    ViewData["msg"] = "充值次数不得小于0！";
                //}
                //else
                //{
                //    userone.UserRemainder = userone.UserRemainder + Convert.ToInt32(chongzhicishu);

                //    //添加 操作记录
                //    CaoZuoJiLu CaoZuoJiLu = new Models.CaoZuoJiLu();
                //    CaoZuoJiLu.UserID = UserID;
                //    CaoZuoJiLu.CaoZuoLeiXing = "充值";
                //    CaoZuoJiLu.CaoZuoNeiRong = "WEB内用户充值，充值方式：手动操作；充值人：" + HttpContext.User.Identity.Name + "；充值次数：" + chongzhicishu + "；充值备注：" + beizhu + "。";
                //    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                //    CaoZuoJiLu.CaoZuoRemark = "";
                //    accountdb.CaoZuoJiLu.Add(CaoZuoJiLu);

                //    accountdb.SaveChanges();
                //}
                userone.UserRemainder = userone.UserRemainder + Convert.ToInt32(chongzhicishu);

                //添加 操作记录
                CaoZuoJiLu CaoZuoJiLu = new Models.CaoZuoJiLu();
                CaoZuoJiLu.UserID = UserID;
                CaoZuoJiLu.CaoZuoLeiXing = "划拨";
                CaoZuoJiLu.CaoZuoNeiRong = "WEB内用户划拨，划拨方式：手动操作；划拨人：" + HttpContext.User.Identity.Name + "；划拨次数：" + chongzhicishu + "；划拨备注：" + beizhu + "。";
                CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                CaoZuoJiLu.CaoZuoRemark = "";
                accountdb.CaoZuoJiLu.Add(CaoZuoJiLu);
                accountdb.SaveChanges();
            }
            else
            {
                ViewData["msg"] = "划拨次数不可为空！";
            }
            return View(userone);
        }

        //用户管理修改
        public ActionResult Edit(string UserID)
        {
            User userone = accountdb.User.Where(x => x.UserLeiXing == "APP" && x.UserID == UserID).First();
            return View(userone);
        }
        [HttpPost]
        public ActionResult Edit(string UserID,string beizhu,User user_edit)
        {
            User userone = accountdb.User.Where(x => x.UserLeiXing == "APP" && x.UserID == UserID).First();

            userone.UserWxEnable = user_edit.UserWxEnable;

            userone.UserRemark = beizhu;

            ViewData["UserWxEnable"] = user_edit.UserWxEnable;
            //添加 操作记录
            CaoZuoJiLu CaoZuoJiLu = new Models.CaoZuoJiLu();
            CaoZuoJiLu.UserID = UserID;
            CaoZuoJiLu.CaoZuoLeiXing = "修改";
            CaoZuoJiLu.CaoZuoNeiRong = "WEB内APP用户修改，修改字段：微信查询；修改人：" + HttpContext.User.Identity.Name + "；修改备注：" + beizhu + "。";
            CaoZuoJiLu.CaoZuoTime = DateTime.Now;
            CaoZuoJiLu.CaoZuoRemark = "";
            accountdb.CaoZuoJiLu.Add(CaoZuoJiLu);
            accountdb.SaveChanges();

            return View(userone);
        }



        public class UserList
        {
            public int xuhao { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }

            public string UserPassword { get; set; }

            public string UserCity { get; set; }

            public int UserRemainder { get; set; }

            public int zuyongshebei { get; set; }
            public int zaiyongshebei { get; set; }
            public bool UserWxEnable { get; set; }
            public string UserRemark { get; set; }

            public DateTime UserCreateTime { get; set; }
        }

        [PermissionAuthorize]
        public string LookPaw()
        {
            //string UserPassword = HttpContext.Request["UserPassword"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "查看失败！";
            try
            {
                hash["sign"] = "1";
                hash["msg"] = "查看成功";
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = ex.Message;
            }

            return JsonHelper.ToJson(hash);
        }

        [PermissionAuthorize]
        public string ChongZhiQX()
        {
            //string UserPassword = HttpContext.Request["UserPassword"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "查看失败！";
            try
            {
                hash["sign"] = "1";
                hash["msg"] = "查看成功";
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
