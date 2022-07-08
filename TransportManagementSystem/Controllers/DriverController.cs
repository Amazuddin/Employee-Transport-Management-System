using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransportManagementSystem.Models;
using TransportManagementSystem.Context;

namespace TransportManagementSystem.Controllers
{
    public class DriverController : Controller
    {
        private TMSContext db = new TMSContext();

        // GET: /Driver/
        public ActionResult DriverIndex()
        {
            ViewBag.DriverIndex = "active";
            return View(db.Driver.ToList());
        }

        // GET: /Driver/Create
        public ActionResult Create()
        {
            ViewBag.route = db.Route.ToList();
            return View();
        }

        // POST: /Driver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DriverName,Phone,VehicleNo,RouteId,Image")] DriverInfo driverinfo, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {

                    try
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Image.FileName);
                        string uploadUrl = Server.MapPath("~/picture");
                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        driverinfo.Image = "picture/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }

                driverinfo.DriverName = driverinfo.DriverName;
                driverinfo.Phone = driverinfo.Phone;
                driverinfo.VehicleNo = driverinfo.VehicleNo;
                driverinfo.RouteId = driverinfo.RouteId;
                db.Driver.Add(driverinfo);
                db.SaveChanges();
            }

            return RedirectToAction("DriverIndex", new { message = "Driver added Successfully" });
        }

        // GET: /Driver/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.route = db.Route.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverInfo driverinfo = db.Driver.Find(id);
            if (driverinfo == null)
            {
                return HttpNotFound();
            }
            return View(driverinfo);
        }

        // POST: /Driver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DriverName,Phone,VehicleNo,RouteId,Image")] DriverInfo driverinfo, HttpPostedFileBase Image, string pastImage)
        {

            driverinfo.Image = pastImage;
            if (Image != null && Image.ContentLength > 0)
            {
                string fullPath = Request.MapPath("~/" + pastImage);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                try
                {
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Image.FileName);
                    string uploadUrl = Server.MapPath("~/picture");
                    Image.SaveAs(Path.Combine(uploadUrl, fileName));
                    driverinfo.Image = "picture/" + fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "ERROR:" + ex.Message.ToString();
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(driverinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DriverIndex");
            }
            return View(driverinfo);
        }

        // GET: /Driver/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverInfo driverinfo = db.Driver.Find(id);
            if (driverinfo == null)
            {
                return HttpNotFound();
            }
            return View(driverinfo);
        }

        // POST: /Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DriverInfo driverinfo = db.Driver.Find(id);
            db.Driver.Remove(driverinfo);
            db.SaveChanges();
            return RedirectToAction("DriverIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
