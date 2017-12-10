using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using Common;
using System.IO;
using ChaHuoBaoWeb.Filters;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class YongHuGuanLiController : Controller
    {
        //
        // GET: /YongHuGuanLi/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        //用户管理首页
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string UserName, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
            IEnumerable<User> yonghuModel = accountdb.User.Where(x => x.UserLeiXing == "WEB"); ; ;
            if (!string.IsNullOrEmpty(UserName))
            {
                yonghuModel = yonghuModel.Where(P => P.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                yonghuModel = yonghuModel.Where(p => p.UserCreateTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                yonghuModel = yonghuModel.Where(p => p.UserCreateTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            yonghuModel = yonghuModel.OrderByDescending(p => p.UserCreateTime);
            var total = yonghuModel.Count();
            var currentPersonList = yonghuModel
                                            .Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<Userlist> yonghuModels = new List<Userlist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                Userlist yonghuone = new Userlist();
                yonghuone.xuhao = n;
                yonghuone.UserName = obj.UserName;
                yonghuone.UserPassword = obj.UserPassword;
                yonghuone.UserCreateTime = obj.UserCreateTime;
                yonghuone.UserRemark = obj.UserRemark;
                yonghuone.UserID = obj.UserID;
                yonghuModels.Add(yonghuone);
            }
            var rows = yonghuModels.Select(p => new
            {
                id = p.xuhao,
                UserName = p.UserName,
                UserPassword = p.UserPassword,
                UserCreateTime = p.UserCreateTime.ToString(),
                UserRemark = p.UserRemark,
                UserID = p.UserID

            });
            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }
        //用户管理新增
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(User xinzengyonghu)
        {
            string msg = "";
            //非必填项设置默认值
            xinzengyonghu.UserCreateTime = DateTime.Now;
            xinzengyonghu.UserIsLimit = false;
            xinzengyonghu.UserID = Guid.NewGuid().ToString();
            xinzengyonghu.UserLeiXing = "WEB";
            xinzengyonghu.UserRemainder = 0;
            xinzengyonghu.UserCity = "江苏省 常州市";
            xinzengyonghu.UserEndTime = DateTime.Now.AddYears(100);
            xinzengyonghu.UserNameDescribe = "WEB新增用户";
            //验证参数
            if (string.IsNullOrEmpty(xinzengyonghu.UserName))
            {
                msg = "用户名不能为空";
                ViewData["msg"] = msg;
                return View(new User());
            }
            if (!RegexHelper.IsMatch(xinzengyonghu.UserName, @"^[0-9]{11}$"))
            {
                msg = "用户名应为11位有效手机号码";
                ViewData["msg"] = msg;
                return View(new User());
            }
            if (string.IsNullOrEmpty(xinzengyonghu.UserPassword))
            {
                msg = "密码不能为空";
                ViewData["msg"] = msg;
                return View(new User());
            }
            IEnumerable<User> yonghu = accountdb.User.Where(x => x.UserName == xinzengyonghu.UserName && x.UserLeiXing == "WEB");
            if (yonghu.Count() == 0)
            {
                accountdb.User.Add(xinzengyonghu);
                accountdb.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                msg = "该用户已存在，无法重复新增！";
            }
            ViewData["msg"] = msg;
            return View(new User());
        }
        //用户管理删除
        [PermissionAuthorize]
        public ActionResult Delete(string UserID)
        {
            IEnumerable<User> user = accountdb.User.Where(x => x.UserID == UserID);
            accountdb.User.Remove(user.First());
            accountdb.SaveChanges();
            return Json(new { state = true, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
        }
        //用户管理修改
        [PermissionAuthorize] 
        public ActionResult Edit(string UserID)
        {
            User user = accountdb.User.Include("User_Roles").Where(g => g.UserID == UserID).First();
            UserEditViewModel userview = new UserEditViewModel { user = user, allroles = new List<checklst>(), selectedroles = new List<checklst>() };
            foreach (Role rl in accountdb.Role)
            {
                checklst cl = new checklst();
                cl.id = rl.RoleID;
                cl.name = rl.RoleName;
                if (user.User_Roles.Where(g => g.RoleID == rl.RoleID).Count() > 0)
                {
                    cl.isselected = true;
                    checklst cl1 = new checklst();
                    cl1.id = rl.RoleID;
                    cl1.name = rl.RoleName;
                    cl1.isselected = true;
                    userview.selectedroles.Add(cl1);
                }
                else
                {
                    cl.isselected = false;
                }
                userview.allroles.Add(cl);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(userview);
        }

        [HttpPost]
        public ActionResult Edit(UserEditViewModel userview)
        {
            if (ModelState.IsValid)
            {
                User user = accountdb.User.Include("User_Roles").Where(g => g.UserID == userview.user.UserID).First();
                user.UserRemark = userview.user.UserRemark;
                ///菜单权限
                foreach (Role rl in accountdb.Role)
                {
                    if (userview.postroleids != null)
                    {
                        if (userview.postroleids.Ids.Contains(rl.RoleID))
                        {
                            if (user.User_Roles.Where(g => g.RoleID == rl.RoleID).Count() == 0)
                            {
                                //不包含，需要添加
                                User_Role uir = new User_Role();
                                uir.UserID = user.UserID;
                                uir.RoleID = rl.RoleID;
                                accountdb.User_Role.Add(uir);
                            }
                        }
                        else
                        {
                            IEnumerable<User_Role> uir = user.User_Roles.Where(g => g.RoleID == rl.RoleID);
                            if (uir.Count() > 0)
                            {
                                //不包含，需要添加
                                accountdb.User_Role.Remove(uir.First());
                            }
                        }
                    }
                }
                accountdb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userview);
        }

        //用户管理导出
        public FileResult Export(string UserName, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder)
        {

            //获取list数据
            IEnumerable<User> yonghuModel = accountdb.User.Where(x=>x.UserLeiXing=="WEB");
            if (!string.IsNullOrEmpty(UserName))
            {
                yonghuModel = yonghuModel.Where(P => P.UserName == UserName);
            }
            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                yonghuModel = yonghuModel.Where(p => p.UserCreateTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                yonghuModel = yonghuModel.Where(p => p.UserCreateTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            yonghuModel = yonghuModel.OrderByDescending(p => p.UserCreateTime);
            List<Userlist> yonghuModels = new List<Userlist>();
            foreach (var obj in yonghuModel)
            {

                Userlist yonghuone = new Userlist();

                yonghuone.UserName = obj.UserName;
                yonghuone.UserPassword = obj.UserPassword;
                yonghuone.UserCreateTime = obj.UserCreateTime;
                yonghuone.UserRemark = obj.UserRemark;

                yonghuModels.Add(yonghuone);
            }

            var list = yonghuModels;
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("用户名");
            row1.CreateCell(2).SetCellValue("密码");
            row1.CreateCell(3).SetCellValue("创建时间");
            row1.CreateCell(4).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            int z = 1;
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(z);
                rowtemp.CreateCell(1).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(2).SetCellValue(list[i].UserPassword);
                rowtemp.CreateCell(3).SetCellValue(list[i].UserCreateTime.ToString());
                rowtemp.CreateCell(4).SetCellValue(list[i].UserRemark);

                z = z + 1;
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "用户管理.xls");

        }
        public class Userlist
        {
            public int xuhao { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public DateTime UserCreateTime { get; set; }
            public string UserRemark { get; set; }
        }
    }
}
