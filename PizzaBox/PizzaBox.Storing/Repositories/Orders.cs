using System;
using System.Collections.Generic;

namespace PizzaBox.Storing.Repositories
{
    public partial class Orders
    {
        public int Oid { get; set; }
        public string Sname { get; set; }
        public string Uname { get; set; }
        public decimal? Cost { get; set; }
        public string Pizzas { get; set; }

        public virtual Store SnameNavigation { get; set; }
        public virtual Customer UnameNavigation { get; set; }
    }
}
