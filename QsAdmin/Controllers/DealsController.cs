using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QsAdmin.Models;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Web.Helpers;
using WebGrease.Css.Extensions;
using System.Globalization;
using System.Data.Entity.SqlServer;
using Microsoft.AspNet.Identity.Owin;

namespace QsAdmin.Controllers
{
    public class DealsController : QsAdminController
    {
        private DealManager m = new DealManager();

        public ActionResult Index(string year, string month, bool? kanryo, string keyword, int? endDealId)
        {
            // 完了
            m.EndDeal(endDealId);

            // 取引リスト取得
            List<DealViewModel> deals = m.GetDealList(year, month, kanryo, keyword);

            ViewBag.year = m.GetYearOptions(DateTime.Now.Year);
            ViewBag.month = m.GetMonthOptions(DateTime.Now.Month);

            return View(deals.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        private List<SelectListItem> getCustomerList(string id)
        {
            var list = from c in db.Customers
                       where c.DeleteFlag == false
                       orderby c.GroupName
                       select new SelectListItem
                       {
                           Value = c.CustomerId,
                           Text = c.CustomerShortName,
                           Selected = id != null ? c.CustomerId == id : false
                       };

            return list.ToList();
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = getCustomerList(null);
            ViewBag.DealCategoryId = new SelectList(db.DealCategories, "DealCategoryId", "DealCategoryName");

            Deal deal = new Deal();

            deal.ReceptionDate = DateTime.Now;
            // 単価は現状ほぼ300円
            deal.Price = 300;
            deal.Number = 0;

            return View(deal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,DealCategoryId,AccountId,Tag,Price,Number,Memo,ReceptionDate,EndDate,KeijoTsuki")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                deal.AccountId = User.Identity.GetUserId();

                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = getCustomerList(deal.CustomerId);
            ViewBag.DealCategoryId = new SelectList(db.DealCategories, "DealCategoryId", "DealCategoryName", deal.DealCategoryId);
            return View(deal);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);

            // 完了日時と計上年月をセット
            if (deal.EndDate == null)
            {
                deal.EndDate = DateTime.Now;
                deal.KeijoTsuki = deal.GetKeijoTsuki();
            }

            if (deal == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerId = getCustomerList(deal.CustomerId);
            ViewBag.DealCategoryId = new SelectList(db.DealCategories, "DealCategoryId", "DealCategoryName", deal.DealCategoryId);
            return View(deal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,DealCategoryId,AccountId,Tag,Price,Number,Memo,ReceptionDate,EndDate,KeijoTsuki")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", deal.CustomerId);
            ViewBag.DealCategoryId = new SelectList(db.DealCategories, "DealCategoryId", "DealCategoryName", deal.DealCategoryId);
            return View(deal);
        }

        public ActionResult Copy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);

            if (string.IsNullOrEmpty(deal.KeijoTsuki))
            {
                deal.KeijoTsuki = deal.GetKeijoTsuki();
            }

            if (deal == null)
            {
                return HttpNotFound();
            }

            ViewBag.AccountId = new SelectList(db.Users, "Id", "Email", deal.AccountId);
            ViewBag.CustomerId = getCustomerList(deal.CustomerId);
            ViewBag.DealCategoryId = new SelectList(db.DealCategories, "DealCategoryId", "DealCategoryName", deal.DealCategoryId);
            return View(deal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy([Bind(Include = "Id,CustomerId,DealCategoryId,AccountId,Tag,Price,Number,Memo,ReceptionDate,EndDate,KeijoTsuki")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Users, "Id", "Email", deal.AccountId);
            ViewBag.CustomerId = getCustomerList(deal.CustomerId);
            ViewBag.DealCategoryId = new SelectList(db.DealCategories, "DealCategoryId", "DealCategoryName", deal.DealCategoryId);
            return View(deal);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deal deal = db.Deals.Find(id);
            db.Deals.Remove(deal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 月別売上表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Report(string year, string month, int? range)
        {
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.Month.ToString();
            }
            if (range == null)
            {
                range = 12;
            }

            string date = year + "/" + month + "/01";
            DateTime dateEnd = DateTime.Parse(date);
            DateTime dateStart = DateTime.Parse(date).AddMonths(((int)range - 1) * -1);

            var reports = db.Database.SqlQuery<ReportViewModel>(""
                + " select"
                + "   a.KeijoTsuki as Nengetsu,"
                + "   sum(a.Number) as Number,"
                + "   sum(a.Price * a.Number) as Uriage"
                + " from"
                + "   Deals a"
                + " where"
                + "   a.KeijoTsuki >= '" + dateStart.ToString("yyyy/MM") + "'"
                + "   and a.KeijoTsuki <= '" + dateEnd.ToString("yyyy/MM") + "'"
                + " group by"
                + "   a.KeijoTsuki"
                + " order by"
                + "   a.KeijoTsuki"
                );

            List<ReportViewModel> reportView = reports.ToList();

            ViewBag.year = m.GetYearOptions(int.Parse(year));
            ViewBag.month = m.GetMonthOptions(int.Parse(month));

            return View(reportView);
        }



        public ActionResult Seikyu(string year, string month)
        {
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.Month.ToString();
            }

            ViewBag._Year = year;
            ViewBag._Month = month;

            var seikyus = FindSeikyuView(year, month);

            ViewBag.year = m.GetYearOptions(DateTime.Now.Year);
            ViewBag.month = m.GetMonthOptions(DateTime.Now.Month);

            return View(seikyus);
        }

        public IEnumerable<SeikyuViewModel> FindSeikyuView(string year, string month)
        {
            string pYear = DateTime.Now.Year.ToString();
            string pMonth = DateTime.Now.Month.ToString();

            if (!string.IsNullOrEmpty(year))
            {
                pYear = year;
            }
            if (!string.IsNullOrEmpty(month))
            {
                pMonth = month;
            }

            string keijoTsuki = pYear + "/" + pMonth.PadLeft(2, '0');

            var seikyus = db.Database.SqlQuery<SeikyuViewModel>(""
                + " select"
                + "   b.KeijoTsuki,"
                + "   a.CustomerId,"
                + "   a.CustomerShortName,"
                + "   a.CustomerName,"
                + "   a.TransferName,"
                + "   sum(b.Number) as Number,"
                + "   sum(b.Price * b.Number) as HontaiGaku"
                + " from"
                + "   Customers a left join Deals b"
                + "   on a.CustomerId = b.CustomerId"
                + " where"
                + "   b.KeijoTsuki = '" + keijoTsuki + "'"
                + " group by"
                + "   b.KeijoTsuki,"
                + "   a.CustomerId,"
                + "   a.CustomerShortName,"
                + "   a.CustomerName,"
                + "   a.TransferName"
                + " order by"
                + "   a.CustomerId"
                );

            return seikyus;
        }

        public ActionResult SeikyuList(string year, string month)
        {
            var seikyus = FindSeikyuView(year, month);

            var str = new StringBuilder();

            // カラム名
            str.Append("店舗名,店舗略称,件数,合計額,消費税額,源泉徴収税,請求額\r\n");

            // データ
            seikyus.ForEach(s =>
                str.Append(
                    string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n",
                        s.CustomerName,
                        s.CustomerShortName,
                        s.Number,
                        s.HontaiGaku,
                        s.ShohizeiGaku,
                        s.Gensen,
                        s.SeikyuGaku
                    )
                )
            );

            string fileName = "seikyu_" + year + month + ".csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);

            return Content(str.ToString(), "text/csv", Encoding.GetEncoding("Shift-JIS"));
        }

        public ActionResult Deals(string year, string month)
        {
            var seikyus = FindSeikyuView(year, month);

            var str = new StringBuilder();

            // カラム名
            str.Append("収支区分,管理番号,発生日,支払期日,取引先,勘定科目,税区分,金額,税計算区分,税額,備考,品目,部門,メモタグ（複数指定可、カンマ区切り）,支払日,支払口座,支払金額\r\n");

            // データ
            seikyus.ForEach(s =>
                str.Append(
                    String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}\r\n",
                        "収入",
                        "",
                        year + "/" + month + "/" + "21",
                        year + "/" + (int.Parse(month) + 1) + "/" + "15",
                        s.CustomerShortName,
                        "売上高",
                        "課税売上8%",
                        s.Uriage,
                        "内税",
                        s.ShohizeiGaku,
                        s.CustomerName,
                        "",
                        "",
                        "",
                        "",
                        "",
                        ""
                    )
                )
            );

            string fileName = "deals_" + year + month + ".csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);

            return Content(str.ToString(), "text/csv", Encoding.GetEncoding("Shift-JIS"));
        }

