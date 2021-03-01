using OfficeOpenXml;
using OfficeOpenXml.Style;
using PayCartOnline.Areas.Admin.AttributeLogin;
using PayCartOnline.Models;
using PayCartOnline.Service;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayCartOnline.Controllers
{
    public class UserController : Controller
    {
        Handle db = new Handle();
        // GET: User
        public ActionResult Index()
        {
            if ((CheckUser)Session["Account"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult AccountUser()
        {
            if ((CheckUser)Session["Account"] != null)
            {
                CheckUser current = (CheckUser)Session["Account"];


                Users us = db.FindAccByID2(current.ID_User);

                return View(us);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpGet]
        //[CheckLogin]
        public ActionResult HistoryDeal(DateTime? startDate, DateTime? expirationDate, int? typePay,int? status)
        {
            if ((CheckUser)Session["Account"] != null)
            {
                CheckUser current = (CheckUser)Session["Account"];
                if (startDate != null || expirationDate != null || typePay != null || status != null)
                {
                    SearchHistory search = new SearchHistory
                    { ID_Acc = current.ID_User, StartDate = startDate, ExpirationDate = expirationDate, TypePay = typePay ,Status= status};

                    List<Order> data = db.SearchHistory(search);
                    ViewBag.startDate = startDate;
                    ViewBag.expirationDate = expirationDate;
                    ViewBag.typePay = typePay;
                    ViewBag.orders = data;
                    ViewBag.count = data.Count;
                    return Json(data,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var orders = db.GetOrderByIDAcc(current.ID_User);
                    ViewBag.orders = orders;
                    ViewBag.count = orders.Count;
                    return View();
                }
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public ActionResult AccountUser(FormCollection fc)
        {
            if ((CheckUser)Session["Account"] != null)
            {
                CheckUser currentUser = (CheckUser)Session["Account"];
                var record = new Users();
                record.ID = currentUser.ID_User;
                record.FullName = fc["fullname"].Trim();
                record.Address = fc["address"].Trim();
                record.Birthday = fc["birthday"].Trim();
                record.Gender = fc["gender"].Trim().Equals("Nam") ? 1 : 2;
                record.Identity_people = Int32.Parse(fc["cmnd"].Trim());

                if (ModelState.IsValid)
                {
                    db.UpdateInformationUser(record);
                    return RedirectToAction("AccountUser");
                }
                return View(record);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }


        [HttpGet]

        public ActionResult DetailsOrder(int id)
        {
            if ((CheckUser)Session["Account"] != null)
            {
                Order order = db.SearchOrder(id);
                ViewBag.order = order;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult ExportPDF(int id)
        {
            Order order = db.SearchOrder(id);
            try {
                return new ActionAsPdf("DetailsOrder", "User")
                {
                    FileName = Server.MapPath("~/Content/ListOrder.pdf")
                };
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
           

        } 
        public ActionResult ExportExcel(int id)
        {
            if ((CheckUser)Session["Account"] != null)
            {
                Order order = db.SearchOrder(id);
                try
                {
                    //handle Export excel
                    ExcelPackage excel = new ExcelPackage();
                    excel.Workbook.Properties.Title = "Đơn hàng chi tiết";
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    // Set default width cho tất cả column
                    workSheet.DefaultColWidth = 25;
                    // Tự động xuống hàng khi text quá dài
                    workSheet.Cells.Style.WrapText = true;
                    //Header of table  
                    //  
                    workSheet.Name = "Đơn hàng chi tiết";
                    workSheet.Workbook.Properties.Title = "Đơn hàng chi tiết";

                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[1, 1].Value = "STT";
                    workSheet.Cells[1, 2].Value = "Mã đơn hàng";
                    workSheet.Cells[1, 3].Value = "Phone";
                    workSheet.Cells[1, 4].Value = "Nhà mạng";
                    workSheet.Cells[1, 5].Value = "Mệnh giá";
                    workSheet.Cells[1, 6].Value = "Loại thẻ";
                    workSheet.Cells[1, 7].Value = "Phương thức thanh toán";
                    workSheet.Cells[1, 8].Value = "Ngân hàng";
                    workSheet.Cells[1, 9].Value = "Ngày mua";
                    workSheet.Cells[1, 10].Value = "Tổng tiền";
                    int recordIndex = 2;

                    workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                    workSheet.Cells[recordIndex, 2].Value = order.Code_Order;
                    workSheet.Cells[recordIndex, 3].Value = order.Phone;
                    workSheet.Cells[recordIndex, 4].Value = order.Brand;
                    workSheet.Cells[recordIndex, 5].Value = order.Price;

                    workSheet.Cells[recordIndex, 6].Value = order.CardType;
                    workSheet.Cells[recordIndex, 7].Value = "Vn Pay";
                    workSheet.Cells[recordIndex, 8].Value = order.BankCode;
                    workSheet.Cells[recordIndex, 9].Value = order.Create_At.ToString("dd/MM/yyyy");
                    workSheet.Cells[recordIndex, 10].Value = order.Total;

                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    workSheet.Column(6).AutoFit();
                    workSheet.Column(7).AutoFit();
                    workSheet.Column(8).AutoFit();
                    workSheet.Column(9).AutoFit();
                    workSheet.Column(10).AutoFit();
                    string excelName = "TableOrder";
                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}