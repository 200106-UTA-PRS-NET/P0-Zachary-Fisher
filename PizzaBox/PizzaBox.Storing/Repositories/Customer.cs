using System;
using System.Collections.Generic;

namespace PizzaBox.Storing.Repositories
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        public int Uid { get; set; }
        public string Uname { get; set; }
        public string Pass { get; set; }
        public string Lastorder { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
