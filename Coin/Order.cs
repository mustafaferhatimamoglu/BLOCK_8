using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOCK_8.Coin
{
    internal class Order
    {
        public string Type { get; set; }
        public string Side { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }

        public static Order CreateOrder(string type, string side, string price, string amount)
        {
            Order order = new Order();
            order.Type = type;
            order.Side = side;
            order.Price = price;
            order.Amount = amount;
            return order;
        }

    }
}