        public ActionResult Deals2(string year, string month)
        {
            var seikyus = FindSeikyuView(year, month);

            var str = new StringBuilder();

            // カラム名
            str.Append("収支区分,管理番号,発生日,支払期日,取引先,勘定科目,税区分,金額,税計算区分,税額,備考,品目,部門,メモタグ（複数指定可、カンマ区切り）,支払日,支払口座,支払金額\r\n");

            // データ
            seikyus.ForEach(s =>
                str.Append(
                    String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}\r\n",
                        "収入",
                        "",
                        year + "/" + month + "/" + "21",
                        year + "/" + (int.Parse(month) + 1) + "/" + "15",
                        s.CustomerShortName,
                        "売上高",
                        "課税売上8%",
                        s.Uriage,
                        "内税",
                        s.ShohizeiGaku,
                        s.CustomerName,
                        "",
                        "",
                        "",
                        year + "/" + (int.Parse(month) + 1) + "/" + "15",
                        "三菱東京ＵＦＪ",
                        s.HontaiGaku + s.ShohizeiGaku
                    )
                )
            );

            // データ（源泉分）
            seikyus.ForEach(s =>
                str.Append(
                    String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}\r\n",
                        "支出",
                        "",
                        year + "/" + (int.Parse(month) + 1) + "/" + "15",
                        "",
                        s.CustomerShortName,
                        "事業主貸",
                        "対象外",
                        s.Gensen,
                        "内税",
                        0,
                        s.CustomerName,
                        "源泉所得税",
                        "",
                        "",
                        year + "/" + (int.Parse(month) + 1) + "/" + "15",
                        "三菱東京ＵＦＪ",
                        s.Gensen
                    )
                )
            );

            string fileName = "deals2_" + year + month + ".csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);

            return Content(str.ToString(), "text/csv", Encoding.GetEncoding("Shift-JIS"));
        }

        public ActionResult SeikyuIraiKb(string CustomerId, string KeijoTsuki)
        {

            var seikyus = db.Database.SqlQuery<SeikyuIraiKbViewModel>(""
                + " select"
                + "   a.DealCategoryName as Himmoku,"
                + "   b.Price,"
                + "   sum(b.Number) as Number,"
                + "   sum(b.Price * b.Number) as HontaiGaku"
                + " from"
                + "   DealCategories a left Join Deals b"
                + "   on a.DealCategoryId = b.DealCategoryId"
                + " where"
                + "   b.KeijoTsuki = '" + KeijoTsuki + "'"
                + "   and b.CustomerId = '" + CustomerId + "'"
                + " group by"
                + "   a.DealCategoryName,"
                + "   b.Price"
                );

            return View(seikyus);
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
