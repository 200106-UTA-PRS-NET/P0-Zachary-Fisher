using System;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;
using System.Collections.Generic;
using PizzaBox.Storing;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Linq;

namespace PizzaBox.Storing
{
    public class Mapper
    {
        public static Order Map(Orders o)
        {
            return new Order
            {
                Cost = (decimal)o.Cost,
                Sname = o.Sname,
                Uname = o.Uname,
                pizzas = JsonConvert.DeserializeObject<List<Pizza>>(o.Pizzas)
            };
        }
        public static Orders Map(Order o)
        {
            return new Orders
            {
                Cost = o.Cost,
                Sname = o.Sname,
                Uname = o.Uname,
                Pizzas = JsonConvert.SerializeObject(o.pizzas)
            };
        }
        public static User Map(Customer c)
        {
            List<Orders> s = c.Orders.ToList();
            List<Order> p = new List<Order>();
            foreach (var o in s)
            {
                p.Add(Map(o));
            }
            return new User
            {
                UName = c.Uname,
                Password = c.Pass,
                lastOrder = JsonConvert.DeserializeObject<Dictionary<int, DateTime>>(c.Lastorder),
                orders = p
            };
        }
        public static Customer Map(User u)
        {
            List<Orders> p = new List<Orders>();
            if (u.orders != null)
            {
                List<Order> s = u.orders.ToList();
                foreach (var o in s)
                {
                    p.Add(Map(o));

                }
            }
            return new Customer
            {
                Pass = u.Password,
                Uname = u.UName,
                Orders = p,
                Lastorder = JsonConvert.SerializeObject(u.lastOrder)
            };
        }
        public static PizzaBox.Domain.Models.Store Map(PizzaBox.Storing.Repositories.Store s)
        {
            List<Orders> z = s.Orders.ToList();
            List<Order> p = new List<Order>();
            foreach (var o in z)
            {
                p.Add(Map(o));
            }
            return new Domain.Models.Store
            {
                SName = s.Sname,
                Password = s.Spass,
                orders = p
            };
        }
        public static PizzaBox.Storing.Repositories.Store Map(PizzaBox.Domain.Models.Store s)
        {
            List<Order> z = s.orders.ToList();
            List<Orders> p = new List<Orders>();
            foreach (var o in z)
            {
                p.Add(Map(o));
            }
            return new Storing.Repositories.Store
            {
                Sname = s.SName,
                Spass=s.Password,
                Orders = p
            };
        }
    }
}
