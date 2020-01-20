using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Store
    {
        public string SName { get; set; }
        public string Password { get; set; }
        public List<Order> orders;
        public Store()
        {

        }
        public Store(string sName, string password)
        {
            Password = password;
            SName = sName;
        }
        public void showOrders()
        {
            Console.WriteLine("--------------------------------------------------------");
            foreach (var o in orders)
            {
                Console.WriteLine(o.ShowOrder());
                Console.WriteLine("--------------------------------------------------------");
            }
        }

    }
}
