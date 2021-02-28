
using PayCartOnline.Models;
using PayCartOnline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayCartOnline.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        HandleOrder db = new HandleOrder();
        Handle sv = new Handle();
        // GET: Admin/Order
        public ActionResult Index(int? i, int? page, DateTime? startDate, DateTime? expiration, int? status, int? phone, int? id_denomination)
        {
            List<Denomination> denominations = sv.ShowDenomination();
            ViewBag.denomination = denominations;
            List<Order> orders = new List<Order>();
            if ((CheckUser)Session["Account"] != null)
            {   
                if(startDate != null || expiration != null || status != null|| phone != null || id_denomination != null)
                {

                    SearchHistory search = new SearchHistory()
                    { StartDate = startDate, ExpirationDate = expiration, Status = status, Phone = phone, Id_denomination = id_denomination };
                    orders = db.SearchOrder(search);
                    ViewBag.startDate = startDate;
                    ViewBag.expiration = expiration;
                    ViewBag.status = status;
                    ViewBag.phone = phone;
                    ViewBag.id_denomination = id_denomination;
                }
                else
                {
                    orders = db.ListOrder();
                }
               

                int total = 0;
                int orderSuccess = 0;
                int order_Error = 0;
                //List<Order> orders = db.ListOrder();
                
                int countOrder = orders.Count;
                foreach (var item in orders)
                {

                    total += item.Total;
                    _ = item.Status.Equals("Thành Công") ? orderSuccess++ : order_Error++;
                }
                ViewBag.orderSuccess = orderSuccess;
                ViewBag.order_Error = order_Error;
                ViewBag.total = total;
                ViewBag.count = countOrder;

                //
                int pageSize = 5;

                if (page > 0)
                {
                    page = page;
                }
                else
                {
                    page = 1;
                }
                int start = (int)(page - 1) * pageSize;

                ViewBag.pageCurrent = page;
                int totalPage = orders.Count();
                float totalNumsize = (totalPage / (float)pageSize);
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.totalPage = totalPage;
                ViewBag.pageSize = pageSize;
                ViewBag.numSize = numSize;
                ViewBag.numSize = numSize;
                ViewBag.orders = orders.OrderByDescending(x => x.Id_order).Skip(start).Take(pageSize);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
            
        }

       public ActionResult OrderDetail()
        {
            int id = Int32.Parse(Request["id"]);
            Order order = sv.SearchOrder(id);
            
            ViewBag.order = order;
            return View();
        }
        public ActionResult UpdateOrder(int status,string comment,int idOrder)
        {
            db.UpdateOrder(comment, idOrder, status);
            return RedirectToAction("Index","Order");
        }
    }
}