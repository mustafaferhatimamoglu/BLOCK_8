using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOCK_8.Coin
{
    internal class Order
    {
        static int id2 = 0;
        public int ID { get; set; }
        public string Type { get; set; }
        public string Side { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }

        public double Date_Start { get; set; }
        public double Date_Fill { get; set; }
        public static Order CreateOrder(
            string type, string side,
            string price, string amount,
            double date_Start
            //,double date_Fill
            )
        {
            id2++;
            Order order = new Order();
            order.ID = id2 / 2;
            order.Type = type;
            order.Side = side;
            order.Price = price;
            order.Amount = amount;
            order.Date_Start = date_Start;
            //order.Date_Fill = date_Fill;
            return order;
        }

    }
}