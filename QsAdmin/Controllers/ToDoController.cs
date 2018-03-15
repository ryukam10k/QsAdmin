using Microsoft.AspNet.Identity;
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
    public class ToDosController : QsAdminController
    {
        private ToDoManager m = new ToDoManager();

        public ActionResult Index(int? kanryoToDoId, int? returnToDoId)
        {
            // 完了
            m.EndToDo(kanryoToDoId);

            // 未完了に戻す
            m.ReturnToDo(returnToDoId);

            ToDoView view = new ToDoView();

            // 未完了ToDo取得
            view.MikanryoToDo = m.GetToDoList();

            // 完了ToDo取得（7日前に完了したものまで）
            view.KanryoToDo = m.GetKanryoToDoList(DateTime.Now.AddDays(-7));

            return View(view);
        }

        public ActionResult ToDoViewKanryo()
        {
            // 完了ToDo取得（全て）
            List<KanryoToDoView> toDo = m.GetKanryoToDoList(DateTime.MinValue);

            return View(toDo);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo ToDoData = db.ToDos.Find(id);
            if (ToDoData == null)
            {
                return HttpNotFound();
            }
            return View(ToDoData);
        }

        public ActionResult Create()
        {
            ToDo ToDo = new ToDo()
            {
                AccountId = User.Identity.GetUserId()
            };

            ViewBag.TantoshaId = new SelectList(db.Users, "Id", "AccountName");

            return View(ToDo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Title,Description,TantoshaId,ShimekiriDate,KanryoDate")] ToDo ToDoData)
        {
            if (ModelState.IsValid)
            {
                db.ToDos.Add(ToDoData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TantoshaId = new SelectList(db.Users, "Id", "AccountName");

            return View(ToDoData);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo ToDoData = db.ToDos.Find(id);
            if (ToDoData == null)
            {
                return HttpNotFound();
            }

            ViewBag.TantoshaId = new SelectList(db.Users, "Id", "AccountName");

            return View(ToDoData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Title,Description,TantoshaId,ShimekiriDate,KanryoDate")] ToDo ToDoData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ToDoData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TantoshaId = new SelectList(db.Users, "Id", "AccountName");

            return View(ToDoData);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo ToDoData = db.ToDos.Find(id);
            if (ToDoData == null)
            {
                return HttpNotFound();
            }
            return View(ToDoData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDo ToDoData = db.ToDos.Find(id);
            db.ToDos.Remove(ToDoData);
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

    internal class ButtonAttribute : Attribute
    {
    }
}
