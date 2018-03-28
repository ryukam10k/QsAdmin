using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QsAdmin.Models
{
    public class DealManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void EndDeal(int? endDealId)
        {
            if (endDealId != null)
            {
                Deal deal = db.Deals.Find(endDealId);
                if (string.IsNullOrEmpty(deal.KeijoTsuki))
                {
                    deal.KeijoTsuki = deal.GetKeijoTsuki();
                }
                deal.EndDate = DateTime.Now;
                db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<DealViewModel> GetDealList(SeachCondition condition)
        {
            var deals = (from a in db.Deals
                         from b in db.DealCategories.Where(x => x.DealCategoryId == a.DealCategoryId).DefaultIfEmpty()
                         from c in db.Customers.Where(x => x.CustomerId == a.CustomerId).DefaultIfEmpty()
                         orderby a.ReceptionDate ascending
                         select new DealViewModel
                         {
                             Id = a.Id,
                             ReceptionDate = a.ReceptionDate,
                             DealCategoryName = b.DealCategoryName,
                             Tag = a.Tag,
                             CustomerName = c.CustomerShortName,
                             Number = a.Number,
                             Price = a.Price,
                             Memo = a.Memo,
                             EndDate = a.EndDate,
                             KeijoTsuki = a.KeijoTsuki
                         });

            bool kanryo = condition.Kanryo;
            string year = condition.Year.ToString();
            string month = condition.Month.ToString();
            string keyword = condition.Keyword;

            // 完了分
            if (kanryo)
            {
                // 計上年
                deals = deals.Where(a => a.KeijoTsuki.Substring(0, 4) == year || a.KeijoTsuki == null);
                // 計上月
                string month2 = month.PadLeft(2, '0');
                deals = deals.Where(a => a.KeijoTsuki.Substring(5, 2) == month2 || a.KeijoTsuki == null);
            }
            else
            {
                deals = deals.Where(a => a.EndDate == null);
            }

            // キーワード
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                keyword = keyword.Replace('　', ' ');
                string[] keywords = keyword.Split(' ');

                foreach (string key in keywords)
                {
                    deals = deals.Where(a => a.DealCategoryName.Contains(key)
                        || a.CustomerName.Contains(key)
                        || a.Tag.Contains(key)
                        || a.Memo.Contains(key)
                        );
                }

            }

            return deals.ToList();
        }


        public List<SelectListItem> GetYearMonthOptions()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            DateTime start = new DateTime(2015, 1, 1);
            DateTime end = DateTime.Now;
            List<DateTime> DateList = new List<DateTime>();
            GetDateList(ref DateList, start, end);
            foreach (DateTime date in DateList)
            {
                list.Add(
                    new SelectListItem
                    {
                        Value = date.ToString("yyyy/MM"),
                        Text = date.ToString("yyyy/MM"),
                        Selected = date.ToString("yyyy/MM") == "2017/01"
                    }
                );
            }

            return list;
        }

        static void GetDateList(ref List<DateTime> DateList, DateTime start, DateTime end)
        {
            DateList.Clear();
            for (DateTime date = start; date <= end; date = date.AddMonths(1))
            {
                DateList.Add(date);
            }
        }

        public IEnumerable<SelectListItem> GetYearOptions(int selectedValue)
        {
            // 直近の 5 年を選択肢として取得する。
            return Enumerable
                .Range(DateTime.Now.Year - 3, 5)
                .Select(t => new SelectListItem()
                {
                    Value = t.ToString(),
                    Text = t.ToString(),
                    Selected = t == selectedValue
                });
        }

        public IEnumerable<SelectListItem> GetMonthOptions(int selectedValue)
        {
            return Enumerable
                .Range(1, 12)
                .Select(t => new SelectListItem()
                {
                    Value = t.ToString(),
                    Text = t.ToString(),
                    Selected = t == selectedValue
                });
        }

    }
}