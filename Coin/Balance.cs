using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOCK_8.Coin
{
    internal class Balance
    {
        public double USDT { get; set; }
        public double COIN { get; set; }
        public double USDT_locked { get; set; }
        public double COIN_locked { get; set; }

        //public void UpdateBalance(Order order)
        //{
        //    if (order.Side == "buy" && order.Type == "filled")
        //    {
        //        USDT_locked -= double.Parse(order.Amount);
        //        COIN += double.Parse(order.Amount) / double.Parse(order.Price);
        //    }
        //    else if (order.Side == "sell" && order.Type == "filled")
        //    {
        //        COIN_locked -= double.Parse(order.Amount) / double.Parse(order.Price);
        //        USDT += double.Parse(order.Amount) * double.Parse(order.Price);
        //    }
        //}
    }
}
