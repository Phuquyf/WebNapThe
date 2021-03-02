using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayCartOnline.Models
{
    public class Denomination
    {
        
        public int ID { get; set; }
        
        public int Price { get; set; }
        public string Status { get; set; }
        public int Display { get; set; }
    }
}