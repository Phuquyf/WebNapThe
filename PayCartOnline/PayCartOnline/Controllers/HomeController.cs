
using PayCartOnline.Models;
using PayCartOnline.Models.VNPAY;
using PayCartOnline.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace PayCartOnline.Controllers
{
    public class HomeController : Controller
    {
        private Handle db = new Handle();
        //VnPayResponse? nong)

        public ActionResult Index( )
        {
          
            ViewBag.menhgia = db.ShowDenomination().Where(x => x.Status.Equals("1"));
            return View();
        }
        public ActionResult VnResponse(VnPayResponse vnPayResponse)
        {

            var inforOrder = (InforOrder)Session["InforOrder"];
            ViewBag.infor = inforOrder;
            ViewBag.phi = vnPayResponse;
            Pay pay = new Pay();
            var user = (CheckUser)Session["Account"];
            if (inforOrder.id_order != 0)
            {
                if (vnPayResponse.vnp_ResponseCode != "00")
                {
                    return RedirectToAction("HistoryDeal", "User");
                }
                pay.UpdateOrder(vnPayResponse, inforOrder);
            }
            else
            {
                pay.AddOrder(vnPayResponse, user, inforOrder);
            }
            Session.Remove("InforOrder");
            return View(vnPayResponse);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Register()
        {

            ViewBag.arr_phone = db.ListPhone();
            return View();
        }
        [HttpPost]
        public ActionResult Register2()
        {
            var phone = Request["phone"];
            var pass = Request["password"];
            DateTime create_at = DateTime.Now;
            db.RegisterAcc(phone, pass, create_at);
            
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Login(FormCollection data)
        {
            var phone = data["phone"];
            var password = data["password"];

            CheckUser isCheck = db.CheckUserLogin(phone, password);
            if (string.IsNullOrEmpty(isCheck.Phone))
            {
                ViewBag.phone = phone;
                ViewBag.pwd = password;
                ViewBag.error = "Phone đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
            else
            {
                isCheck.Pwd = password;
                Session["Account"] = isCheck;

                return RedirectToAction("Index", "Admin", new { Area = "Admin" });

            }


        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }        
        public string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
                    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
        public ActionResult ReceiveInfo()
        {                 
            var nhamang = Request["nhamang"];
            var type = Request["type"];
            var menhgia = Convert.ToInt32(Request["menhgia"]);
            var valueMenhgia = db.ShowDenomination().Find(c => c.ID == menhgia);
            int total = 0;
            string discount = "";
            var sdt = Int32.Parse(Request["mobile"]);
            if(nhamang.Equals("viettel"))
            {
                total = valueMenhgia.Price - (int)(valueMenhgia.Price * 2.5) / 100;
                discount = "2.5%";
            }
            else if(nhamang.Equals("mobifone"))
            {
                total = valueMenhgia.Price-(int)(valueMenhgia.Price * 4) / 100;
                discount = "4%";
            }
            else if (nhamang.Equals("vinaPhone"))
            {
                total = valueMenhgia.Price - (int)(valueMenhgia.Price * 3) / 100;
                discount = "3%";
            }
            else
            {
                total = valueMenhgia.Price - (int)(valueMenhgia.Price * 5.4) / 100;
                discount = "5.4%";
            }
            ViewBag.total = total;

            InforOrder inforOrder = new InforOrder();
            inforOrder.phone = sdt;
            inforOrder.denomination = valueMenhgia.ID;
            inforOrder.network = nhamang;
            inforOrder.CardType = type;
            inforOrder.Total = total;
            inforOrder.Discount = discount;


            Session["InforOrder"] = inforOrder;

            ViewBag.mobile = sdt;
            ViewBag.menhgia = Request["menhgia"];
             
            ViewBag.valueMenhgia = valueMenhgia;
            ViewBag.nhamang = Request["nhamang"];
            ViewBag.type = Request["type"];

            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            //if (ModelState.IsValid)
            //{
                order.OrderId = DateTime.Now.Ticks;
                order.Phone = Convert.ToInt32(Request["mobile"].ToString());
                order.Amount = Convert.ToInt32(total);
                order.OrderDescription = "aloalo thanh toan";
                //order.Amount = Convert.ToDecimal(Request.QueryString["Amount"]);
                //order.OrderDescription = Request.QueryString["OrderDescription"];
                order.CreatedDate = DateTime.Now;
            //}

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            string locale = "vn";//"en"
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }

            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            vnpay.AddRequestData("vnp_Phone", order.Phone.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", "insurance"); //default value: other
            vnpay.AddRequestData("vnp_Amount", (order.Amount*100).ToString());
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_IpAddr", GetIpAddress());
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            //if (bank.SelectedItem != null && !string.IsNullOrEmpty(bank.SelectedItem.Value))
            //{
            //    vnpay.AddRequestData("vnp_BankCode", bank.SelectedItem.Value);
            //}
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            ViewBag.demoUrl = paymentUrl;
            return View();
        }

       //string menhgia,string moblie,string nhamang,string type
        public ActionResult RePay(string moblie,string id)
        {
            var id_order = Int32.Parse( Request["id"]);
            Order currentOrder = db.SearchOrderByOrderID(id_order);

            InforOrder inforOrder = new InforOrder();
            
           
            inforOrder.id_order = id_order;

            Session["InforOrder"] = inforOrder;

            ViewBag.mobile = currentOrder.Phone;
            ViewBag.menhgia = currentOrder.Price;
            ViewBag.discount = currentOrder.Price - currentOrder.Total;
            ViewBag.total = currentOrder.Total;
            ViewBag.nhamang = currentOrder.Brand;
            ViewBag.type = currentOrder.CardType;

            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

            //Get payment input
            OrderInfo order = new OrderInfo();
            
            order.OrderId = DateTime.Now.Ticks;
            order.Phone = Convert.ToInt32(currentOrder.Phone);
            order.Amount = Convert.ToInt32(currentOrder.Total);
            order.OrderDescription = "aloalo thanh toan";          
            order.CreatedDate = DateTime.Now;
            

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            string locale = "vn";//"en"
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }

            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            vnpay.AddRequestData("vnp_Phone", order.Phone.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", "insurance"); //default value: other
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_IpAddr", GetIpAddress());
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));        
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            ViewBag.demoUrl = paymentUrl;
            return View();
        }
        public ActionResult Tutorial()
        {
            return View();
        }

        public ActionResult CancelOrder()
        {
            Pay db = new Pay();
            int id = Int32.Parse(Request["id"]);
            int status = Int32.Parse(Request["status"]);
            string _status= status == 0 ? "Hủy" : "";
           
            db.CancelOrders(_status, id);
            return RedirectToAction("HistoryDeal", "User");
        }
    }
}