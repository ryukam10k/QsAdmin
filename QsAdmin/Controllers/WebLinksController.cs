using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QsAdmin.Models;

namespace QsAdmin.Controllers
{
    public class WebLinksController : QsAdminController
    {

        // GET: WebLinks
        public ActionResult Index()
        {
            var webLinks = db.WebLinks.Include(w => w.Account);
            return View(webLinks.ToList());
        }

        // GET: WebLinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebLink webLink = db.WebLinks.Find(id);
            if (webLink == null)
            {
                return HttpNotFound();
            }
            return View(webLink);
        }

        // GET: WebLinks/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Users, "Id", "AccountName");
            return View();
        }

        // POST: WebLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Url,Name")] WebLink webLink)
        {
            if (ModelState.IsValid)
            {
                db.WebLinks.Add(webLink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Users, "Id", "AccountName", webLink.AccountId);
            return View(webLink);
        }

        // GET: WebLinks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebLink webLink = db.WebLinks.Find(id);
            if (webLink == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Users, "Id", "AccountName", webLink.AccountId);
            return View(webLink);
        }

        // POST: WebLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Url,Name")] WebLink webLink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Users, "Id", "AccountName", webLink.AccountId);
            return View(webLink);
        }

        // GET: WebLinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebLink webLink = db.WebLinks.Find(id);
            if (webLink == null)
            {
                return HttpNotFound();
            }
            return View(webLink);
        }

        // POST: WebLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WebLink webLink = db.WebLinks.Find(id);
            db.WebLinks.Remove(webLink);
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
    }
}
