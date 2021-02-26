using PayCartOnline.Models;
using PayCartOnline.Models.VNPAY;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PayCartOnline.Service
{
    public class Pay
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DuAn"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DuAn"].ConnectionString);
        private const string InsertOrder = "InsertOrder";
        private const string UpdateOrders = "UpdateOrders";
        private const string CancelOrder = "CancelOrder";
        
        public void AddOrder(VnPayResponse vnPayResponse, CheckUser user, InforOrder order)
        {

            var connection = new SqlConnection(connectionString);
            string mobile = order.phone.ToString();
            DateTime date = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss" , CultureInfo.InvariantCulture);
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = InsertOrder;

                var status = vnPayResponse.vnp_ResponseCode == "00" ? "Thành Công" : "Chưa Thanh Toán";

                string code_order = "#" + DateTime.Now.ToBinary().ToString() + user.ID_User;

                command.Parameters.Add(new SqlParameter("@USER_ID", user.ID_User));
                command.Parameters.Add(new SqlParameter("@Code", code_order));
                command.Parameters.Add(new SqlParameter("@TYPEPAY",1));
                command.Parameters.Add(new SqlParameter("@CARDTYPE", order.CardType));
                command.Parameters.Add(new SqlParameter("@DENOMINATION_ID", order.denomination));
                command.Parameters.Add(new SqlParameter("@PHONE", mobile));
                command.Parameters.Add(new SqlParameter("@BRAND", order.network));
                command.Parameters.Add(new SqlParameter("@QUANTITY", 1));
                command.Parameters.Add(new SqlParameter("@TOTAL",vnPayResponse.vnp_Amount/100));
                command.Parameters.Add(new SqlParameter("@DISCOUNT",1));
                command.Parameters.Add(new SqlParameter("@STATUS", status));
                command.Parameters.Add(new SqlParameter("@BANKCODE",vnPayResponse.vnp_BankCode));
                command.Parameters.Add(new SqlParameter("@CREATED_AT",date));
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception e)
            {
                //fix not ho em nhe , dang bi loi convert datetime nua thoi may th kia em fix het rôi
                Console.WriteLine(e.Message);
            }
        }
        public void UpdateOrder(VnPayResponse vnPayResponse, InforOrder order)
        {

            var connection = new SqlConnection(connectionString);
            
            DateTime date = DateTime.ParseExact(vnPayResponse.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = UpdateOrders;

                var status = vnPayResponse.vnp_ResponseCode == "00" ? "Thành Công" : "Chưa Thanh Toán";

                command.Parameters.Add(new SqlParameter("@order_id", order.id_order));
                
                command.Parameters.Add(new SqlParameter("@status", status));
                
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public void CancelOrders(string status,int id_order)
        {

            var connection = new SqlConnection(connectionString);

          
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = CancelOrder;

               

                command.Parameters.Add(new SqlParameter("@order_id", id_order));

                command.Parameters.Add(new SqlParameter("@status", status));

                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}