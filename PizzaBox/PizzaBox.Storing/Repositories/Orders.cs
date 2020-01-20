using System;
using System.Collections.Generic;

namespace PizzaBox.Storing.Repositories
{
    public partial class Orders
    {
        public int Oid { get; set; }
        public int Sid { get; set; }
        public int Uid { get; set; }
        public decimal? Cost { get; set; }
        public string Pizzas { get; set; }

        public virtual Store S { get; set; }
        public virtual Customer U { get; set; }
    }
}
