using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class Seikyu
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string KeijoTsuki { get; set; }

        public int Number { get; set; }

        public int Gokei { get; set; }

        public double ShohizeiGaku { get; set; }

        public double Gensen { get; set; }

        public double SeikyuGaku { get; set; }

        public DateTime RegistDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}