using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransportManagementSystem.Models;
using TransportManagementSystem.Context;

namespace TransportManagementSystem.Controllers
{
    public class RouteInformationController : Controller
    {
        private TMSContext db = new TMSContext();

        // GET: /RouteInformation/
        public ActionResult RouteInformationIndex()
        {
            ViewBag.RouteInformationIndex = "active";
            return View(db.RoutesInformation.ToList());
        }

        // GET: /RouteInformation/Create
        public ActionResult Create()
        {
            ViewBag.route = db.Route.ToList();
            ViewBag.driver = db.Driver.ToList();
            return View();
        }

        // POST: /RouteInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,RouteId,DriverInfoId,StartTime")] RouteInformation routeinformation)
        {
            if (ModelState.IsValid)
            {
                db.RoutesInformation.Add(routeinformation);
                db.SaveChanges();
                return RedirectToAction("RouteInformationIndex");
            }

            return View(routeinformation);
        }

        // GET: /RouteInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.route = db.Route.ToList();
            ViewBag.driver = db.Driver.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteInformation routeinformation = db.RoutesInformation.Find(id);
            if (routeinformation == null)
            {
                return HttpNotFound();
            }
            return View(routeinformation);
        }

        // POST: /RouteInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,RouteId,DriverInfoId,StartTime")] RouteInformation routeinformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeinformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RouteInformationIndex");
            }
            return View(routeinformation);
        }

        // GET: /RouteInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteInformation routeinformation = db.RoutesInformation.Find(id);
            if (routeinformation == null)
            {
                return HttpNotFound();
            }
            return View(routeinformation);
        }

        // POST: /RouteInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RouteInformation routeinformation = db.RoutesInformation.Find(id);
            db.RoutesInformation.Remove(routeinformation);
            db.SaveChanges();
            return RedirectToAction("RouteInformationIndex");
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
