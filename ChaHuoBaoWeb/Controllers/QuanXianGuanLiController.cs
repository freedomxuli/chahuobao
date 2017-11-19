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
    public class QuanXianGuanLiController : Controller
    {
        //
        // GET: /QuanXianGuanLi/

        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        //权限管理首页
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<QuanXian> QuanXianModel = accountdb.QuanXian.OrderBy(x=>x.QuanXianRemark);
            var total = QuanXianModel.Count();
            var currentPersonList = QuanXianModel
                                            .Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<QuanXianlist> QuanXianModels = new List<QuanXianlist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                QuanXianlist quanxian_new = new QuanXianlist();
                quanxian_new.xuhao = n;
                quanxian_new.QuanXianID = obj.QuanXianID;
                quanxian_new.QuanXianName = obj.QuanXianName;
                quanxian_new.QuanXianRemark = obj.QuanXianRemark;
                QuanXianModels.Add(quanxian_new);
            }
            var rows = QuanXianModels.Select(p => new
            {
                id = p.xuhao,
                QuanXianID = p.QuanXianID,
                QuanXianName = p.QuanXianName,
                QuanXianRemark = p.QuanXianRemark,
            });
            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        //权限管理新增
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(QuanXian quanxianxinzneg)
        {
            string msg = "";
            //非必填项设置默认值
            quanxianxinzneg.QuanXianID = Guid.NewGuid().ToString();
            //验证参数
            if (string.IsNullOrEmpty(quanxianxinzneg.QuanXianName))
            {
                msg = "权限名称不能为空";
                ViewData["msg"] = msg;
                return View(new QuanXian());
            }
            if (string.IsNullOrEmpty(quanxianxinzneg.QuanXianRemark))
            {
                msg = "权限说明不能为空";
                ViewData["msg"] = msg;
                return View(new QuanXian());
            }
            IEnumerable<QuanXian> QuanXian = accountdb.QuanXian.Where(x => x.QuanXianName == quanxianxinzneg.QuanXianName);
            if (QuanXian.Count() == 0)
            {
                accountdb.QuanXian.Add(quanxianxinzneg);
                accountdb.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                msg = "该权限已存在，无法重复新增！";
            }
            ViewData["msg"] = msg;
            return View(new QuanXian());
        }

        //权限管理删除
        public ActionResult Delete(string QuanXianID)
        {
            IEnumerable<QuanXian> QuanXian = accountdb.QuanXian.Where(x => x.QuanXianID == QuanXianID);
            accountdb.QuanXian.Remove(QuanXian.First());
            accountdb.SaveChanges();
            return Json(new { state = true, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

        //权限管理修改
        public ActionResult Edit(string QuanXianID)
        {
            IEnumerable<QuanXian> QuanXian = accountdb.QuanXian.Where(x => x.QuanXianID == QuanXianID);
            return View(QuanXian.First());
        }
        [HttpPost]
        public ActionResult Edit(QuanXian quanxianxiugai)
        {
            string msg = "";
            string QuanXianID = quanxianxiugai.QuanXianID;
            IEnumerable<QuanXian> QuanXian2 = accountdb.QuanXian.Where(x => x.QuanXianName == quanxianxiugai.QuanXianName);
            IEnumerable<QuanXian> QuanXian = accountdb.QuanXian.Where(x => x.QuanXianID == QuanXianID);
            if (string.IsNullOrEmpty(quanxianxiugai.QuanXianName))
            {
                msg = "权限名称不能为空";
                ViewData["msg"] = msg;
                return View(QuanXian.First());
            }
            if (string.IsNullOrEmpty(quanxianxiugai.QuanXianRemark))
            {
                msg = "权限说明不能为空";
                ViewData["msg"] = msg;
                return View(QuanXian.First());
            }
            if (QuanXian2.Count() == 0)
            {
                QuanXian.First().QuanXianName = quanxianxiugai.QuanXianName;
                QuanXian.First().QuanXianRemark = quanxianxiugai.QuanXianRemark;
                accountdb.SaveChanges();
                msg = "权限修改成功！";
                ViewData["msg"] = msg;
                return View(QuanXian.First());
            }
            else if (QuanXian2.First().QuanXianID != QuanXianID)
            {
                msg = "权限修改失败，已存在相同权限！";
                ViewData["msg"] = msg;
                return View(QuanXian.First());
            }
            else
            {
                QuanXian.First().QuanXianName = quanxianxiugai.QuanXianName;
                QuanXian.First().QuanXianRemark = quanxianxiugai.QuanXianRemark;
                accountdb.SaveChanges();
                msg = "权限修改成功！";
                ViewData["msg"] = msg;
                return View(QuanXian.First());
            }
        }
        public class QuanXianlist
        {
            public int xuhao { get; set; }
            public string QuanXianID { get; set; }
            public string QuanXianName { get; set; }
            public string QuanXianRemark { get; set; }
        }

    }
}
