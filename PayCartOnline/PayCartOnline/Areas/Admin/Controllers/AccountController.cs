using PayCartOnline.Models;
using PayCartOnline.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PayCartOnline.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        Handle db = new Handle();
        // GET: Admin/Account
        public ActionResult Index(int? i, int? page)
        {
            CheckUser currentUser = (CheckUser)Session["Account"];
            if (currentUser != null)
            {
                ViewBag.current = currentUser;
                List<CheckUser> list_account = db.GetAllAcount();
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
                int totalPage = list_account.Count();
                float totalNumsize = (totalPage / (float)pageSize);
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.totalPage = totalPage;
                ViewBag.pageSize = pageSize;
                ViewBag.numSize = numSize;
                ViewBag.numSize = numSize;


                ViewBag.list_account = list_account.OrderByDescending(x => x.ID_User).Skip(start).Take(pageSize);

                return View(list_account);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
            
        }


        public ActionResult Add()
        {
            ViewBag.arr_phone = db.ListPhone();
            List<Roles> list_role = db.ListRole();
            return View(list_role);
        }

        public ActionResult Update()
        {
            var id = Request["id"];

            dynamic data = new ExpandoObject();

            data.list_role = db.ListRole();

            int id_user = Int32.Parse(id);
            data.account = db.FindAccByID(id_user);
            return View(data);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update2(CheckUser user)
        {
            if (user.ID_User is null)
            {
                user.Create_At = DateTime.Now;
                db.AddAcc(user);
            }
            else
            {
                db.UpdateUser(user);
            }

            int x = 1;
            return RedirectToAction("Index");
        }
    }
}