using System;
using System.Data;
using System.Data.SqlClient;

namespace CustomerOrderManagement
{
    public class Admin
    {
        SqlCommand _command;
        string connectionString = Program.ConnectionString;
        public DataTable FetchAllCustomerWithOrders()
        {
            DataTable customerOrderRecords = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    _command = new SqlCommand("select Name,Email,Address,City,OrderNumber,status,OrderDate from customers left join orders on customers.ID=orders.CustomerId", conn);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(_command);
                    adapter.Fill(customerOrderRecords);
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
                return customerOrderRecords;
            }
        }
    }
}
