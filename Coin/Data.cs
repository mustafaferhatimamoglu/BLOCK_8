using Io.Gate.GateApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLOCK_8.Coin
{
    internal class Data
    {
        public string Id { get; set; }
        public string Base { get; set; }
        public string Quote { get; set; }
        public string Fee { get; set; }
        public string min_base_amount { get; set; }
        public string min_quote_amount { get; set; }
        public string max_quote_amount { get; set; }
        //public string MaxQuoteAmount { get; set; }
        public int amount_precision { get; set; }
        public int precision { get; set; }
        public string trade_status { get; set; }
        public long sell_start { get; set; }
        public long buy_start { get; set; }


        public List<DataRow> Rows = new List<DataRow>();



        public long EarliestStart
        {
            get
            {
                if (buy_start > sell_start)
                    return sell_start;
                return buy_start;
            }
        }
        public long SecondStart
        {
            get
            {
                if (buy_start > sell_start)
                    return buy_start;
                return sell_start;
            }
        }
        public string Interval_string { get; set; }
        public long Interval
        {
            get
            {
                switch (Interval_string)
                {
                    case "1s": return 1;
                    case "10s": return 10;
                    case "1m": return 60;
                    case "5m": return 300;
                    case "30m": return 1800;
                    //case "1d": return 84600;                    
                    case "1h": return 3600;
                    case "4h": return 4*3600;
                    case "1d": return 84600;
                    default:
                        //MessageBox.Show("UNRECOGNIZED INTERVAL");
                        return 0;
                        break;
                }
            }
        }
        public string path
        {
            get
            { return "CoinData\\" + Id + "\\" + Interval_string + ".mfi"; }
        }
    }
}
