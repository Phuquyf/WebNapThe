using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayCartOnline.Models
{
    public class InforOrder
    {
        public int id_order { get; set; }
        public int phone { get; set; }
        public int denomination { get; set; }
        public string network { get; set; }
        public int TypePay { get; set; }
        public string CardType { get; set; }
        public int Total { get; set; }
        public string Discount { get; set; }
    }
}