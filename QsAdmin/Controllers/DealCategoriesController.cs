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
    public class DealCategoriesController : QsAdminController
    {

        // GET: DealCategories
        public ActionResult Index()
        {
            return View(db.DealCategories.ToList());
        }

        // GET: DealCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DealCategory dealCategory = db.DealCategories.Find(id);
            if (dealCategory == null)
            {
                return HttpNotFound();
            }
            return View(dealCategory);
        }

        // GET: DealCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DealCategories/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DealCategoryId,DealCategoryName,BasicPrice")] DealCategory dealCategory)
        {
            if (ModelState.IsValid)
            {
                db.DealCategories.Add(dealCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dealCategory);
        }

        // GET: DealCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DealCategory dealCategory = db.DealCategories.Find(id);
            if (dealCategory == null)
            {
                return HttpNotFound();
            }
            return View(dealCategory);
        }

        // POST: DealCategories/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DealCategoryId,DealCategoryName,BasicPrice")] DealCategory dealCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealCategory);
        }

        // GET: DealCategories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DealCategory dealCategory = db.DealCategories.Find(id);
            if (dealCategory == null)
            {
                return HttpNotFound();
            }
            return View(dealCategory);
        }

        // POST: DealCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DealCategory dealCategory = db.DealCategories.Find(id);
            db.DealCategories.Remove(dealCategory);
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
