using QsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QsAdmin.Controllers
{
    public class HomeController : QsAdminController
    {
        public ActionResult Index()
        {
            // 未完了タスク数取得
            ToDoManager m1 = new ToDoManager();
            ViewBag._ToDoCount = m1.GetToDoList().Count();

            // 未完了取引数取得
            DealManager m2 = new DealManager();
            ViewBag._DealsCount = m2.GetDealList(null, null, false, null).Count();

            return View();
        }

    }
}