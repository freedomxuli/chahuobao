using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using ChaHuoBaoWeb.Filters;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class JiaGeCeLveController : Controller
    {
        //
        // GET: /JiaGeCeLve/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            IEnumerable<JiaGeCeLve> celve = accountdb.JiaGeCeLve;
            return View(celve);
        }


        //价格策略删除
        [HttpPost]
        [PermissionAuthorize]
        public ActionResult Delete(int JiaGeCeLveID)
        {

            IEnumerable<JiaGeCeLve> chongzhicelv = accountdb.JiaGeCeLve.Where(x => x.JiaGeCeLveID == JiaGeCeLveID);
            accountdb.JiaGeCeLve.Remove(chongzhicelv.First());
            accountdb.SaveChanges();
            return Json("已经成功删除该充值策略！");
        }


        //充值策略新增
        //充值策略新增
        [PermissionAuthorize]
        public ActionResult Add()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Add(JiaGeCeLve xinzengchongzhi)
        {

            string msg = "";
            //非必填项设置默认值
            xinzengchongzhi.JiaGeCeLveLeiXing = "ChongZhi";

            //验证参数
            if (string.IsNullOrEmpty(xinzengchongzhi.JiaGeCeLveName))
            {
                msg = "套餐内容不能为空";
                ViewData["msg"] = msg;
                return View(new JiaGeCeLve());
            }
            //else if (!RegexHelper.IsMatch(xinzengchongzhi.shibiema, @"^[0-9]{5}$"))
            //{
            //    msg = "公司识别码应为5位有效数字";
            //}
            if (xinzengchongzhi.JiaGeCeLveCiShu <= 1)
            {
                msg = "充值次数必须大于 1";
                ViewData["msg"] = msg;
                return View(new JiaGeCeLve());
            }

            if (string.IsNullOrEmpty(xinzengchongzhi.JiaGeCeLveJinE.ToString()))
            {
                msg = "充值金额不能为空！";
                ViewData["msg"] = msg;
                return View(new JiaGeCeLve());
            }
            if (string.IsNullOrEmpty(xinzengchongzhi.JiaGeCeLveCiShu.ToString()))
            {
                msg = "充值次数不能为空！";
                ViewData["msg"] = msg;
                return View(new JiaGeCeLve());
            }
            accountdb.JiaGeCeLve.Add(xinzengchongzhi);
            accountdb.SaveChanges();
            return RedirectToAction("Index");
            //ViewData["msg"] = msg;
            //return View(new JiaGeCeLve());
        }


        //策略修改
        //策略修改
        [PermissionAuthorize]
        public ActionResult Edit(int id)
        {
            IEnumerable<JiaGeCeLve> celv = accountdb.JiaGeCeLve.Where(x => x.JiaGeCeLveID == id);
            return View(celv.First());
        }
        [HttpPost]
        public ActionResult Edit(JiaGeCeLve xiugai)
        {
            string msg = "";
            int id = xiugai.JiaGeCeLveID;
            IEnumerable<JiaGeCeLve> celv = accountdb.JiaGeCeLve.Where(x => x.JiaGeCeLveID == id);

            celv.First().JiaGeCeLveCiShu = xiugai.JiaGeCeLveCiShu;
            celv.First().JiaGeCeLveJinE = xiugai.JiaGeCeLveJinE;
            celv.First().JiaGeCeLveLeiXing = xiugai.JiaGeCeLveLeiXing;
            celv.First().JiaGeCeLveName = xiugai.JiaGeCeLveName;
            celv.First().JiaGeCeLveRemark = xiugai.JiaGeCeLveRemark;
            accountdb.SaveChanges();
            msg = "充值金额修改成功！";
            ViewData["msg"] = msg;

            return RedirectToAction("Index");
            //return View(celv.First());
        }
    }
}
