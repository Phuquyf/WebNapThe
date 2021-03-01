using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayCartOnline.Models
{
    public class Order
    {
        public int Id_order { get; set; }
        [DisplayName("Mã đơn hàng")]
        public string Code_Order { get; set; }
        [DisplayName("Số điện thoại nạp")]
        public int Phone { get; set; }
        [DisplayName("Nhà mạng")]
        public string Brand { get; set; }
        [DisplayName("Tổng tiền")]
        public int Total { get; set; }
        [DisplayName("Loại thẻ")]
        public string CardType { get; set; }
        public string BankCode { get; set; }
        [DisplayName("Ngân hàng")]
        public DateTime Create_At { get; set; }
        [DisplayName("Mệnh giá thẻ")]
        public int Price { get; set; }
        [DisplayName("Trạng thái")]
        public string Status { get; set; }
        public int ID_Denomination { get; set; }
        public int ID_User { get; set; }
        public string Comment { get; set; }
        public string Discount { get; set; }
    }
}