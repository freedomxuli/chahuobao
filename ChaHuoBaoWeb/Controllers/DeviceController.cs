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
    public class DeviceController : Controller
    {
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string sortName, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            IEnumerable<GpsDeviceTable> GpsDeviceTable = accountdb.GpsDeviceTable;

            GpsDeviceTable = GpsDeviceTable.OrderByDescending(p => p.ID);
            var total = GpsDeviceTable.Count();
            var currentPersonList = GpsDeviceTable
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize).ToList();
            int n = (pageIndex - 1) * pageSize;
            List<GpsDeviceTablelist> GpsDeviceTables = new List<GpsDeviceTablelist>();
            foreach (var obj in currentPersonList)
            {
                n = n + 1;
                GpsDeviceTablelist gpsdevicetableone = new GpsDeviceTablelist();
                gpsdevicetableone.xuhao = n;
                gpsdevicetableone.ID = obj.ID;
                gpsdevicetableone.DeviceCode = obj.DeviceCode;
                gpsdevicetableone.TableName = obj.TableName;
                gpsdevicetableone.DeviceTime = obj.DeviceTime;
                GpsDeviceTables.Add(gpsdevicetableone);

            }
            var rows = GpsDeviceTables.Select(p => new
            {
                ID = p.ID,
                DeviceCode = p.DeviceCode,
                TableName = p.TableName,
                DeviceTime = p.DeviceTime
            });

            return Json(new { total = total, rows = rows, state = true, msg = "加载成功" }, JsonRequestBehavior.AllowGet);
        }

        public class GpsDeviceTablelist
        {
            public int xuhao { get; set; }

            public int ID { get; set; }

            public string DeviceCode { get; set; }

            public string TableName { get; set; }

            public int DeviceTime { get; set; }
        }
    }
}