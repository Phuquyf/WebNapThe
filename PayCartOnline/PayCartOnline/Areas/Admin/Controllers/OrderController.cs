
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PayCartOnline.Models;
using PayCartOnline.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
                if (startDate != null || expiration != null || status != null || phone != null || id_denomination != null)
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
        public ActionResult UpdateOrder(int status, string comment, int idOrder)
        {
            db.UpdateOrder(comment, idOrder, status);
            return RedirectToAction("Index", "Order");
        }
        public ActionResult ExportExcel(DateTime? startDate, DateTime? expiration, int? status, int? phone, int? id_denomination)
        {
            SearchHistory search = new SearchHistory()
            { StartDate = startDate, ExpirationDate = expiration, Status = status, Phone = phone, Id_denomination = id_denomination };

            List<Order> orders = db.SearchOrder(search);
            HandleExportExcel(orders);
            return View();
        }
        public void HandleExportExcel(List<Order> orders)
        {
               
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

                //
                foreach (var order in orders)
                {
                    
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
                    recordIndex++;
                }
                   

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
            
        }
    }
