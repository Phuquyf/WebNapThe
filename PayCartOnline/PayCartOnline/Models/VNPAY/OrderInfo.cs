﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayCartOnline.Models.VNPAY
{
    public class OrderInfo
    {
        public decimal OrderId { get; set; }
        /// <summary>
        /// Payment amount
        /// </summary>     
       
        public int Phone { get; set; }
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }

        public string BankCode { get; set; }

        /// <summary>
        /// Order Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Creaed date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// VNPAY Transaction Id
        /// </summary>
        public decimal vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }
    }
}