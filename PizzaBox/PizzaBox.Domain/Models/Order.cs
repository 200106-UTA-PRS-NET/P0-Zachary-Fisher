using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Order
    {
        public List<Pizza> pizzas;
        public decimal Cost { get; set; }
        public string Uname { get; set; }

        public string Sname { get; set; }
        public Order()
        {
            pizzas = new List<Pizza>();
            Cost = 0;
        }
        public Order(string uname, string sname)
        {
            Uname = uname;
            Sname = sname;
            pizzas = new List<Pizza>();
            Cost = 0;
        }
        public void AddPizza(Pizza p)
        {
            if (pizzas.Count < 100)
            {
                pizzas.Add(p);
                CalculateCost();
                if (Cost > 250m)
                {
                    decimal t = Cost;
                    RemovePizza(p);
                    throw new InvalidOperationException($"Your order total cannot exceed $250 (${t})");
                }
            }
            else
            {
                throw new InvalidOperationException("You cannot exceed 100 pizzas in an order");
            }
        }
        public void RemovePizza(Pizza p)
        {
            if (pizzas.Contains(p))
            {
                pizzas.Remove(p);
                CalculateCost();
            }
            else
            {
                throw new ArgumentException("You can only remove a Pizza that is part of your order");
            }
        }
        public void RemovePizza(int n)
        {
            if (pizzas.Count > n)
            {
                pizzas.RemoveAt(n+1);
                CalculateCost();
            }
            else
            {
                throw new ArgumentOutOfRangeException("You can only remove a pizza on your order list.");
            }
        }
        public string ShowOrder()
        {
            CalculateCost();
            StringBuilder b = new StringBuilder();
            int n = 1;
            b.Append($"{ Uname } at {Sname}, Price: ${Cost}\n");
            foreach (Pizza p in pizzas)
            {
                b.Append($"Pizza:{n} {p.Size}\", Crust: {p.Crust}, {p.Toppings()}\n");
                n++;
            }
            return b.ToString();
        }
        public void CalculateCost()
        {
            decimal c = 0m;
            foreach (Pizza p in pizzas)
            {
                c += p.Cost;
            }
            Cost = c;
        }
        
    }
}
