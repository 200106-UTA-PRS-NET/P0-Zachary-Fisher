using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Pizza
    {
        private string _crust ="thin";
        private int _size = 6;
        public List<string> allowedToppings = new List<string> { "sausage", "pepperoni", "cheese", "bacon", "beef",
            "canadian bacon", "mushrooms", "onions", "green peppers", "olives", "pineapple", "jalapenos", "sauce"};
        public List<string> toppings = new List<string>();
        public Pizza()
        {
            
        }
        public void MakeDefault()
        {
            toppings = new List<string> { "sauce", "cheese" };
        }

        public decimal Cost { get; set; }
        public string Crust
        {
            get => _crust;
            set
            {
                if(value == "thin"||value=="sicilian"||value=="deep dish")
                {
                    _crust = value;
                    setCost();
                }
                else
                {
                    throw new ArgumentException("The offered crusts are thin, sicilian, and deep dish", nameof(value));
                }
            }
        }
        public int Size
        {
            get => _size;
            set
            {
                if (value == 6 || value == 8 || value == 10 || value == 12 || value == 14)
                {
                    _size = value;
                    setCost();
                }
                else
                {
                    throw new ArgumentException("The size must be 6, 8, 10, 12, or 14", nameof(value));
                }
            }
        }
        //(a.Equals("sausage") || a.Equals("pepperoni") || a.Equals("cheese") || a.Equals("bacon") || a.Equals("beef") ||
        //a.Equals("canadian bacon") || a.Equals("mushrooms") || a.Equals("onions") || a.Equals("green peppers") ||
        //        a.Equals("olives") || a.Equals("pineapple") || a.Equals("jalapenos") || a.Equals("sauce")
        public void AddTopping(string a)
        { 
            if (allowedToppings.Contains(a))
            {
                if (toppings.Count < 5)
                {
                    toppings.Add(a);
                    setCost();
                }
                else
                {
                    Console.WriteLine("Too many toppings"); ;
                }
            }
            else
            {
                Console.WriteLine("Not a valid topping");           
            }
        }
        public void RemoveTopping(string a)
        {
            if(toppings.Contains(a))
            {
                toppings.Remove(a);
                setCost();
            }
            else
            {
                Console.WriteLine("You can only remove a topping that is on the pizza");
            }
        }
        public string Toppings()
        {
            StringBuilder r = new StringBuilder();
            foreach (string t in toppings)
            {
                r.Append(t);
                r.Append(", ");
            }
            string re = Convert.ToString(r);
            return re;
        }
        private void setCost()
        {
            decimal cost = 0m;
            if (Crust.Equals("thin"))
            {
                cost += .50m;
            }
            else if (Crust.Equals("sicilian"))
            {
                cost += 1m;
            }
            else
            {
                cost += 1.5m;
            }
            cost += (decimal)(.25 * _size + 2.5);
            foreach (var topping in toppings)
            {
                cost += .25m;
            }
            Cost = cost;
        }
        public void Preset(string s)
        {
            if (s.Equals("hawaiian"))
            {
                toppings = new List<string> { "sauce", "cheese", "pineapple", "canadian bacon" };
                setCost();
            }
            else if(s.Equals("3 meat"))
            {
                toppings = new List<string> { "sauce", "cheese", "sausage", "canadian bacon", "pepperoni" };
                setCost();
            }
            else if(s.Equals("supreme"))
            {
                toppings = new List<string> { "sauce", "cheese", "sausage", "mushroom", "green pepper", "pepperoni" };
                setCost();
            }
            else if(s.Equals("meat lover"))
            {
                toppings = new List<string> { "sauce", "cheese", "sausage", "canadian bacon", "pepperoni", "bacon", "beef" };
                setCost();
            }
            else if(s.Equals("cheeseburger"))
            {
                toppings = new List<string> { "sauce", "cheese", "beef", "pickles", "mushrooms" };
                setCost();
            }
            else if(s.Equals("bacon cheeseburger"))
            {
                toppings = new List<string> { "sauce", "cheese", "beef", "pickles", "mushrooms", "bacon" };
                setCost();
            }
            else
            {
                throw new ArgumentException("The special pizza options are hawaiian, 3 meat, supreme, meat lover, cheeseburger, and " +
                    "bacon cheeseburger.");
            }
        }
    }
}
