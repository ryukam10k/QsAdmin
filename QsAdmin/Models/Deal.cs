using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class Deal
    {
        public int Id { get; set; }

        [DisplayName("取引先")]
        public string CustomerId { get; set; }

        [DisplayName("取引区分")]
        public string DealCategoryId { get; set; }

        [DisplayName("登録者")]
        public string AccountId { get; set; }

        [DisplayName("タグ")]
        public string Tag { get; set; }

        [DisplayName("単価")]
        public int Price { get; set; }

        [DisplayName("件数")]
        public int Number { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("メモ")]
        public string Memo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [DisplayName("受付日時")]
        public DateTime ReceptionDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [DisplayName("完了日時")]
        public DateTime? EndDate { get; set; }

        [DisplayName("売上計上月")]
        public string KeijoTsuki { get; set; }


        public virtual Customer Customer { get; set; }

        public virtual DealCategory DealCategory { get; set; }

        public virtual ApplicationUser Account { get; set; }


        /// <summary>
        /// 売上計上月
        /// </summary>
        /// <returns></returns>
        public string GetKeijoTsuki()
        {
            var date = DateTime.Now;
            int Shimebi = DateTime.DaysInMonth(date.Year, date.Month);

            if (Customer != null && Customer.ClosingDate != 0)
            {
                Shimebi = Customer.ClosingDate;
            }

            DateTime _EndDate = DateTime.Now;

            if (EndDate != null) {
                _EndDate = (DateTime)EndDate;
            }

            if (_EndDate.Day > Shimebi)
            {
                // 締日より後日付の売上は次月計上とする
                _EndDate = _EndDate.AddMonths(1);
            }

            string Year = _EndDate.Year.ToString();
            string Month = _EndDate.Month.ToString().PadLeft(2, '0');

            return Year + "/" + Month;
        }

    }

}