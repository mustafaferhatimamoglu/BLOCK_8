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
        public string MinBaseAmount { get; set; }
        public string MinQuoteAmount { get; set; }
        public string MaxBaseAmount { get; set; }
        public string MaxQuoteAmount { get; set; }
        public int AmountPrecision { get; set; }
        public int Precision { get; set; }
        public long SellStart { get; set; }
        public long BuyStart { get; set; }
        public List<DataRow> Rows = new List<DataRow>();



        public long EarliestStart
        {
            get
            {
                if (BuyStart > SellStart)
                    return SellStart;
                return BuyStart;
            }
        }
        public long SecondStart
        {
            get
            {
                if (BuyStart > SellStart)
                    return BuyStart;
                return SellStart;
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
                        MessageBox.Show("UNRECOGNIZED INTERVAL");
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
