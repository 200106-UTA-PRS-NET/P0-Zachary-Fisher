using System;
using System.Collections.Generic;

namespace PizzaBox.Storing.Repositories
{
    public partial class Store
    {
        public Store()
        {
            Orders = new HashSet<Orders>();
        }

        public string Sname { get; set; }
        public string Spass { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
