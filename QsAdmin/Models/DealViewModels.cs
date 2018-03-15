using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{

    public class DealViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [DisplayName("受付日時")]
        public DateTime ReceptionDate { get; set; }

        [DisplayName("取引区分")]
        public string DealCategoryName { get; set; }

        [DisplayName("タグ")]
        public string Tag { get; set; }

        [DisplayName("取引先")]
        public string CustomerName { get; set; }

        [DisplayName("件数")]
        public int Number { get; set; }

        [DisplayName("単価")]
        public int Price { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("メモ")]
        public string Memo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("完了日時")]
        public DateTime? EndDate { get; set; }

        [DisplayName("売上計上月")]
        public string KeijoTsuki { get; set; }


        // 合計金額
        public int SumPrice()
        {
            return Price * Number;
        }

        // 売上計上月
        public string GetKeijoTsuki()
        {
            const int Shimebi = 20;

            DateTime _EndDate = DateTime.Now;

            if (EndDate != null)
            {
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

        // 完了フラグ
        public bool IsKanryo()
        {
            if (EndDate != null)
            {
                return true;
            }

            return false;
        }

        [DisplayName("メモ有")]
        public string ExistsMemo
        {
            get
            {
                string str = "";
                if (!string.IsNullOrEmpty(Memo))
                {
                    str = "〇";
                }
                return str;
            }
        }
    }


    public class ReportViewModel
    {
        [DisplayName("年月")]
        public string Nengetsu { get; set; }

        [DisplayName("件数")]
        public int Number { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("売上")]
        public int Uriage { get; set; }
    }


    public class SeikyuViewModel
    {
        public string KeijoTsuki { get; set; }

        [DisplayName("取引先ID")]
        public string CustomerId { get; set; }

        [DisplayName("取引先名")]
        public string CustomerName { get; set; }

        [DisplayName("取引先名略称")]
        public string CustomerShortName { get; set; }

        [DisplayName("振込名")]
        public string TransferName { get; set; }

        [DisplayName("件数")]
        public int Number { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("本体価格")]
        public int HontaiGaku { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("消費税額")]
        public double ShohizeiGaku
        {
            get
            {
                return HontaiGaku * 0.08;
            }
            set
            {
            }
        }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("売上高")]
        public double Uriage
        {
            get
            {
                return HontaiGaku + ShohizeiGaku;
            }
            set
            {
            }
        }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("源泉徴収税")]
        public double Gensen
        {
            get
            {
                return Math.Floor(HontaiGaku * 0.1021);
            }
            set
            {
            }
        }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("請求額")]
        public double SeikyuGaku
        {
            get
            {
                return HontaiGaku + ShohizeiGaku - Gensen;
            }
            set
            {
            }
        }
    }


    public class SeikyuIraiKbViewModel
    {
        [DisplayName("品目")]
        public string Himmoku { get; set; }

        [DisplayName("単価")]
        public int Price { get; set; }

        [DisplayName("数量")]
        public int Number { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}", ApplyFormatInEditMode = false)]
        [DisplayName("本体価格")]
        public int HontaiGaku { get; set; }
    }

}