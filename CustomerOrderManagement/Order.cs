using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CustomerOrderManagement
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }

        SqlCommand _command;
        string connectionString = Program.ConnectionString;


        //TODO: check for invalid customerId
        //public void CreateOrder(int customerId)
        //{
        //    string orderNumber = RandomGenerator.GenerateUniqueOrderNumber();
        //    string orderStatus = "Ordered";
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            if (new Customer().FetchCustomerById(customerId) == null)
        //            {
        //                Console.WriteLine("Customer Id does not exist!!");
        //                return;
        //            }

        //            _command = new SqlCommand("insert into orders(customerId,orderNumber,status) values (@customerId,@orderNumber,@status)", conn);
        //            _command.Parameters.AddWithValue("customerId", customerId);
        //            _command.Parameters.AddWithValue("orderNumber", orderNumber);
        //            _command.Parameters.AddWithValue("status", orderStatus);
        //            conn.Open();
        //            int rowsAffected = _command.ExecuteNonQuery();
        //            if (rowsAffected > 0)
        //            {
        //                Order order = FetchOrderByOrderNumer(orderNumber);
        //                Console.WriteLine("Order created and your order ID is " + order.Id);
        //                Console.WriteLine("Order created and your order number is " + orderNumber);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Internal error while creating order!!!");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error occured: " + ex.Message);
        //        }
        //        finally
        //        {
        //            if (_command != null)
        //                _command.Dispose();
        //        }
        //    }
        //}
        public void CreateOrder(int customerId)
        {
            string orderNumber = RandomGenerator.GenerateUniqueOrderNumber();
            string orderStatus = "Ordered";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (new Customer().FetchCustomerById(customerId) == null)
                    {
                        Console.WriteLine("Customer Id does not exist!!");
                        return;
                    }

                    _command = new SqlCommand("sp_insertRecordsToOrders", conn);
                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@CustomerId", customerId);
                    _command.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    _command.Parameters.AddWithValue("@Status", orderStatus);
                    SqlParameter orderId = new SqlParameter()
                    {
                        ParameterName = "@OrderID",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    _command.Parameters.Add(orderId);
                    conn.Open();
                    int rowsAffected = _command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Order created and your order ID is " + orderId.Value.ToString());
                        Console.WriteLine("Order created and your order number is " + orderNumber);
                    }
                    else
                    {
                        Console.WriteLine("Internal error while creating order!!!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
                finally
                {
                    if (_command != null)
                        _command.Dispose();
                }
            }
        }
        public List<Order> FetchAllOrderRecords()
        {
            List<Order> listOfOrders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("select * from orders", conn);
                    conn.Open();
                    SqlDataReader reader = _command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        Order order = new Order();
                        order.Id = (int)reader["id"];
                        order.OrderNumber = (string)reader["OrderNumber"];
                        order.CustomerId = reader["CustomerId"]!=DBNull.Value? (int)reader["CustomerId"]:-1;
                        order.OrderDate = reader["OrderDate"].ToString();
                        order.OrderStatus = (string)reader["status"];
                        listOfOrders.Add(order);
                    }
                    return listOfOrders;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
                finally
                {

                    if (_command != null)
                        _command.Dispose();
                }
                return null;
            }
        }
        public Order FetchOrderByOrderNumer(string orderNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                Order order = new Order();
                try
                {
                    _command = new SqlCommand("select * from orders where orderNumber=@orderNumber", conn);
                    _command.Parameters.AddWithValue("orderNumber", orderNumber);
                    conn.Open();
                    SqlDataReader reader = _command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        order.Id = (int)reader["id"];
                        order.OrderNumber = (string)reader["OrderNumber"];
                        order.CustomerId = (int)reader["CustomerId"];
                        order.OrderDate = reader["OrderDate"].ToString();
                        order.OrderStatus = (string)reader["status"];
                    }
                    return order;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                    return null;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }
        public Order FetchOrderById(int orderId)
        {
            Order order = new Order();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("select * from orders where ID=@ID", conn);
                    _command.Parameters.AddWithValue("ID", orderId);
                    conn.Open();
                    SqlDataReader reader = _command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        order.Id = (int)reader["id"];
                        order.OrderNumber = (string)reader["OrderNumber"];
                        order.CustomerId = reader["CustomerId"]!=DBNull.Value? (int)reader["CustomerId"]:-1;
                        order.OrderDate = reader["OrderDate"].ToString();
                        order.OrderStatus = (string)reader["status"];
                    }
                    return order;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                    return null;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }
        public void EditOrderStatus(string orderNumber, string newStatus)
        {
            HashSet<String> orderStatusSet = new HashSet<String>() { "Ordered", "Shipped", "Delivered", "Returned", "Cancelled" };
            Order order = new Order();
            if (!orderStatusSet.Contains(newStatus))
            {
                Console.WriteLine("Invalid Status");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("update orders set status=@status where orderNumber=@orderNumber", conn);
                    _command.Parameters.AddWithValue("orderNumber", orderNumber);
                    _command.Parameters.AddWithValue("status", newStatus);
                    conn.Open();
                    int rowsAffected = _command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Successfully Updated");
                    }
                    else
                    {
                        Console.WriteLine("Update failed ");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }

        public void DeleteOrderByIdOrOrderNumber(string searchKey, object searchValue)
        {
            HashSet<string> allowedKeys = new HashSet<string>() { "CustomerId", "ID", "OrderNumber", "OrderDate", "status" };
            if (!allowedKeys.Contains(searchKey))
            {
                Console.WriteLine("Invalid searchKey");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand($"delete from orders where {searchKey}=@searchValue", conn);
                    if (searchValue is int)
                        _command.Parameters.AddWithValue("searchValue", (int)searchValue);
                    else if (searchValue is string)
                        _command.Parameters.AddWithValue("searchValue", (string)searchValue);
                    conn.Open();
                    int rowsAffected = (int)_command.ExecuteNonQuery();
                    if (rowsAffected > 0) Console.WriteLine("Order deleted");
                    else Console.WriteLine("Order does not exists");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                }
                finally
                {
                    _command.Dispose();
                }
            }
        }
    }
}
