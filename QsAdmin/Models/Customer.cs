using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }

        [DisplayName("取引先名")]
        public string CustomerName { get; set; }

        [DisplayName("取引先略称")]
        public string CustomerShortName { get; set; }

        [DisplayName("グループ名")]
        public string GroupName { get; set; }

        [DisplayName("会社名")]
        public string CompanyName { get; set; }

        [DisplayName("振込名")]
        public string TransferName { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("メモ")]
        public string Memo { get; set; }

        [DisplayName("削除フラグ")]
        public bool DeleteFlag { get; set; }

    }
}