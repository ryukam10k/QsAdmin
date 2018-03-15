using Microsoft.AspNet.Identity;
using QsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QsAdmin.Controllers
{
    public class QsAdminController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// アクションメソッド実行前に呼び出されるメソッド
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            CreateMenu();
        }

        /// <summary>
        /// Sessionチェック
        /// </summary>
        public void CreateMenu()
        {
            // 認証済の場合
            if (User != null && User.Identity.IsAuthenticated)
            {
                // 外部リンク
                if (Session["WEB_LINKS"] == null)
                {
                    string userId = User.Identity.GetUserId();
                    Session["WEB_LINKS"] = db.WebLinks.Where(a => a.AccountId == userId).ToList();
                }

            }
        }
    }
}