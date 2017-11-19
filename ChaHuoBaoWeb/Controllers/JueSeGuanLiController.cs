using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using Common;
using ChaHuoBaoWeb.Filters;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class JueSeGuanLiController : Controller
    {
        //
        // GET: /JueSeGuanLi/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        //用户管理首页
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<Role> RoleModel = accountdb.Role;
            var total = RoleModel.Count();
            var currentPersonList = RoleModel
                                            .Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<Rolelist> RoleModels = new List<Rolelist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                Rolelist role_new = new Rolelist();
                role_new.xuhao = n;
                role_new.RoleID = obj.RoleID;
                role_new.RoleGroup = obj.RoleGroup;
                role_new.RoleName = obj.RoleName;
                role_new.RoleRemark = obj.RoleRemark;
                RoleModels.Add(role_new);
            }
            var rows = RoleModels.Select(p => new
            {
                id = p.xuhao,
                RoleID = p.RoleID,
                RoleGroup = p.RoleGroup,
                RoleName = p.RoleName,
                RoleRemark = p.RoleRemark,
            });
            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        //角色管理新增
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Role rolexinzneg)
        {
            string msg = "";
            //非必填项设置默认值
            rolexinzneg.RoleID = Guid.NewGuid().ToString();
            rolexinzneg.RoleGroup = "WEB";
            //验证参数
            if (string.IsNullOrEmpty(rolexinzneg.RoleName))
            {
                msg = "角色名称不能为空";
                ViewData["msg"] = msg;
                return View(new Role());
            }
            IEnumerable<Role> Role = accountdb.Role.Where(x => x.RoleName == rolexinzneg.RoleName);
            if (Role.Count() == 0)
            {
                accountdb.Role.Add(rolexinzneg);
                accountdb.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                msg = "该角色已存在，无法重复新增！";
            }
            ViewData["msg"] = msg;
            return View(new Role());
        }

        //角色管理删除
        public ActionResult Delete(string RoleID)
        {
            IEnumerable<Role> Role = accountdb.Role.Where(x => x.RoleID == RoleID);
            accountdb.Role.Remove(Role.First());
            accountdb.SaveChanges();
            return Json(new { state = true, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
        }
        //角色管理修改
        public ActionResult Edit(string RoleID)
        {
            Role role = accountdb.Role.Include("Role_QuanXians").Include("Role_QuanXians.QuanXian").Where(g => g.RoleID == RoleID).First();
            RoleEditViewModel roleview = new RoleEditViewModel { role = role, selectedquanxians = new List<checklst>(), allquanxians = new List<checklst>() };

            foreach (QuanXian qx in accountdb.QuanXian)
            {
                checklst cl = new checklst();
                cl.id = qx.QuanXianID;
                cl.name = qx.QuanXianRemark;
                if (role.Role_QuanXians.Where(g => g.QuanXian.QuanXianID == qx.QuanXianID).Count() > 0)
                {
                    cl.isselected = true;
                    checklst cl1 = new checklst();
                    cl1.id = qx.QuanXianID;
                    cl1.name = qx.QuanXianRemark;
                    cl1.isselected = true;
                    roleview.selectedquanxians.Add(cl1);
                }
                else
                {
                    cl.isselected = false;
                }
                roleview.allquanxians.Add(cl);
            }
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(roleview);
        }
        [HttpPost]
        public ActionResult Edit(RoleEditViewModel roleview)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                if (accountdb.Role.First(g => g.RoleID == roleview.role.RoleID).RoleName == "超级管理员")
                {
                    //ModelState.AddModelError("管理员角色不能改动", new Exception("管理员角色不能改动"));
                    msg = "超级管理员角色不能改动";
                    ViewData["msg"] = msg;
                    return View(new RoleEditViewModel());
                }
                else
                {
                    Role r = accountdb.Role.Include("Role_QuanXians").Where(g => g.RoleID == roleview.role.RoleID).First();
                    r.RoleName = roleview.role.RoleName;
                    r.RoleRemark = roleview.role.RoleRemark;
                    ///url权限
                    ///
                    if (roleview.postquanxianids != null)
                    {
                        foreach (QuanXian qx in accountdb.QuanXian)
                        {
                            if (roleview.postquanxianids.Ids.Contains(qx.QuanXianID))
                            {
                                if (r.Role_QuanXians.Where(g => g.QuanXianID == qx.QuanXianID).Count() == 0)
                                {
                                    //不包含，需要添加
                                    Role_QuanXian rqx = new Role_QuanXian();
                                    rqx.QuanXianID = qx.QuanXianID;
                                    rqx.RoleID = r.RoleID;
                                    accountdb.Role_QuanXian.Add(rqx);
                                }
                            }
                            else
                            {
                                IEnumerable<Role_QuanXian> rqx = r.Role_QuanXians.Where(g => g.QuanXianID == qx.QuanXianID);
                                if (rqx.Count() > 0)
                                {
                                    //不包含，需要添加
                                    accountdb.Role_QuanXian.Remove(rqx.First());
                                }
                            }
                        }
                    }
                    accountdb.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(roleview);
        }
        public class Rolelist
        {
            public int xuhao { get; set; }
            public string RoleID { get; set; }
            public string RoleGroup { get; set; }
            public string RoleName { get; set; }
            public string RoleRemark { get; set; }
        }

    }
}
