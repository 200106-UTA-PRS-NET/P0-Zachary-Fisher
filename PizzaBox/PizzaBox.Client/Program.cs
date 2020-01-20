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
            User currentUser = null;
            Domain.Models.Store currentStore = null;
            Domain.Models.Store selectedStore = null;
            string input;
            string activity = "menu";
            Console.WriteLine("Welcome to the Pizza Client.");
            while (running)
            {
                Console.WriteLine("Would you like to: ");
                Console.WriteLine("u:\t Login as user.");
                Console.WriteLine("s:\t Login as store administrator.");
                Console.WriteLine("q:\t Quit the program.");
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
                                    if (db.Customer.Any(c => c.Uname == un) && un != "BACK")
                                    {
                                        Console.WriteLine("Enter your password");
                                        string pass = Console.ReadLine();
                                        if (db.Customer.Any(c => c.Uname == un && c.Pass == pass))
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
                                        if (db.Customer.Any(c => c.Uname == un))
                                        {
                                            unique = false;
                                            Console.WriteLine("That username is taken");
                                        }
                                    }
                                    Console.WriteLine("Enter a password");
                                    pw = Console.ReadLine();
                                    User nuser = new User
                                    {
                                        UName = un,
                                        Password = pw
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
                                Console.WriteLine("Select a store to order from. Available stores are:");
                                activity = "ss";
                                while(activity.Equals("ss"))
                                {
                                    ShowStores(db);
                                    input = Console.ReadLine();
                                    if(input == "b")
                                    {
                                        activity = "user";
                                        break;
                                    }
                                    var query = from s in db.Store
                                                where s.Sname == input
                                                select s;
                                    try
                                    {
                                        Storing.Repositories.Store st = query.Single();
                                        selectedStore = Mapper.Map(st);
                                        Console.WriteLine($"Store Selected: {selectedStore.SName}.");
                                        activity = "";
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Error, that store cannot be found.");
                                    }
                                }
                                if (activity.Equals("user"))
                                {
                                    break;
                                }
                                Console.WriteLine("Create your order.");
                                activity = "ordering";
                                Order o = new Order(currentUser.UName, selectedStore.SName);
                                while (activity.Equals("ordering"))
                                { 
                                    Console.WriteLine("p\t Add preset pizza."); //done
                                    Console.WriteLine("c\t Add custom pizza."); //done
                                    Console.WriteLine("r\t Remove a pizza from the order"); //done
                                    Console.WriteLine("b\t Cancel the order."); //done
                                    Console.WriteLine("v\t View the order."); //done
                                    Console.WriteLine("s\t Submit the order"); //done
                                    input = Console.ReadLine();
                                    if (input.Equals("p"))
                                    {
                                        Pizza p = new Pizza();
                                    Preset:
                                        try
                                        {
                                            Console.WriteLine("What preset do you want?");
                                            Console.WriteLine("hawaiian, 3 meat, supreme, meat lover, cheeseburger, bacon cheeseburger.");
                                            input = Console.ReadLine();
                                            p.Preset(input);
                                        }
                                        catch
                                        {
                                            goto Preset;
                                        }
                                    PresetSize:
                                        try
                                        {
                                            Console.WriteLine("What size do you want?");
                                            Console.WriteLine("6, 8, 10, 12, or 14");
                                            int s = Convert.ToInt32(Console.ReadLine());
                                            p.Size = s;
                                        }
                                        catch
                                        {
                                            goto PresetSize;
                                        }
                                    PresetCrust:
                                        try
                                        {
                                            Console.WriteLine("What crust type do you want?");
                                            Console.WriteLine("sicilian, thin, or deep dish.");
                                            input = Console.ReadLine();
                                            p.Crust = input;
                                        }
                                        catch
                                        {
                                            goto PresetCrust;
                                        }
                                        o.AddPizza(p);
                                    }
                                    else if (input.Equals("c"))
                                    {
                                        Pizza p = new Pizza();
                                    CustomSize:
                                        try
                                        {
                                            Console.WriteLine("What size do you want?");
                                            Console.WriteLine("6, 8, 10, 12, or 14");
                                            int s = Convert.ToInt32(Console.ReadLine());
                                            p.Size = s;
                                        }
                                        catch
                                        {
                                            goto CustomSize;
                                        }
                                    CustomCrust:
                                        try
                                        {
                                            Console.WriteLine("What crust type do you want?");
                                            Console.WriteLine("sicilian, thin, or deep dish.");
                                            input = Console.ReadLine();
                                            p.Crust = input;
                                        }
                                        catch
                                        {
                                            goto CustomCrust;
                                        }
                                        bool done = false;
                                        while (!done)
                                        {
                                            Console.WriteLine("Change toppings");
                                            Console.WriteLine("a\t Add topping");
                                            Console.WriteLine("r\t Remove topping");
                                            Console.WriteLine("v\t View toppings");
                                            Console.WriteLine("d\t Done selecting toppings");
                                            input = Console.ReadLine();
                                            if (input.Equals("a"))
                                            {
                                                Console.WriteLine("Available toppings");
                                                foreach (string t in p.allowedToppings)
                                                {
                                                    Console.Write($"{t}, ");
                                                }

                                                Console.WriteLine("\n");
                                                input = Console.ReadLine();
                                                p.AddTopping(input);
                                            }
                                            else if (input.Equals("r"))
                                            {
                                                Console.WriteLine("Toppings to remove");
                                                Console.WriteLine(p.Toppings());
                                                input = Console.ReadLine();
                                                p.RemoveTopping(input);
                                            }
                                            else if (input.Equals("v"))
                                            {
                                                Console.WriteLine(p.Toppings());
                                            }
                                            else if (input.Equals("d"))
                                            {
                                                o.AddPizza(p);
                                                done = true;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Accepted inputs a, r, v, and d");
                                            }
                                        }
                                    }
                                    else if (input.Equals("r"))
                                    {
                                        Console.WriteLine(o.ShowOrder());
                                        Console.WriteLine("Enter the number of the pizza to remove");
                                        int a = Convert.ToInt32(Console.ReadLine());
                                        o.RemovePizza(a);
                                    }
                                    else if(input.Equals("b"))
                                    {
                                        activity = "user";
                                    }
                                    else if(input.Equals("v"))
                                    {
                                        Console.WriteLine(o.ShowOrder());
                                    }
                                    else if (input.Equals("s"))
                                    {
                                        Console.WriteLine(o.ShowOrder());
                                        Console.WriteLine("Would you like to finalize the order? (y/n)");
                                        input = Console.ReadLine();
                                        if (input.Equals("y"))
                                        {
                                            activity = "user";
                                            AddOrder(db, o);
                                            currentUser.lastOrder[selectedStore.SName] = DateTime.Now;
                                        }
                                        else
                                        {
                                            
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid choice. available choices are p,c,b, and v");
                                    }
                                }

                            }
                            else if (input.Equals("v"))
                            {
                                int n = 1;
                                foreach (Orders orders in db.Orders)
                                {
                                    Order order = Mapper.Map(orders);
                                    if (order.Uname == currentUser.UName)
                                    {
                                        order.CalculateCost();
                                        Console.WriteLine($"Order {n}, {order.ShowOrder()}");
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
                else if (input.Equals("s"))
                {
                    activity = "store";
                    while (activity.Equals("store"))
                    {
                        if (currentStore == null)
                        {
                            Console.WriteLine("Would you like to: ");
                            Console.WriteLine("l:\t Login to an existing store.");
                            Console.WriteLine("c:\t Create a new store.");
                            Console.WriteLine("b:\t Go back to previous menu.");
                            input = Console.ReadLine();
                            if (input.Equals("l"))
                            {
                                activity = "loginstore";
                                while (activity.Equals("loginstore"))
                                {
                                    Console.WriteLine("Enter store name. Type BACK to quit.");
                                    string sn = Console.ReadLine();
                                    if (sn == "BACK")
                                    {
                                        activity = "store";
                                    }
                                    if (db.Store.Any(s => s.Sname == sn) && sn != "BACK")
                                    {
                                        Console.WriteLine("Enter store password.");
                                        string pass = Console.ReadLine();
                                        if (db.Store.Any(s => s.Sname == sn && s.Spass == pass))
                                        {
                                            var query = from s in db.Store
                                                        where s.Sname == sn && s.Spass == pass
                                                        select s;
                                            try
                                            {
                                                Storing.Repositories.Store store = query.Single();
                                                currentStore = Mapper.Map(store);
                                                Console.WriteLine($"Welcome, {currentStore.SName} administrator.");
                                                activity = "store";
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Error, more than one store with that name/password exists.");
                                                throw;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("That store does not exist.");
                                    }
                                }
                            }
                            else if (input.Equals("c"))
                            {
                                activity = "createstore";
                                while (activity.Equals("createstore"))
                                {
                                    string sn = "ERRORIFSHOWN";
                                    string pw;
                                    bool unique = false;
                                    while (!unique)
                                    {
                                        unique = true;
                                        Console.WriteLine("Enter a new store name.");
                                        sn = Console.ReadLine();
                                        if (db.Store.Any(s => s.Sname == sn))
                                        {
                                            unique = false;
                                            Console.WriteLine("That store name is taken");
                                        }
                                    }
                                    Console.WriteLine("Enter a password");
                                    pw = Console.ReadLine();
                                    Domain.Models.Store nstore = new Domain.Models.Store
                                    {
                                        SName = sn,
                                        Password = pw
                                    };
                                    AddStore(db, nstore);
                                    activity = "store";
                                    Console.WriteLine($"Store created with Username: {sn}, Password: {pw}");
                                }
                            }
                            else if(input.Equals("b"))
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
                            Console.WriteLine("l:\t Log out of your store");
                            Console.WriteLine("v:\t View your store's order history");
                            Console.WriteLine("b:\t Go back to previous menu");
                            input = Console.ReadLine();
                            if (input.Equals("l"))
                            {
                                currentStore = null;
                            }
                            else if (input.Equals("v"))
                            {
                                int n = 1;
                                foreach (Orders orders in db.Orders)
                                {
                                    Order order = Mapper.Map(orders);
                                    if (order.Sname == currentStore.SName)
                                    {
                                        order.CalculateCost();
                                        Console.WriteLine($"Order {n}, {order.ShowOrder()}");
                                        n++;
                                    }
                                }
                            }
                            else if (input.Equals("b"))
                            {
                                activity = "menu";
                            }
                        }
                    }
                }
                else if (input.Equals("q"))
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("Error, invalid option. Accepted options are u, s, and q.");
                }
            }
        }
        static IEnumerable<Orders> GetUserOrders(PizzaBoxContext db, User u)
        {
            var query = from o in db.Orders
                        where o.Uname == u.UName
                        select o;
            return query;
        }
        static IEnumerable<Orders> GetStoreOrders(PizzaBoxContext db, Domain.Models.Store s)
        {
            var query = from o in db.Orders
                        where o.Sname == s.SName
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
        static void ShowStores(PizzaBoxContext db)
        {
            foreach (var s in db.Store)
            {
                Console.WriteLine($"{s.Sname}");
            }
        }
    }
}
