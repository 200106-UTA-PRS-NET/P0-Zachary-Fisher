using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Order
    {
        public List<Pizza> pizzas;
        private decimal _cost;
        private int _uid;
        private int _sid;
        public decimal Cost { get; set; }
        public int UID { get; set; }

        public int SID { get; set; }
        public Order()
        {

        }
        public Order(int uid, int sid)
        {
            _uid = uid;
            _sid = sid;
            pizzas = new List<Pizza>();
            _cost = 0;
        }
        public void AddPizza(Pizza p)
        {
            if (pizzas.Count < 100)
            {
                pizzas.Add(p);
                CalculateCost();
                if (_cost > 250m)
                {
                    decimal t = _cost;
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
                pizzas.RemoveAt(n);
                CalculateCost();
            }
            else
            {
                throw new ArgumentOutOfRangeException("You can only remove a pizza on your order list.");
            }
        }
        public string ShowOrder()
        {
            StringBuilder b = new StringBuilder();
            int n = 0;
            foreach (Pizza p in pizzas)
            {
                b.Append($"{n} {p.Size}, {p.Crust}, {p.Toppings()}\n");
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
            _cost = c;
        }
    }
}
