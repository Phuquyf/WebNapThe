using PayCartOnline.Areas.Admin.AttributeLogin;
using PayCartOnline.Models;
using PayCartOnline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayCartOnline.Areas.Admin.Controllers
{
    public class DenominationController : Controller
    {
        HandleDenomination db = new HandleDenomination();
        // GET: Admin/Denomination
        public ActionResult Index(int? i, int? page)
        {
            if ((CheckUser)Session["Account"] != null) 
            {
                List<Denomination> denominations = db.ShowDenomination();
                ViewBag.deno = denominations;

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
                int totalPage = denominations.Count();
                float totalNumsize = (totalPage / (float)pageSize);
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.totalPage = totalPage;
                ViewBag.pageSize = pageSize;
                ViewBag.numSize = numSize;
                ViewBag.numSize = numSize;


                ViewBag.deno = denominations.OrderByDescending(x => x.ID).Skip(start).Take(pageSize);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
               
        }
        [CheckLogin]
        public ActionResult Add()
        {
            int x = 110;
                return View();
        }

        [CheckLogin]
        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult CreateOrUpdate()
        {

            var price = Convert.ToInt32(Request["Price"]);
            int status = Convert.ToInt32(Request["Status"]);
            if (Request["ID"] != null){
                var id = Int32.Parse(Request["ID"]);
                db.UpdateDenominations(id, price, status);
            }
            else
            {
                db.AddDenominations(price, status);
            }
            
            
            return RedirectToAction("Index");
        }
        public ActionResult Update()
        {
            if ((CheckUser)Session["Account"] != null)
            {
                var id = Int32.Parse(Request["id"]);
                Denomination data = db.FinDenomination(id);
                ViewBag.data = data;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
               
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.DeleteDenominations(id);
            return Content("Success");
        }
    }
}