using PayCartOnline.Models;
using PayCartOnline.Models.VNPAY;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PayCartOnline.Service
{
    public class Handle
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DuAn"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DuAn"].ConnectionString);

        private const string GetAllDenomination = "GetAllDenomination";
        private const string CheckUser = "CheckUser";
        private const string GetAllAccount = "GetAllAccount";
        private const string GetAccById = "GetAccById";
        private const string GetAccById2 = "TakeAccById";
        private const string CheckPhone = "CheckPhone";
        private const string AllRole = "AllRole";
        private const string UpdateAccount = "UpdateAccount";
        private const string InsertAcc = "InsertUser";
        private const string Register = "Register";
        private const string InsertOrder = "InsertOrder";
        private const string UpdateInformationCustomer = "UpdateInformationCustomer";
        private const string AllOrderByID = "TableOrder";
        private const string Search = "Search";
        private const string SearchOrderByID = "SearchOrderByID";
        


        

        public Order SearchOrder(int id)
        {
            SqlCommand com = new SqlCommand(SearchOrderByID, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID", System.Data.SqlDbType.Int).Value = id == 0 ? DBNull.Value : (object)id;

           

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Order> data = new List<Order>();
            Order record = new Order();
            foreach (DataRow item in ds.Rows)
            {
                
                record.Id_order = Int32.Parse(item["ID_Order"].ToString());
                record.Code_Order = item["Code_Order"] != null ? item["Code_Order"].ToString() : null;
                record.Phone = item["Phone"] != null ? Int32.Parse(item["Phone"].ToString()) : 0;
                record.Brand = item["Brand"] != null ? item["Brand"].ToString() : null;
                record.Total = item["Total"] != null ? Convert.ToInt32(item["Total"].ToString()) : 0;
                record.Price = item["Price"] != null ? Convert.ToInt32(item["Price"].ToString()) : 0;
                record.CardType = item["CardType"] != null ? item["CardType"].ToString() : null;
                record.BankCode = item["BankCode"] != null ? item["BankCode"].ToString() : null;
                record.Create_At =  DateTime.Parse(item["Create_At"].ToString());
                record.Status = item["Status"].ToString();
                
            }

            return record;

        }
        public List<Order> SearchHistory(SearchHistory search)
        {
            var status = "";
            if (search.Status != 0)
            {
                if(search.Status== -1) { status = "Hủy"; }
                else
                {
                    status = search.Status == 1 ? "Thành Công" : "Chưa Thanh Toán";
                }
                
            }
            else
            {
                status = null;
            }
            
            SqlCommand com = new SqlCommand(Search, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ID_Acc", System.Data.SqlDbType.Int).Value = search.ID_Acc == 0 ? DBNull.Value : (object)search.ID_Acc;
            
            com.Parameters.AddWithValue("@startDate", System.Data.SqlDbType.DateTime).Value = search.StartDate == null ? DBNull.Value : (object)search.StartDate;

          
            com.Parameters.AddWithValue("@expirationDate", System.Data.SqlDbType.DateTime).Value = search.ExpirationDate == null ? DBNull.Value : (object)search.ExpirationDate;

            com.Parameters.AddWithValue("@typePay", System.Data.SqlDbType.Int).Value = search.TypePay == 0 ? DBNull.Value : (object)search.TypePay;
            com.Parameters.AddWithValue("@status", System.Data.SqlDbType.NVarChar).Value = String.IsNullOrEmpty(status) ? DBNull.Value : (object)status;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Order> data = new List<Order>();
            foreach (DataRow item in ds.Rows)
            {
                Order record = new Order();
                record.Id_order = Int32.Parse(item["ID_Order"].ToString());
                record.Code_Order = item["Code_Order"] != null ? item["Code_Order"].ToString() : null;
                record.Phone = item["Phone"] != null ? Int32.Parse(item["Phone"].ToString()) : 0;
                record.Brand = item["Brand"] != null ? item["Brand"].ToString() : null;
                record.Total = item["Total"] != null ? Convert.ToInt32(item["Total"].ToString()) : 0;
                record.Price = item["Price"] != null ? Convert.ToInt32(item["Price"].ToString()) : 0;
                record.CardType = item["CardType"] != null ? item["CardType"].ToString() : null;
                record.BankCode = item["BankCode"] != null ? item["BankCode"].ToString() : null;
                record.Status = item["Status"] != null ? item["Status"].ToString() : null;
                record.Create_At =  DateTime.Parse(item["Create_At"].ToString());
                record.ID_Denomination = Int32.Parse(item["ID_Denomination"].ToString());

                data.Add(record);
            }

            return data;

        }

        public List<Order> GetOrderByIDAcc(int? ID)
        {
            SqlCommand com = new SqlCommand(AllOrderByID, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@ID_Acc", ID));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Order> data = new List<Order>();
            foreach (DataRow item in ds.Rows)
            {
                Order record = new Order();
                record.Id_order = Int32.Parse(item["ID_Order"].ToString());
                record.Code_Order = item["Code_Order"] != null ? item["Code_Order"].ToString() : null;
                record.Phone = item["Phone"] != null ? Int32.Parse(item["Phone"].ToString()) : 0;
                record.Brand = item["Brand"] != null ? item["Brand"].ToString() : null;
                record.Total = item["Total"] != null ? Convert.ToInt32(item["Total"].ToString()) : 0;
                record.Price = item["Price"] != null ? Convert.ToInt32(item["Price"].ToString()) : 0;
                record.CardType = item["CardType"] != null ? item["CardType"].ToString() : null;
                record.BankCode = item["BankCode"] != null ? item["BankCode"].ToString() : null;
                record.Create_At =  DateTime.Parse(item["Create_At"].ToString());
                record.Status = item["Status"] != null ? item["Status"].ToString() : null;
                record.ID_Denomination = Int32.Parse(item["ID_Denomination"].ToString());
                data.Add(record);
            }

            return data;

        }

        /// <summary>
        /// get data all table denomination
        /// </summary>
        /// <returns></returns>

        public List<Denomination> ShowDenomination()
        {
            SqlCommand com = new SqlCommand(GetAllDenomination, con);
            com.CommandType = CommandType.StoredProcedure;
           
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Denomination> data = new List<Denomination>();
            foreach (DataRow item in ds.Rows)
            {
                Denomination record = new Denomination();
                record.ID = item["ID_Denomination"] != null ? Int32.Parse(item["ID_Denomination"].ToString()) : 0;
                record.Price = item["Price"] != null ? Int32.Parse(item["Price"].ToString()) : 0;
               
                data.Add(record);
            }

            return data;

        }

        /// <summary>
        /// check user access
        /// </summary>
        /// <returns> string name role</returns>
        public CheckUser CheckUserLogin(string phone,string pwd)
        {
            SqlCommand com = new SqlCommand(CheckUser, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@Phone", phone));
            com.Parameters.Add(new SqlParameter("@Pass", pwd));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            
            CheckUser user = new CheckUser();
            foreach (DataRow item in ds.Rows)
            {
                user.ID_User = Int32.Parse(item["User_ID"].ToString());
                user.Phone = string.IsNullOrEmpty(item["PhoneUser"].ToString()) ? null : item["PhoneUser"].ToString();
                user.Role = string.IsNullOrEmpty(item["Name"].ToString()) ? null : item["Name"].ToString();
                user.UserName = string.IsNullOrEmpty(item["UserName"].ToString()) ? null : item["UserName"].ToString();
            }

            return user;

        }

        public List<CheckUser> GetAllAcount()
        {
            SqlCommand com = new SqlCommand(GetAllAccount, con);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<CheckUser> data = new List<CheckUser>();
            foreach (DataRow item in ds.Rows)
            {
                CheckUser record = new CheckUser();
                record.Phone = string.IsNullOrEmpty(item["PhoneUser"].ToString()) ? null : item["PhoneUser"].ToString();
                record.Role = string.IsNullOrEmpty(item["Name"].ToString()) ? null : item["Name"].ToString();
                record.UserName = string.IsNullOrEmpty(item["UserName"].ToString()) ? null : item["UserName"].ToString();
                record.Pwd = string.IsNullOrEmpty(item["Password"].ToString()) ? null : item["Password"].ToString();
                record.ID_User = item["ID"].ToString() == null ? 0 : Int32.Parse(item["ID"].ToString());
                record.Status = string.IsNullOrEmpty(item["Status"].ToString()) ? null : item["Status"].ToString();
                data.Add(record);
            }

            return data;

        }

        public CheckUser FindAccByID(int id)
        {
            SqlCommand com = new SqlCommand(GetAccById, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@ID_User", id));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            DataRow dr = ds.NewRow();
            if (ds.Rows.Count > 0)
                dr = ds.Rows[0];
            
                CheckUser record = new CheckUser();
                record.Phone = string.IsNullOrEmpty(dr["PhoneUser"].ToString()) ? null : dr["PhoneUser"].ToString();
                record.Role = string.IsNullOrEmpty(dr["Name"].ToString()) ? null : dr["Name"].ToString();
                record.UserName = string.IsNullOrEmpty(dr["UserName"].ToString()) ? null : dr["UserName"].ToString();
                record.Pwd = string.IsNullOrEmpty(dr["Password"].ToString()) ? null : dr["Password"].ToString();
                record.ID_User = dr["ID"].ToString() == null ? 0 : Int32.Parse(dr["ID"].ToString());
                record.Status = string.IsNullOrEmpty(dr["Status"].ToString()) ? null : dr["Status"].ToString();
                
            

            return record;

        }
        public Users FindAccByID2(int? id)

        {
            SqlCommand com = new SqlCommand(GetAccById2, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@ID", id));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            DataRow dr = ds.NewRow();
            if (ds.Rows.Count > 0)
                dr = ds.Rows[0];

            Users record = new Users();
            record.FullName = string.IsNullOrEmpty(dr["FullName"].ToString()) ? null : dr["FullName"].ToString();
            record.Address = string.IsNullOrEmpty(dr["Address"].ToString()) ? null : dr["Address"].ToString();
            record.Birthday = string.IsNullOrEmpty(dr["Birthday"].ToString()) ? null : dr["Birthday"].ToString();
            record.Gender = string.IsNullOrEmpty(dr["Gender"].ToString()) ? 0 : Int32.Parse(dr["Gender"].ToString());
            record.Identity_people = string.IsNullOrEmpty(dr["Identity_People"].ToString()) ? 0 : Int32.Parse(dr["Identity_People"].ToString());
            record.ID = Int32.Parse(dr["ID"].ToString());
            
            return record;

        }

        public List<string> ListPhone()
        {
            SqlCommand com = new SqlCommand(GetAllAccount, con);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<string> data = new List<string>();
            foreach (DataRow item in ds.Rows)
            {
                CheckUser record = new CheckUser();
                data.Add(item["PhoneUser"].ToString() == null ? null : item["PhoneUser"].ToString());
                
                
            }
            return data;

        }

        public List<Roles> ListRole()
        {
            SqlCommand com = new SqlCommand(AllRole, con);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable ds = new DataTable();
            da.Fill(ds);
            List<Roles> data = new List<Roles>();
            foreach (DataRow item in ds.Rows)
            {
                Roles record = new Roles();
                record.ID=(item["ID_Role"].ToString() == null ? 0 : Int32.Parse(item["ID_Role"].ToString()));
                record.Name = (item["Name"].ToString() == null ? null : item["Name"].ToString());
                data.Add(record);
            }
            return data;

        }

        public void UpdateUser(CheckUser user)
        {

            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = UpdateAccount;
                command.Parameters.Add(new SqlParameter("@ID_user", user.ID_User));
                command.Parameters.Add(new SqlParameter("@Phone", user.Phone));
                
                command.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                command.Parameters.Add(new SqlParameter("@Status", user.Status));
                command.Parameters.Add(new SqlParameter("@Role", user.Role));
                int ID = command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddAcc(CheckUser user)
        {

            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = InsertAcc;
                
                command.Parameters.Add(new SqlParameter("@Phone", user.Phone));
                command.Parameters.Add(new SqlParameter("@Pass", user.Pwd));
                command.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                command.Parameters.Add(new SqlParameter("@Status", user.Status));
                command.Parameters.Add(new SqlParameter("@Role_Id", user.Role));
                command.Parameters.Add(new SqlParameter("@Create_At", user.Create_At));
                int ID = command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
      

        public void RegisterAcc(string phone,string pwd,DateTime date)
        {

            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Register;

                command.Parameters.Add(new SqlParameter("@Phone", phone));
                command.Parameters.Add(new SqlParameter("@Pass", pwd));
                command.Parameters.Add(new SqlParameter("@Create_At", date));
                int ID = command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

       //cap nhat thông tin tài khoảng user
       public void UpdateInformationUser(Users user)
        {
            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = UpdateInformationCustomer;
                command.Parameters.Add(new SqlParameter("@ID_user", user.ID));
                command.Parameters.Add(new SqlParameter("@FullName", user.FullName));
                command.Parameters.Add(new SqlParameter("@Address", user.Address));
                command.Parameters.Add(new SqlParameter("@Identity_people", user.Identity_people));
                command.Parameters.Add(new SqlParameter("@Gender", user.Gender));
                command.Parameters.Add(new SqlParameter("@Birthday", user.Birthday));
                int ID = command.ExecuteNonQuery();
                connection.Close();
               
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
        }

    }
}