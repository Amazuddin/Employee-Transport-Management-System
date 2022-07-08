using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportManagementSystem.Context;
using TransportManagementSystem.Models;

namespace TransportManagementSystem.Controllers
{
    public class TranportController : Controller
    {
        private LoginController login = new LoginController();
        //
        // GET: /Tranport/
        public ActionResult Index()
        {
            ViewBag.Index = "active";
            return View();
        }

        public ActionResult DriverInformation()
        {
            ViewBag.Driver = "active";
            var driverInformations = new List<DriverVehicleViewModel>();
            ;
            using (var ctx = new TMSContext())
            {
                var driverInfos = from driver in ctx.Driver
                    join route in ctx.Route
                        on driver.RouteId equals route.Id
                    where driver.RouteId == route.Id
                    select new
                    {
                        driverName = driver.DriverName,
                        phone = driver.Phone,
                        busNo = driver.VehicleNo,
                        image = driver.Image,
                        routesName = route.RoutesName
                    };

                foreach (var item in driverInfos)
                {
                    var driverVehicle = new DriverVehicleViewModel();
                    driverVehicle.DriverName = item.driverName;
                    driverVehicle.Phone = item.phone;
                    driverVehicle.Image = item.image;
                    driverVehicle.VehicleNo = item.busNo;
                    driverVehicle.RouteName = item.routesName;
                    driverInformations.Add(driverVehicle);
                }
            }
            return View(driverInformations);
        }

        public ActionResult RouteInformation()
        {
            ViewBag.RouteInformation = "active";
            var routes = new List<Routes>();

            using (var db = new TMSContext())
            {
                routes = db.Route.ToList();
            }

            ViewBag.routes = routes;
            return View();
        }

        public JsonResult GetAllRouteInfoById(int id)
        {
            var routeInformations = new List<RoutesInfoView>();

            using (var ctx = new TMSContext())
            {
                var routeInfos = from routeInfo in ctx.RoutesInformation
                    join route in ctx.Route
                        on routeInfo.RouteId equals route.Id
                    join driverInfo in ctx.Driver
                        on routeInfo.DriverInfoId equals driverInfo.Id
                    where routeInfo.RouteId == id
                    select new
                    {
                        driverName = driverInfo.DriverName,
                        vehicleNo = driverInfo.VehicleNo,
                        routesName = route.RoutesName,
                        startTime = routeInfo.StartTime
                    };

                foreach (var item in routeInfos)
                {
                    var routeInformation = new RoutesInfoView();
                    routeInformation.DriverName = item.driverName;
                    routeInformation.VehicleNo = item.vehicleNo;
                    routeInformation.RoutesName = item.routesName;
                    routeInformation.StartTime = item.startTime;
                    routeInformations.Add(routeInformation);
                }
            }
            return Json(routeInformations);
        }

        public ActionResult Notification()
        {
            ViewBag.Notification = "active";
            var noti = new List<Notification>();
            using (var db = new TMSContext())
            {
                noti = db.Notifications.ToList();
            }

            return View(noti);
        }

        public ActionResult Profile()
        {
            ViewBag.Profile = "active";
            var employeeId = Convert.ToInt32(Session["EmployeeId"]);

            var employee = new Employee();
            using (var db = new TMSContext())
            {
                var am = db.Employees.Where(s => s.Id == employeeId);
                foreach (var k in am)
                {
                    employee.EmployeeName = k.EmployeeName;
                    employee.Email = k.Email;
                    employee.Mobile = k.Mobile;
                    employee.Address = k.Address;
                    employee.Image = k.Image;
                }

                ViewBag.Allinfo = employee;
            }

            return View();
        }

        public ActionResult ProfileUpdate()
        {
            ViewBag.ProfileUpdate = "active";
            var employeeId = Convert.ToInt32(Session["EmployeeId"]);

            var employee = new Employee();
            using (var db = new TMSContext())
            {
                var u = db.Employees.Where(k => k.Id == employeeId).Select(c =>
                    new {c.EmployeeName, c.Email, c.Mobile, c.Address, c.Image});
                foreach (var k in u)
                {
                    employee.EmployeeName = k.EmployeeName;
                    employee.Email = k.Email;
                    employee.Mobile = k.Mobile;
                    employee.Address = k.Address;
                    employee.Image = k.Image;
                }

                ViewBag.Employee = employee;
                return View();
            }
        }

        public ActionResult Update(Employee employee, HttpPostedFileBase Image, string pastImage)
        {
            var employeeId = Convert.ToInt32(Session["EmployeeId"]);

            using (var db = new TMSContext())
            {
                employee.Image = pastImage;
                if (Image != null && Image.ContentLength > 0)
                {
                    var fullPath = Request.MapPath("~/" + pastImage);
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                    try
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                        var uploadUrl = Server.MapPath("~/picture");
                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        employee.Image = "picture/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "ERROR:" + ex.Message;
                    }
                }

                var pa = db.Employees.Single(e => e.Id == employeeId);
                if (pa.Id == employeeId)
                {
                    pa.EmployeeName = employee.EmployeeName;
                    pa.Email = employee.Email;
                    pa.Mobile = employee.Mobile;
                    pa.Address = employee.Address;
                    pa.Image = employee.Image;
                    db.SaveChanges();
                    return RedirectToAction("Profile", "Tranport");
                }

                return RedirectToAction("Profile", "Tranport");
            }
        }
    }
}