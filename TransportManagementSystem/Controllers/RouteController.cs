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
    public class RouteController : Controller
    {
        private TMSContext db = new TMSContext();

        // GET: /Route/
        public ActionResult RouteIndex()
        {
            ViewBag.RouteIndex = "active";
            return View(db.Route.ToList());
        }

      

        // GET: /Route/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Route/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,RoutesName")] Routes routes)
        {
            if (ModelState.IsValid)
            {
                db.Route.Add(routes);
                db.SaveChanges();
                return RedirectToAction("RouteIndex");
            }

            return View(routes);
        }

        // GET: /Route/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Route.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            return View(routes);
        }

        // POST: /Route/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,RoutesName")] Routes routes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RouteIndex");
            }
            return View(routes);
        }

        // GET: /Route/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Route.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            return View(routes);
        }

        // POST: /Route/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Routes routes = db.Route.Find(id);
            db.Route.Remove(routes);
            db.SaveChanges();
            return RedirectToAction("RouteIndex");
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
