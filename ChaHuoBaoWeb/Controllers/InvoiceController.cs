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
    public class InvoiceController : Controller
    {
        //
        // GET: /Invoice/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string IsOut, DateTime? startDate, DateTime? endDate, string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<InvoiceModel> invoiceModel = accountdb.InvoiceModel;

            if (IsOut != "0")
            {
                bool shenhezhuangtai = true;

                if (IsOut == "1") { shenhezhuangtai = true; }
                if (IsOut == "2") { shenhezhuangtai = false; }

                invoiceModel = invoiceModel.Where(p => p.IsOut == shenhezhuangtai);

            }

            if (!string.IsNullOrEmpty(startDate.ToString()))
            {
                invoiceModel = invoiceModel.Where(p => p.AddTime >= startDate);
            }
            if (!string.IsNullOrEmpty(endDate.ToString()))
            {
                invoiceModel = invoiceModel.Where(p => p.AddTime <= Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1));
            }
            invoiceModel = invoiceModel.OrderByDescending(p => p.AddTime);
            var total = invoiceModel.Count();
            var currentPersonList = invoiceModel
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<Invoicelist> invoiceModels = new List<Invoicelist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                string zhuangtai = "";
                Invoicelist invoiceone = new Invoicelist();
                if (obj.IsOut == true)
                {
                    zhuangtai = "已审核寄送";
                }
                else
                {
                    zhuangtai = "未审核寄送";
                }
                invoiceone.xuhao = n;
                invoiceone.InvoiceId = obj.InvoiceId;
                invoiceone.InvoiceTitle = obj.InvoiceTitle;
                invoiceone.InvoiceZZJGDM = obj.InvoiceZZJGDM;
                invoiceone.UserID = obj.UserId;
                invoiceone.InvoicePerson = obj.InvoicePerson;
                invoiceone.InvoiceMobile = obj.InvoiceMobile;
                invoiceone.InvoiceAddress = obj.InvoiceAddress;
                invoiceone.IsOut = zhuangtai;
                invoiceone.InvoiceJe = obj.InvoiceJe;
                invoiceone.AddTime = (DateTime)obj.AddTime;
                invoiceModels.Add(invoiceone);

            }
            var rows = invoiceModels.Select(p => new
            {
                id = p.xuhao,
                InvoiceId = p.InvoiceId,
                InvoiceTitle = p.InvoiceTitle,
                InvoiceZZJGDM = p.InvoiceZZJGDM,
                UserID = p.UserID,
                InvoicePerson = p.InvoicePerson,
                InvoiceMobile = p.InvoiceMobile,
                InvoiceAddress = p.InvoiceAddress,
                IsOut = p.IsOut,
                InvoiceJe = p.InvoiceJe,
                AddTime = p.AddTime.ToString()
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        public class Invoicelist
        {
            public int xuhao { get; set; }

            public string InvoiceId { get; set; }

            public string InvoiceTitle { get; set; }

            public string InvoiceZZJGDM { get; set; }

            public string UserID { get; set; }

            public string InvoicePerson { get; set; }

            public string InvoiceMobile { get; set; }

            public string InvoiceAddress { get; set; }

            public string IsOut { get; set; }

            public decimal InvoiceJe { get; set; }

            public DateTime AddTime { get; set; }
        }

        //审核
        //审核
        //审核
        [PermissionAuthorize]
        public string DataShenhe()
        {
            string InvoiceId = HttpContext.Request["InvoiceId"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.InvoiceModel invoicemodel = db.InvoiceModel.Where(g => g.InvoiceId == InvoiceId).First();
                if (invoicemodel.IsOut == true)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "此单据已审核！";
                }
                else
                {
                    invoicemodel.IsOut = true;
                    db.SaveChanges();

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
