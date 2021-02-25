using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayCartOnline.Models
{
    public class SearchHistory
    {
        public int? ID_Acc { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? TypePay { get; set; }
        public int? Status { get; set; }
        public int? Phone { get; set; }
        public int? Id_denomination { get; set; }



    }
}