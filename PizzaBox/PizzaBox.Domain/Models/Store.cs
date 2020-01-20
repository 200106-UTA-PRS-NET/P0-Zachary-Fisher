using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Store
    {
        private int _sid;
        private string _sName;
        private string _password;
        public int Sid { get; set; }
        public string SName { get; set; }
        public string Password { get; set; }
        public List<Order> orders;
        public Store()
        {

        }
        public Store(string sName, string password, int sid)
        {
            _sid = sid;
            _password = password;
            _sName = sName;
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
