using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CustomerOrderManagement
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }


        SqlCommand _command;
        string connectionString = Program.ConnectionString;


        //public void CreateCustomer(string name, string email, string address, string city)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            _command = new SqlCommand("Insert into customers(name,email,address,city) values(@name,@email,@address,@city)", conn);
        //            _command.Parameters.AddWithValue("name", name);
        //            _command.Parameters.AddWithValue("email", email);
        //            _command.Parameters.AddWithValue("address", address);
        //            _command.Parameters.AddWithValue("city", city);
        //            conn.Open();
        //            int rowsAffected = _command.ExecuteNonQuery();
        //            if (rowsAffected > 0)
        //            {
        //                Customer customer = FetchCustomerByEmail(email);
        //                Console.WriteLine("Customer account created and your ID is " + customer.Id + "\n\n");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Internal error while creating account!!!");
        //            }
        //        }
        //        catch (SqlException sqlException)
        //        {
        //            if (sqlException.Number == 2627 || sqlException.Number == 2601)
        //            {
        //                Console.WriteLine("Customer with this email already exists. Enter a new email!!!");
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
        public void CreateCustomerUsingStoredProcedure(string name, string email, string address, string city)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("sp_insertRecordsToCustomers", conn);
                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@Name", name);
                    _command.Parameters.AddWithValue("@Email", email);
                    _command.Parameters.AddWithValue("@Address", address);
                    _command.Parameters.AddWithValue("@City", city);
                    SqlParameter customerId = new SqlParameter()
                    {
                        ParameterName = "@CustomerId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    _command.Parameters.Add(customerId);
                    conn.Open();
                    int rowsAffected = _command.ExecuteNonQuery();
                    if (rowsAffected > 0 && customerId.Value != DBNull.Value)
                    {
                        Console.WriteLine("Customer account created and your ID is " + customerId.Value.ToString() + "\n\n");
                    }
                    else
                    {
                        Console.WriteLine("Internal error while creating account!!!");
                    }
                }
                catch (SqlException sqlException)
                {
                    Console.WriteLine(sqlException.Message);
                    if (sqlException.Number == 2627 || sqlException.Number == 2601)
                    {
                        Console.WriteLine("Customer with this email already exists. Enter a new email!!!");
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
        public List<Customer> FetchAllCustomerRecords()
        {
            List<Customer> listOfCustomer = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("select * from customers", conn);
                    conn.Open();
                    SqlDataReader reader = _command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.Id = (int)reader["id"];
                        customer.Name = (string)reader["name"];
                        customer.Email = (string)reader["Email"];
                        customer.Address = (string)reader["address"];
                        customer.City = (string)reader["city"];
                        listOfCustomer.Add(customer);
                    }
                    return listOfCustomer;
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
        public Customer FetchCustomerByEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Customer customer = new Customer();
                    _command = new SqlCommand("select * from customers where email=@email", conn);
                    _command.Parameters.AddWithValue("email", email);
                    conn.Open();
                    SqlDataReader reader = _command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        customer.Id = (int)reader["id"];
                        customer.Name = (string)reader["name"];
                        customer.Email = (string)reader["Email"];
                        customer.Address = (string)reader["address"];
                        customer.City = (string)reader["city"];
                        break;
                    }
                    return customer;
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
        public Customer FetchCustomerById(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Customer customer = new Customer();
                    _command = new SqlCommand("select * from customers where ID=@ID", conn);
                    _command.Parameters.AddWithValue("ID", customerId);
                    conn.Open();
                    SqlDataReader reader = _command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        customer.Email = (string)reader["Email"];
                        customer.Id = (int)reader["id"];
                        customer.Name = (string)reader["name"];
                        customer.Address = (string)reader["address"];
                        customer.City = (string)reader["city"];
                        break;
                    }
                    return customer;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                    Console.WriteLine(2);
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
        public void DeleteCustomerByIdOrEmail(string searchKey, object searchValue)
        {
            HashSet<string> allowedKeys = new HashSet<string>() { "ID", "Email", "Name", "Address", "City" };
            if (!allowedKeys.Contains(searchKey))
            {
                Console.WriteLine("Invalid searchKey");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand($"delete from customers where {searchKey}=@searchValue", conn);
                    if (searchValue is int)
                        _command.Parameters.AddWithValue("searchValue", (int)searchValue);
                    else if (searchValue is string)
                        _command.Parameters.AddWithValue("searchValue", (string)searchValue);
                    conn.Open();
                    int rowsAffected = (int)_command.ExecuteNonQuery();
                    if (rowsAffected > 0) Console.WriteLine("Customer account deleted");
                    else Console.WriteLine("Customer not exists");
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
        public DataTable FetchAllOrdersOfCustomer(int customerId)
        {
            DataTable customerOrders = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("select Name,Email,OrderNumber,status,OrderDate from customers inner join orders on customers.ID=orders.CustomerId where customers.ID=@customerId", conn);
                    _command.Parameters.AddWithValue("customerId", customerId);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(_command);
                    adapter.Fill(customerOrders);
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
                return customerOrders;
            }
        }
        public void EditCustomerRecord(int id, string searchKey, object searchValue)
        {
            HashSet<string> allowedKeys = new HashSet<string>() { "Name", "Address", "City" };
            if (!allowedKeys.Contains(searchKey))
            {
                Console.WriteLine("Invalid search key");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand($"update customers set {searchKey}=@searchValue where id=@ID", conn);
                    if (searchValue is int)
                        _command.Parameters.AddWithValue("searchValue", (int)searchValue);
                    else if (searchValue is string)
                        _command.Parameters.AddWithValue("searchValue", (string)searchValue);
                    _command.Parameters.AddWithValue("ID", id);
                    conn.Open();
                    int rowsAffected = (int)_command.ExecuteNonQuery();
                    if (rowsAffected > 0) Console.WriteLine("Customer account updated");
                    else Console.WriteLine("Customer not exists");
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
