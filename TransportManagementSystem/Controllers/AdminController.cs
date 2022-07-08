using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TransportManagementSystem.Context;
using TransportManagementSystem.Models;

namespace TransportManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly TMSContext db = new TMSContext();

        // GET: /Admin/
        public ActionResult Index()
        {
            return View(db.RoutesInformation.ToList());
        }


        // GET: /Admin/Create
        public ActionResult Create()
        {
            ViewBag.Create = "active";
            ViewBag.routes = db.Route.ToList();
            return View();
        }

        // POST: /Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,VehicleNo,RouteId,UserId,StartTime,DriverName,Phone")] RouteInformation routeinformation,
            int id, int idd)
        {
            if (ModelState.IsValid)
            {
                routeinformation.RouteId = id;
                //routeinformation.VehicleNo = routeinformation.VehicleNo;
                //routeinformation.StartTime = routeinformation.StartTime;
                //routeinformation.DriverName = routeinformation.DriverName;
                //routeinformation.Phone = routeinformation.Phone;
                db.RoutesInformation.Add(routeinformation);
                db.SaveChanges();
            }
            ViewBag.Message = "Insert Values Successfully";
            ViewBag.routes = db.Route.ToList();

            return View(routeinformation);
        }

        // GET: /Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Edit = "active";
            ViewBag.routes = db.Route.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var routeinformation = db.RoutesInformation.Find(id);
            if (routeinformation == null)
            {
                return HttpNotFound();
            }
            //routeinformation.VehicleNo = routeinformation.VehicleNo;
            //routeinformation.StartTime = routeinformation.StartTime;
            //routeinformation.DriverName = routeinformation.DriverName;
            //routeinformation.Phone = routeinformation.Phone;
            return View(routeinformation);
        }

        // POST: /Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,VehicleNo,RouteId,UserId,StartTime,DriverName,Phone")] RouteInformation routeinformation,
            int id, int idd)
        {
            if (ModelState.IsValid)
            {
                routeinformation.RouteId = id;

                //routeinformation.VehicleNo = routeinformation.VehicleNo;
                //routeinformation.StartTime = routeinformation.StartTime;
                //routeinformation.DriverName = routeinformation.DriverName;
                //routeinformation.Phone = routeinformation.Phone;
                db.Entry(routeinformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Update Values Successfully";
            return View(routeinformation);
        }

        // GET: /Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var routeinformation = db.RoutesInformation.Find(id);
            if (routeinformation == null)
            {
                return HttpNotFound();
            }
            //routeinformation.VehicleNo = routeinformation.VehicleNo;
            //routeinformation.StartTime = routeinformation.StartTime;
            //routeinformation.DriverName = routeinformation.DriverName;
            //routeinformation.Phone = routeinformation.Phone;
            return View(routeinformation);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var routeinformation = db.RoutesInformation.Find(id);
            db.RoutesInformation.Remove(routeinformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Payment()
        {
            ViewBag.Payment = "active";
           
            using (var db = new TMSContext())
            {
                ViewBag.Vehilce = db.Driver.ToList();
            }
            return View();
        }

         [HttpPost]
        public ActionResult Payment(Payment payment)
        {
            ViewBag.Payment = "active";
            using (var db = new TMSContext())
            {
                var paymentInfo =
                    db.Payments.Count(i => i.VehicleId == payment.VehicleId && i.PaymentDate == payment.PaymentDate);
                if (paymentInfo > 0)
                {
                    ViewBag.message = "Payment Already Exist!";
                }
                else
                {
                    db.Payments.Add(payment);
                    db.SaveChanges();
                    ViewBag.message = "Payment Successful!";
                }
                ViewBag.Vehilce = db.Driver.ToList();
               
            }
            return View();
        }

         public ActionResult PaymentInfo()
         {
             ViewBag.PaymentInfo = "active";
             return View();
         }
         public JsonResult GetAllPayment(string date)
         {
             List<PaymentViewModel> pv = new List<PaymentViewModel>();
             using (var ctx = new TMSContext())
             {
                 var data = from p in ctx.Payments
                            join d in ctx.Driver
                                on p.VehicleId equals d.Id
                            where p.PaymentDate == date

                            select new
                            {
                                vehicleNo = d.VehicleNo,
                                amount = p.Ammount,
                                paymentDate = p.PaymentDate,

                            };
                 foreach (var dc in data)
                 {
                     PaymentViewModel p = new PaymentViewModel();
                     p.VehicleNo = dc.vehicleNo;
                     p.Amount = dc.amount;
                     p.PaymentDate = dc.paymentDate;
                     pv.Add(p);
                 }
             }
             return Json(pv);
         }

        //public ActionResult Notice()
        //{
        //    ViewBag.Notice = "active";
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Notice(Notification notification)
        //{
        //    using (var ctx = new TMSContext())
        //    {
        //        ctx.Notifications.Add(notification);
        //        ctx.SaveChanges();
        //    }
        //    ViewBag.Yes = '1';
        //    return View();
        //}
    }
}
