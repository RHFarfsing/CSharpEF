using EFLibrary;
using EFLibrary.Models;
using System;
using System.Linq;

namespace CSharpEF {
    class Program {
        static void Main(string[] args) {
            var context = new AppDbContext();
            //var top2 = context.Products.Where(x => x.Id > 3).ToList();
            //Console.WriteLine($"Avg price is { context.Products.Average(x => x.Price)}");
            //var actCust = context.Customers.Where(x => x.Active); 

            //AddCustomer(context);
            //GetCustomerByPk(context);
            //UpdateCustomer(context);
            //DeleteCustomer(context);
            //UpdateCustomerSales(context);
            //GetAllCustomers(context);
            //AddOrder(context);
            //Deleteorder(context);
            //GetAllOrders(context);
            //AddProduct(context);
            //GetAllProducts(context);
            //AddOrderline(context);
            //RecalcOrderAmounts(context);
            GetOrderLines(context);
            
        }
        static void DeleteCustomer(AppDbContext context) {
            var KeyToDelete = 1;
            var Cust = context.Customers.Find(KeyToDelete);
            if (Cust == null) throw new Exception("Customer not found!");
            context.Customers.Remove(Cust);
            var rowsAffected = context.SaveChanges();
            if (rowsAffected != 1) throw new Exception("Delete Failed!");
        }
        static void UpdateCustomer(AppDbContext context) {
            var custPk = 2;
            var cust = context.Customers.Find(custPk);
            if (cust == null) throw new Exception("Customer not Found!");
            cust.Name = "Lowe's";
            var rowsAffected = context.SaveChanges();
            if (rowsAffected != 1) throw new Exception("Failed to update customer!");
            Console.WriteLine("Update Successful!");
        }
        static void GetAllCustomers(AppDbContext context) {
            var custs = context.Customers.ToList();
            foreach (var c in custs) {
                Console.WriteLine(c);
            }
        }
        static void GetCustomerByPk(AppDbContext context) {
            var custpk = 2;
            var cust = context.Customers.Find(custpk);
            if (cust == null) throw new Exception("Customer not found!");
            Console.WriteLine(cust);
        }
        static void AddCustomer(AppDbContext context) {
            var cust = new Customer {
                Id = 0,
                Name = "Max Technical Training",
                Sales = 0,
                Active = true
            };
            context.Customers.Add(cust);
            var rowsAffected = context.SaveChanges();
            if (rowsAffected == 0) throw new Exception("Add Failed!");
            return;
        }
        //below are orders adding and selecting all orders
        static void AddOrder(AppDbContext context) {
            var order1 = new Order { Id = 0, Description = "Order 1", Amount = 500, CustomerId = 4 };            
            var order2 = new Order { Id = 0, Description = "Order 2", Amount = 400, CustomerId = 4 };
            var order3 = new Order { Id = 0, Description = "Order 3", Amount = 850, CustomerId = 4 };
            var order4 = new Order { Id = 0, Description = "Order 4", Amount = 250, CustomerId = 4 };
            var order5 = new Order { Id = 0, Description = "Order 5", Amount = 700, CustomerId = 4 };
            context.AddRange(order1, order2, order3, order4, order5);
            var rowsAffected = context.SaveChanges();
            if (rowsAffected == 0) throw new Exception("Add Failed!");
            Console.WriteLine("Added order!");
        }
        static void GetAllOrders(AppDbContext context) {
            var orders = context.Orders.ToList();
            foreach(var o in orders) {
                Console.WriteLine(o);
            }
        }
        static void Deleteorder(AppDbContext context) {
            var KeyToDelete = 1;
            var order = context.Orders.Find(KeyToDelete);
            if (order == null) throw new Exception("Order not found!");
            context.Orders.Remove(order);
            var rowsAffected = context.SaveChanges();
            if (rowsAffected != 1) throw new Exception("Delete Failed!");
        }
        //below are pruducts adding and selecting all products
        static void AddProduct(AppDbContext context) {
            var product1 = new Product { Id = 0, Code = "prod1", Name = "Product One", Price = 500 };
            var product2 = new Product { Id = 0, Code = "prod2", Name = "Product Two", Price = 400 };
            var product3 = new Product { Id = 0, Code = "prod3", Name = "Product Three", Price = 850 };
            var product4 = new Product { Id = 0, Code = "prod4", Name = "Product Four", Price = 250 };
            var product5 = new Product { Id = 0, Code = "prod5", Name = "Product Five", Price = 700 };
            context.AddRange(product1, product2, product3, product4, product5);
            var rowsAffected = context.SaveChanges();
            if (rowsAffected == 0) throw new Exception("Add Failed!");
            Console.WriteLine("Added order!");
        }
        static void GetAllProducts(AppDbContext context) {
            var products = context.Products.ToList();
            foreach(var p in products) {
                Console.WriteLine(p);
            }
        }
        //join views for customer and order tables
        static void UpdateCustomerSales(AppDbContext context) {
            var CustOrderJoin = from c in context.Customers
                                join o in context.Orders
                                on c.Id equals o.CustomerId
                                where c.Id == 4
                                select new { Amount = o.Amount,
                                    Customer = c.Name,
                                    Order = o.Description 
                                };
            var OrderTotal = CustOrderJoin.Sum(c=> c.Amount);
            var cust = context.Customers.Find(4);
            cust.Sales = OrderTotal;
            context.SaveChanges();
        }
        //below are orderline table stuff
        static void AddOrderline(AppDbContext context) {
            var order = context.Orders.SingleOrDefault(o => o.Description == "Order 5");
            var product = context.Products.SingleOrDefault(p => p.Code == "prod2");
            var orderline = new Orderline { 
                Id = 0,
                ProductId = product.Id,
                OrderId = order.Id,
                Quantity = 1
            };
            context.Orderlines.Add(orderline);
            var rowsAffected = context.SaveChanges();
            if (rowsAffected != 1) throw new Exception("Orderline insert failed!");
        }
        static void GetOrderLines(AppDbContext context) {
            var orderlines = context.Orderlines.ToList();
            orderlines.ForEach(line => Console.WriteLine($"{line.Order.Id}/{line.Order.Description}/{line.Product.Code}/{line.Product.Name}/{line.Product.Price}/{line.Quantity}/{line.Order.Amount}"));
        }
        //This is what we will be recreating in our capstone project
        static void RecalcOrderAmount(int orderId, AppDbContext context) {
            var order = context.Orders.Find(orderId);
            var total = order.Orderlines.Sum(ol => ol.Quantity * ol.Product.Price);
            order.Amount = total;
            var rc = context.SaveChanges();
            if (rc != 1) throw new Exception("Order update of amount failed!");
        }
        static void RecalcOrderAmounts(AppDbContext context) {
            var orderIds = context.Orders.Select(x => x.Id).ToArray();
            foreach(var orderId in orderIds) {
                RecalcOrderAmount(orderId, context);
            }
        }
    }
}
