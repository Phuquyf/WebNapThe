﻿using PayCartOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PayCartOnline.Service
{
    public class HandleOrder
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["DuAn"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DuAn"].ConnectionString);

        private const string TakeAllOrder = "AllOrder";
        private const string Search = "SearchOrderAdmin";
        private const string updateOrder = "updateOrder";

        public List<Order> ListOrder()
        {
            SqlCommand com = new SqlCommand(TakeAllOrder, con);
            com.CommandType = CommandType.StoredProcedure;

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

                record.CardType = item["CardType"] != null ? item["CardType"].ToString() : null;
                record.BankCode = item["BankCode"] != null ? item["BankCode"].ToString() : null;
                record.Create_At = DateTime.Parse(item["Create_At"].ToString());
                record.Status = item["Status"] != null ? item["Status"].ToString() : null;

                data.Add(record);
            }

            return data;

        }

        public List<Order> SearchOrder(SearchHistory search)
        {
            var status = "";
            if (search.Status != 0 && search.Status != null)
            {
                if(search.Status == 2) 
                {
                    status = "Hủy";
                }
                else { status = search.Status == 1 ? "Thành Công" : "Chưa Thanh Toán"; }
                
            }
            else
            {
                status = null;
            }

            SqlCommand com = new SqlCommand(Search, con);
            com.CommandType = CommandType.StoredProcedure;
            

            com.Parameters.AddWithValue("@startDate", System.Data.SqlDbType.DateTime).Value = search.StartDate == null ? DBNull.Value : (object)search.StartDate;


            com.Parameters.AddWithValue("@expirationDate", System.Data.SqlDbType.DateTime).Value = search.ExpirationDate == null ? DBNull.Value : (object)search.ExpirationDate;
            com.Parameters.AddWithValue("@id_denomination", System.Data.SqlDbType.Int).Value = search.Id_denomination == 0 ? DBNull.Value : (object)search.Id_denomination;
            com.Parameters.AddWithValue("@phone", System.Data.SqlDbType.Int).Value = search.Phone == 0 ? DBNull.Value : (object)search.Phone;
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
                record.Create_At = DateTime.Parse(item["Create_At"].ToString());
                record.ID_Denomination = Int32.Parse(item["ID_Denomination"].ToString());

                data.Add(record);
            }

            return data;

        }

        public void UpdateOrder(string comment, int id_order,int status)
        {
            var status2 = "";
            var connection = new SqlConnection(connectionString);
            
            if (status != 0)
            {
                if (status == 2)
                {
                    status2 = "Hủy";
                }
                else { status2 = status == 1 ? "Thành Công" : "Chưa Thanh Toán"; }

            }
            else
            {
                status2 = null;
            }

            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = updateOrder;

                command.Parameters.AddWithValue("@status", System.Data.SqlDbType.NVarChar).Value = String.IsNullOrEmpty(status2) ? DBNull.Value : (object)status2;

                command.Parameters.AddWithValue("@order_id", System.Data.SqlDbType.Int).Value = id_order ==0 ? DBNull.Value : (object)id_order;


                command.Parameters.AddWithValue("@comment", System.Data.SqlDbType.NVarChar).Value = String.IsNullOrEmpty(comment) ? DBNull.Value : (object)comment;

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