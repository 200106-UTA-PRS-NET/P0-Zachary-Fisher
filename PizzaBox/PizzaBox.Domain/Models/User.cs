using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PizzaBox.Domain.Models
{
    public class User
    {
        private string _uName;
        private string _password;
        private int uid;
        public Dictionary<int, DateTime> lastOrder;
        public List<Order> orders;
        
        public string UName { get; set; }
        public string Password { get; set; }
        public int Uid { get; set; }
        public User()
        {

        }
        public User(string _uName, string _password, int uid)
        {
            this._uName = _uName;
            this._password = _password;
            this.uid = uid;
            lastOrder = new Dictionary<int, DateTime>();
            orders = new List<Order>();
            //Console.WriteLine($"User {_uName} with password {_password} created successfully.");
        }
        public void ChangePassword(string current, string n)
        {
            if (_password == current)
            {
                _password = n;
                Console.WriteLine("Password changed successfully.");
            }
            else
            {
                Console.WriteLine("The password you entered is incorrect");
            }
        }
        public void ShowOrders()
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