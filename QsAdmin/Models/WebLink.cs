using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class WebLink
    {
        public int Id { get; set; }

        public string AccountId { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public virtual ApplicationUser Account { get; set; }
    }
}