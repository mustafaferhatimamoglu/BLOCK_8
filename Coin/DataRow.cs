using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOCK_8.Coin
{
    internal class DataRow
    {
        public long TimeStamp { get; set; }//0
        public double SizeVolume { get; set; }//1
        public double ClosePrice { get; set; }//2
        public double HighestPrice { get; set; }//3
        public double LowestPrice { get; set; }//4
        public double OpenPrice { get; set; }//5
        public double TradingVolume { get; set; }//6
        public bool IsClosed { get; set; }//7
    }

}
