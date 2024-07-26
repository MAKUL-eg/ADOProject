using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace CustomerOrderManagement
{
    public class Program
    {
        public static DataSet CustomerOrdersTable = new DataSet();
        public static string ConnectionString;
        static Program()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public static void DisplayMenu(out int choice)
        {
            Console.WriteLine("-------------Customer Menu-------------\n");
            Console.WriteLine("1. Create customer account");
            Console.WriteLine("2. View customer details");
            Console.WriteLine("3. Display all Orders of a customer");
            Console.WriteLine("4. Edit user details");
            Console.WriteLine("5. Delete account using customer Id");
            Console.WriteLine("6. Delete account using customer Email");
            Console.WriteLine();

            Console.WriteLine("-------------Order Menu-------------\n");
            Console.WriteLine("7. Create Order ");
            Console.WriteLine("8. View Order details");
            Console.WriteLine("9. Delete Order using order ID");
            Console.WriteLine("10. Delete Order using order number");
            Console.WriteLine();

            Console.WriteLine("-------------Admin Menu-------------\n");
            Console.WriteLine("11. View all customers and their orders");
            Console.WriteLine("12. Display all customers");
            Console.WriteLine("13. Display all Orders");
            Console.WriteLine("14. Edit Order Status");
            Console.WriteLine();

            Console.WriteLine("15. EXIT");
            try
            {
                Console.Write("Enter your choice: ");
                choice = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                choice = -1;
                Console.WriteLine("Invalid choice");
            }
        }
        public static void Main()
        {
            string name, address, city, email;
            int customerId;
            int orderId;
            string orderNumber, status;

            Customer customer = new Customer();
            Order order = new Order();
            Admin admin = new Admin();

            List<Customer> customerList;
            List<Order> orderList;

            DataTable customerOrderRecords;
            DataTable customerOrders;
            while (true)
            {
                try
                {
                    DisplayMenu(out int choice);
                    if (choice == -1) continue;
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter your name: ");
                            name = Console.ReadLine();
                            Console.Write("Enter your Email: ");
                            email = Console.ReadLine();
                            Console.Write("Enter your Address: ");
                            address = Console.ReadLine();
                            Console.Write("Enter your City: ");
                            city = Console.ReadLine();
                            customer.CreateCustomerUsingStoredProcedure(name, email, address, city);
                            break;
                        case 2:
                            Console.Write("Do you know your customer ID(y/n):");
                            char IdOrEmailChoice = char.Parse(Console.ReadLine());
                            if (IdOrEmailChoice.Equals('y') || IdOrEmailChoice.Equals('Y'))
                            {
                                Console.Write("Enter your customer Id: ");
                                customerId = int.Parse(Console.ReadLine());
                                customer = customer.FetchCustomerById(customerId);
                            }
                            else if (IdOrEmailChoice.Equals('n')|| IdOrEmailChoice.Equals('N'))
                            {
                                Console.Write("Enter your Email: ");
                                email = Console.ReadLine();
                                customer = customer.FetchCustomerByEmail(email);
                                if (customer != null) Console.WriteLine("ID: " + customer.Id);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice for edit");
                                continue;
                            }
                            if (customer == null)
                            {
                                Console.WriteLine("Customer account not found!! ");
                            }
                            else
                            {
                                Console.WriteLine("\n");
                                Console.WriteLine("{0,-20} | {1,-30} | {2,-20} | {3,-15}", "Name", "Email", "Address", "City");
                                Console.WriteLine(new string('-', 85));
                                Console.WriteLine("{0,-20} | {1,-30} | {2,-20} | {3,-15}", customer.Name, customer.Email, customer.Address, customer.City);
                                Console.WriteLine("\n\n");
                            }
                            break;
                        case 3:
                            Console.Write("Enter your customer Id: ");
                            customerId = int.Parse(Console.ReadLine());
                            customerOrders = customer.FetchAllOrdersOfCustomer(customerId);
                            if (customerOrders.Rows.Count <= 0)
                            {
                                Console.WriteLine("No record found");
                            }
                            else
                            {
                                Console.WriteLine("\n");
                                Console.WriteLine("{0,-20} | {1,-30} | {2,-22} | {3,-15} | {4,-20}", "Name", "Email", "Order Number", "Status", "Order Date");
                                Console.WriteLine(new string('-', 118));
                                foreach (DataRow row in customerOrders.Rows)
                                {
                                    name = row["Name"] != DBNull.Value ? row["Name"].ToString() : "N/A";
                                    email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "N/A";
                                    orderNumber = row["OrderNumber"] != DBNull.Value ? row["OrderNumber"].ToString() : "N/A";
                                    status = row["status"] != DBNull.Value ? row["status"].ToString() : "N/A";
                                    string date = row["OrderDate"] != DBNull.Value ? row["OrderDate"].ToString() : "N/A";
                                    Console.WriteLine("{0,-20} | {1,-30} | {2,-20} | {3,-15} | {4,-20}", name, email, orderNumber, status, date);
                                }
                                Console.WriteLine("\n\n");
                            }
                            break;
                        case 4:
                            int editChoice;
                            Console.WriteLine("1.Edit Name");
                            Console.WriteLine("2.Edit Address");
                            Console.WriteLine("3.Edit City");
                            Console.WriteLine("4.Go to main menu");
                            Console.Write("Enter your choice: ");
                            editChoice = int.Parse(Console.ReadLine());
                            switch (editChoice)
                            {
                                case 1:
                                    Console.Write("Enter your customer Id: ");
                                    customerId = int.Parse(Console.ReadLine());
                                    customer = customer.FetchCustomerById(customerId);
                                    if (customer == null)
                                    {
                                        Console.WriteLine("Invalid customer Id");
                                    }
                                    else
                                    {
                                        Console.Write("Enter new name: ");
                                        name = Console.ReadLine();
                                        customer.EditCustomerRecord(customerId, "Name", name);
                                    }
                                    break;
                                case 2:
                                    Console.Write("Enter your customer Id: ");
                                    customerId = int.Parse(Console.ReadLine());
                                    customer = customer.FetchCustomerById(customerId);
                                    if (customer == null)
                                    {
                                        Console.WriteLine("Invalid customer Id");
                                    }
                                    else
                                    {
                                        Console.Write("Enter new Address: ");
                                        address = Console.ReadLine();
                                        customer.EditCustomerRecord(customerId, "Address", address);
                                    }
                                    break;
                                case 3:
                                    Console.Write("Enter your customer Id: ");
                                    customerId = int.Parse(Console.ReadLine());
                                    customer = customer.FetchCustomerById(customerId);
                                    if (customer == null)
                                    {
                                        Console.WriteLine("Invalid customer Id");
                                    }
                                    else
                                    {
                                        Console.Write("Enter new city: ");
                                        city = Console.ReadLine();
                                        customer.EditCustomerRecord(customerId, "City", city);
                                    }
                                    break;
                                case 4: continue;
                                default:
                                    break;
                            }
                            break;
                        case 5:
                            Console.Write("Enter your customer Id: ");
                            customerId = int.Parse(Console.ReadLine());
                            customer.DeleteCustomerByIdOrEmail("ID", customerId);
                            break;
                        case 6:
                            Console.Write("Enter your customer Email: ");
                            email = Console.ReadLine();
                            customer.DeleteCustomerByIdOrEmail("Email", email);
                            break;
                        case 7:
                            Console.Write("Enter the customer Id: ");
                            customerId = int.Parse(Console.ReadLine());
                            order.CreateOrder(customerId);
                            Console.WriteLine("\n\n");
                            break;
                        case 8:
                            Console.Write("Do you know your order ID(y/n):");
                            char IdOrOrderNumberChoice = char.Parse(Console.ReadLine());
                            if (IdOrOrderNumberChoice.Equals('y')|| IdOrOrderNumberChoice.Equals('Y'))
                            {
                                Console.Write("Enter your order Id: ");
                                orderId = int.Parse(Console.ReadLine());
                                order = order.FetchOrderById(orderId);
                            }
                            else if (IdOrOrderNumberChoice.Equals('n')|| IdOrOrderNumberChoice.Equals('N'))
                            {
                                Console.Write("Enter your orderNumber: ");
                                orderNumber = Console.ReadLine();
                                order = order.FetchOrderByOrderNumer(orderNumber);
                                if (order != null) Console.WriteLine("Order Id: " + order.Id);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice for edit");
                                continue;
                            }
                            if (order == null)
                            {
                                Console.WriteLine("Cannot found order");
                            }
                            else
                            {
                                Console.WriteLine("\n");
                                Console.WriteLine("{0,-22} | {1,-30} | {2,-20} | {3,-15}", "Order Number", "Customer Id", "Order Status", "Order Date");
                                Console.WriteLine(new string('-', 100));
                                Console.WriteLine("{0,-20} | {1,-30} | {2,-20} | {3,-15}", order.OrderNumber, order.CustomerId==-1?"customer account deleted":order.CustomerId.ToString(), order.OrderStatus, order.OrderDate);
                                Console.WriteLine("\n\n");
                            }
                            break;
                        case 9:
                            Console.Write("Enter your order Id: ");
                            orderId = int.Parse(Console.ReadLine());
                            order.DeleteOrderByIdOrOrderNumber("ID", orderId);
                            break;
                        case 10:
                            Console.Write("Enter your orderNumber: ");
                            orderNumber = Console.ReadLine();
                            order.DeleteOrderByIdOrOrderNumber("OrderNumber", orderNumber);
                            break;
                        case 11:
                            customerOrderRecords = admin.FetchAllCustomerWithOrders();
                            if (customerOrderRecords.Rows.Count <= 0)
                            {
                                Console.WriteLine("No record found");
                            }
                            else
                            {
                                Console.WriteLine("{0,-15} | {1,-20} | {2,-10} | {3,-10} | {4,-25} | {5,-10} | {6,-15}", "Name", "Email", "Address", "City", "Order Number", "Status", "Order Date");
                                Console.WriteLine(new string('-', 133));
                                foreach (DataRow row in customerOrderRecords.Rows)
                                {
                                    name = row["Name"] != DBNull.Value ? row["Name"].ToString() : "N/A";
                                    email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "N/A";
                                    address = row["Address"] != DBNull.Value ? row["Address"].ToString() : "N/A";
                                    city = row["City"] != DBNull.Value ? row["City"].ToString() : "N/A";
                                    orderNumber = row["OrderNumber"] != DBNull.Value ? row["OrderNumber"].ToString() : "N/A";
                                    status = row["status"] != DBNull.Value ? row["status"].ToString() : "N/A";
                                    string orderDate = row["OrderDate"] != DBNull.Value ? row["OrderDate"].ToString() : "N/A";
                                    Console.WriteLine("{0,-15} | {1,-20} | {2,-10} | {3,-10} | {4,-25} | {5,-10} | {6,-15:MM/dd/yyyy}", name, email, address, city, orderNumber, status, orderDate);
                                }
                                Console.WriteLine("\n");
                            }
                            break;
                        case 12:
                            customerList = customer.FetchAllCustomerRecords();
                            if (customerList == null)
                            {
                                Console.WriteLine("There is no data in database");
                            }
                            else
                            {
                                Console.WriteLine("{0,-20} | {1,-30} | {2,-20} | {3,-15}", "Name", "Email", "Address", "City");
                                Console.WriteLine(new string('-', 85));

                                foreach (Customer customerRecord in customerList)
                                {
                                    Console.WriteLine("{0,-20} | {1,-30} | {2,-20} | {3,-15}", customerRecord.Name, customerRecord.Email, customerRecord.Address, customerRecord.City);
                                }
                            }
                            Console.WriteLine("\n");
                            break;
                        case 13:
                            orderList = order.FetchAllOrderRecords();
                            if (orderList == null)
                            {
                                Console.WriteLine("There is no data in database");
                                continue;
                            }
                            else
                            {
                                    Console.WriteLine("\n");
                                    Console.WriteLine("{0,-30} | {1,-25} | {2,-20} | {3,-15}", "Order Number", "Customer Id", "Order Status", "Order Date");
                                    Console.WriteLine(new string('-', 100));
                                foreach (Order orderRecord in orderList)
                                {
                                    string customerIdNullHandle = orderRecord.CustomerId == -1 ? "customer account deleted" : orderRecord.CustomerId.ToString();
                                    Console.WriteLine("{0,-30} | {1,-25} | {2,-20} | {3,-15}", orderRecord.OrderNumber, customerIdNullHandle, orderRecord.OrderStatus, orderRecord.OrderDate);
                                }
                                    Console.WriteLine("\n\n");
                            }
                            Console.WriteLine("\n");
                            break;
                        case 14:
                                Console.Write("Enter your orderNumber: ");
                                orderNumber = Console.ReadLine();
                            Console.Write("Enter the status (Ordered, Shipped, Delivered, Returned, Cancelled):");
                            status = Console.ReadLine();
                            order.EditOrderStatus(orderNumber, CapitalizeWord(status));
                            break;
                        case 15: return;
                        default:
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format");
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }
        public static string CapitalizeWord(string word)
        {
            if (word == null) return word;
            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }
    }
}
