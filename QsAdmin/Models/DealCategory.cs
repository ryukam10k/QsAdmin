using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class DealCategory
    {
        public string DealCategoryId { get; set; }

        [DisplayName("取引区分")]
        public string DealCategoryName { get; set; }

        [DisplayName("基本単価")]
        public int? BasicPrice { get; set; }
    }
}