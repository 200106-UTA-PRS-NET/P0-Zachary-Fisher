using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;
using System.Linq;
using PizzaBox.Storing;


namespace PizzaBox.Client
{
    class Program
    {
        static Mapper m = new Mapper();
        static void Main(string[] args)
        {
            #region config
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = configBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<PizzaBoxContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaBoxDB"));
            var options = optionsBuilder.Options;
            PizzaBoxContext db = new PizzaBoxContext(options);
            #endregion config

            bool running = true;
            List<Domain.Models.Store> stores = new List<Domain.Models.Store>();
            List<Order> orders = new List<Order>();
            User currentUser = null;
            Domain.Models.Store currentStore = null;
            string input;
            string activity = "menu";
            Console.WriteLine("Welcome to the Pizza Client.");
            while (running)
            {
                Console.WriteLine("Would you like to: ");
                Console.WriteLine("u:\t Login as user");
                Console.WriteLine("p:\t Login as store administrator");
                Console.WriteLine("q:\t Quit the program");
                input = Console.ReadLine();
                if (input.Equals("u"))
                {
                    activity = "user";
                    while (activity.Equals("user"))
                    {
                        if (currentUser == null)
                        {
                            Console.WriteLine("Would you like to: ");
                            Console.WriteLine("l:\t Login to an existing account");
                            Console.WriteLine("c:\t Create a new account");
                            Console.WriteLine("b:\t Go back to previous menu");
                            input = Console.ReadLine();
                            if (input.Equals("l"))
                            {
                                activity = "loginuser";
                                while (activity.Equals("loginuser"))
                                {
                                    Console.WriteLine("Enter your username. Type BACK to quit.");
                                    string un = Console.ReadLine();
                                    if (un == "BACK")
                                    {
                                        activity = "user";
                                    }
                                    if( db.Customer.Any(c => c.Uname == un)&& un!="BACK")
                                    {
                                        Console.WriteLine("Enter your password");
                                        string pass = Console.ReadLine();
                                        if(db.Customer.Any(c=> c.Uname ==un && c.Pass == pass))
                                        {
                                            var query = from c in db.Customer
                                                        where c.Uname == un && c.Pass == pass
                                                        select c;
                                            try
                                            {
                                                Customer cust = query.Single();
                                                currentUser = Mapper.Map(cust);
                                                Console.WriteLine($"Welcome, {currentUser.UName}.");
                                                activity = "user";
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Error, more than one user with that username/password exists");
                                                throw;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("That username does not exist");
                                    }
                                }
                            }
                            else if (input.Equals("c"))
                            {
                                activity = "createuser";
                                while (activity.Equals("createuser"))
                                {
                                    string un = "ERRORIFSHOWN";
                                    string pw;
                                    bool unique = false;
                                    while (!unique)
                                    {
                                        unique = true;
                                        Console.WriteLine("Enter a new username");
                                        un = Console.ReadLine();
                                        if(db.Customer.Any(c => c.Uname == un))
                                        {
                                            unique = false;
                                            Console.WriteLine("That username is taken");
                                        }
                                    }
                                    Console.WriteLine("Enter a password");
                                    pw = Console.ReadLine();
                                    User nuser = new User{
                                        UName=un,
                                        Password=pw
                                    };
                                    AddUser(db, nuser);
                                    activity = "user";
                                    Console.WriteLine($"Account created with Username: {un}, Password: {pw}");
                                }
                            }
                            else if (input.Equals("b"))
                            {
                                activity = "menu";
                            }
                            else
                            {
                                Console.WriteLine("Error, invalid option. Accepted options are l, c, and b.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Would you like to: ");
                            Console.WriteLine("l:\t Log out of your account");
                            Console.WriteLine("o:\t Create a new order");
                            Console.WriteLine("v:\t View your order history");
                            Console.WriteLine("b:\t Go back to previous menu");
                            input = Console.ReadLine();
                            if (input.Equals("l"))
                            {
                                currentUser = null;
                            }
                            else if (input.Equals("o"))
                            {
                                //CREATE ORDER
                            }
                            else if (input.Equals("v"))
                            {
                                int n = 1;
                                foreach (Order order in orders)
                                {
                                    if(order.UID==currentUser.Uid)
                                    {
                                        order.CalculateCost();
                                        Console.WriteLine($"Order {n}, {order.ShowOrder()},\n Price: {order.Cost}");
                                        n++;
                                    }
                                }
                            }
                            else if (input.Equals("b"))
                            {
                                activity = "menu";
                            }
                            else
                            {
                                Console.WriteLine("Error, invalid option. Accepted options are l, o, v, and b.");
                            }
                        }
                    }
                }
                else if (input.Equals("p"))
                {

                }
                else if (input.Equals("l"))
                {

                }
                else if (input.Equals("s"))
                {

                }
                else if (input.Equals("q"))
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("Error, invalid option. Accepted options are u, p, l, s, and q");
                }
            }
        }
        static IEnumerable<Orders> GetUserOrders(PizzaBoxContext db, User u)
        {
            var query = from o in db.Orders
                        where o.Uid == u.Uid
                        select o;
            return query;
        }
        static IEnumerable<Orders> GetStoreOrders(PizzaBoxContext db, Domain.Models.Store s)
        {
            var query = from o in db.Orders
                        where o.Sid == s.Sid
                        select o;
            return query;
        }
        static void AddOrder(PizzaBoxContext db, Order o)
        {
            o.CalculateCost();
            Orders no = Mapper.Map(o);
            db.Orders.Add(no);// this will generate insert query
            db.SaveChanges();// this will execute the above generate insert query
        }
        static void AddUser(PizzaBoxContext db, User u)
        {
            if (db.Customer.Any(c => c.Uname == u.UName) || u.UName == null)
            {
                Console.WriteLine($"The username {u.UName} already exists.");
                return;
            }
            else
            {
                Customer c = Mapper.Map(u);
                db.Customer.Add(c);// this will generate insert query
            }
            db.SaveChanges();// this will execute the above generate insert query
        }
        static void AddStore(PizzaBoxContext db, Domain.Models.Store s)
        {
            if (db.Store.Any(st => st.Sname ==s.SName) || s.SName == null)
            {
                Console.WriteLine($"The store {s.SName} already exists.");
                return;
            }
            else
            {
                Storing.Repositories.Store st = Mapper.Map(s);
                db.Store.Add(st);// this will generate insert query
            }
            db.SaveChanges();
        }
    }
}
