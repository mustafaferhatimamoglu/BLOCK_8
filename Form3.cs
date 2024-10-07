using ScottPlot.Plottables;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLOCK_8
{
    public partial class Form3 : Form
    {
        DataLogger logger = new DataLogger();
        DataLogger logger_USDT = new DataLogger();
        DataLogger logger_USDT_locked = new DataLogger();
        DataLogger logger_COIN = new DataLogger();
        DataLogger logger_COIN_locked = new DataLogger();

        Coin.Balance balance = new Coin.Balance();

        List<Coin.Order> orders = new List<Coin.Order>();
        List<Coin.Order> orders_Filled = new List<Coin.Order>();

        Gate.IO.Functions f = new Gate.IO.Functions();

        List<OHLC> OHLCs = new List<OHLC>();
        public Form3()
        {
            InitializeComponent();
            Setups();
            this.Load += CommonFunctions.Form.This_Load;


            Coin.Data coinData = f.CreateData("BLOCKASSET_USDT", "30m");

            foreach (var item in coinData.Rows)
            {
                OHLC a = new();
                a.DateTime = CommonFunctions.Converter.UnixTimeStampToDateTime(item.TimeStamp);
                a.TimeSpan = TimeSpan.FromSeconds(coinData.Interval);//TimeSpan.FromDays(1);
                a.Low = item.LowestPrice;
                a.Open = item.OpenPrice;
                a.Close = item.ClosePrice;
                a.High = item.HighestPrice;
                OHLCs.Add(a);
            }
            formsPlot1.Plot.Add.Candlestick(OHLCs);




            if (true)
            {
                var startSimulationTime = coinData.SecondStart;

                var tempList = new List<Coin.DataRow>();
                foreach (var item in coinData.Rows)
                {
                    if (item.TimeStamp > startSimulationTime)
                    {
                        tempList.Add(item);
                    }
                }
                coinData.Rows = tempList;
            }


            balance.USDT = 0;
            balance.COIN = 10000;
            double everyUSDT = 2;

            logger = formsPlot1.Plot.Add.DataLogger(); logger.ManageAxisLimits = false; logger.Color = ScottPlot.Colors.Green;
            logger_USDT = formsPlot1.Plot.Add.DataLogger(); logger_USDT.ManageAxisLimits = false; logger_USDT.Color = ScottPlot.Colors.Red; logger_USDT.LineWidth = 2;
            logger_USDT_locked = formsPlot1.Plot.Add.DataLogger(); logger_USDT_locked.ManageAxisLimits = false; logger_USDT_locked.Color = ScottPlot.Colors.DarkRed;
            logger_COIN = formsPlot1.Plot.Add.DataLogger(); logger_COIN.ManageAxisLimits = false; logger_COIN.Color = ScottPlot.Colors.Blue; logger_COIN.LineWidth = 2;
            logger_COIN_locked = formsPlot1.Plot.Add.DataLogger(); logger_COIN_locked.ManageAxisLimits = false; logger_COIN_locked.Color = ScottPlot.Colors.DarkBlue;
            formsPlot1.Plot.Axes.DateTimeTicksBottom();
            //formsPlot1.Plot.ShowLegend(Alignment.UpperLeft);

            int jumpthrough =  2800;// 500;
            foreach (var item in coinData.Rows)
            {
                if (jumpthrough > 0)
                {
                    jumpthrough--;
                    if (jumpthrough == 0)
                    {
                        //var _sellCoinAmount = 5000;//everyUSDT / sellPrice;
                        //var _sellOrder = Order.CreateOrder("limit", "sell", item.OpenPrice.ToString(), _sellCoinAmount.ToString());
                        //balance.COIN_locked += double.Parse(_sellOrder.Amount);
                        //balance.COIN -= double.Parse(_sellOrder.Amount);
                        //orders.Add(_sellOrder);

                        //balance.COIN -= 5000;
                        //balance.USDT += item.OpenPrice * 5000;

                        //balance.COIN -= 5000;
                        //balance.USDT += item.OpenPrice * 5000;
                        formsPlot1.Plot.Add.VerticalLine(CommonFunctions.Converter.UnixTimeStampToDateTime(item.TimeStamp).ToOADate());
                    }
                    continue;
                }

                //total balance
                var TotalBalance = balance.USDT + balance.USDT_locked;
                TotalBalance += (balance.COIN + balance.COIN_locked) * item.OpenPrice;

                //var time1 = Convert.ToDateTime((double)item.TimeStamp);
                var time1 = CommonFunctions.Converter.UnixTimeStampToDateTime(item.TimeStamp);
                var time2 = time1.ToOADate();
                logger.Add(time2, TotalBalance);
                logger_USDT.Add(time2, balance.USDT);
                logger_USDT_locked.Add(time2, balance.USDT_locked);
                logger_COIN.Add(time2, balance.COIN);
                logger_COIN_locked.Add(time2, balance.COIN_locked);



                // Zamanın akması
                //System.Threading.Thread.Sleep(60000); // Her döngüde 1 dakika bekle

                // Döngü sırasında openPrice güncellenebilir

                //Check Orders
                for (int i = orders.Count - 1; i >= 0; i--)
                {
                    if (orders[i].Side == "buy" && Convert.ToDouble(orders[i].Price) > item.LowestPrice)
                    {
                        balance.USDT_locked -= everyUSDT;
                        balance.COIN += Convert.ToDouble(orders[i].Amount) * 0.997;
                        orders_Filled.Add(orders[i]);
                        //orders.RemoveAt(i);
                        orders[i].Side = "fill-buy";
                        orders[i].Date_Fill = time2;
                    }
                    else if (orders[i].Side == "sell" && Convert.ToDouble(orders[i].Price) < item.HighestPrice)
                    {
                        balance.COIN_locked -= Convert.ToDouble(orders[i].Amount);
                        balance.USDT += Convert.ToDouble(orders[i].Price) * Convert.ToDouble(orders[i].Amount) * 0.997;
                        orders_Filled.Add(orders[i]);
                        //orders.RemoveAt(i);
                        orders[i].Side = "fill-sell";
                        orders[i].Date_Fill = time2;
                    }
                }
                if (balance.COIN_locked > 9000)
                {

                }

                // Yeni order ekleme
                for (int i = orders.Count - 1; i >= 0; i--)
                {
                    if (orders.Count - i > 2)
                    {
                        break;
                    }
                    if (orders[i].Side == "fill-sell" || orders[i].Side == "fill-buy")
                    {

                        double buyPrice = item.OpenPrice * 0.985;
                        double sellPrice = item.OpenPrice * 1.015;

                        var buyCoinAmount = everyUSDT / buyPrice;
                        var buyOrder = Coin.Order.CreateOrder("limit", "pre-buy", buyPrice.ToString(), buyCoinAmount.ToString(), time2);
                        //balance.USDT_locked += everyUSDT;
                        //balance.USDT -= everyUSDT;
                        orders.Add(buyOrder);

                        var sellCoinAmount = everyUSDT / sellPrice;
                        var sellOrder = Coin.Order.CreateOrder("limit", "pre-sell", sellPrice.ToString(), sellCoinAmount.ToString(), time2);
                        //balance.COIN_locked += double.Parse(sellOrder.Amount);
                        //balance.COIN -= double.Parse(sellOrder.Amount);
                        orders.Add(sellOrder);

                        break;
                    }
                }
                if (orders.Count == 0)
                {
                    double buyPrice = item.OpenPrice * 0.985;
                    double sellPrice = item.OpenPrice * 1.015;

                    var buyCoinAmount = everyUSDT / buyPrice;
                    var buyOrder = Coin.Order.CreateOrder("limit", "pre-buy", buyPrice.ToString(), buyCoinAmount.ToString(), time2);
                    //balance.USDT_locked += everyUSDT;
                    //balance.USDT -= everyUSDT;
                    orders.Add(buyOrder);

                    var sellCoinAmount = everyUSDT / sellPrice;
                    var sellOrder = Coin.Order.CreateOrder("limit", "pre-sell", sellPrice.ToString(), sellCoinAmount.ToString(), time2);
                    //balance.COIN_locked += double.Parse(sellOrder.Amount);
                    //balance.COIN -= double.Parse(sellOrder.Amount);
                    orders.Add(sellOrder);
                }

                //Check Balance and pre-order to order
                //part buy
                for (int i = 0; i <= orders.Count - 1; i++)
                {
                    if (balance.USDT <= everyUSDT)
                    {
                        break;
                    }
                    if (orders[i].Side == "pre-buy")
                    {
                        orders[i].Side = "buy";
                        balance.USDT_locked += everyUSDT;
                        balance.USDT -= everyUSDT;
                    }
                }
                for (int i = 0; i <= orders.Count - 1; i++)
                {
                    if (balance.COIN <= everyUSDT)
                    {
                        break;
                    }
                    if (orders[i].Side == "pre-sell")
                    {
                        orders[i].Side = "sell";
                        balance.COIN_locked  += double.Parse(orders[i].Amount);
                        balance.COIN -= double.Parse(orders[i].Amount);
                    }
                }

            }


















            foreach (var item in orders)
            {
                if (item.Side == "fill-sell" || item.Side == "fill-buy")

                    formsPlot1.Plot.Add.Line(
                        item.Date_Start, Convert.ToDouble(item.Price),
                        item.Date_Fill, Convert.ToDouble(item.Price)
                        );
                else
                {
                    formsPlot1.Plot.Add.Line(
                            item.Date_Start, Convert.ToDouble(item.Price),
                            item.Date_Start + 300, Convert.ToDouble(item.Price)
                            );
                }
            }

            var form2 = new Form2();
            form2.ShowDialog();
        }

        private void Setups()
        {
            Setup_Graph();
        }


        private ScottPlot.WinForms.FormsPlot formsPlot1;
        void Setup_Graph()
        {
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();

            formsPlot1.DisplayScale = 1F;
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(800, 450);
            formsPlot1.TabIndex = 0;
            Controls.Add(formsPlot1);

            formsPlot1.Plot.Axes.DateTimeTicksBottom();
            formsPlot1.Plot.ShowLegend(Alignment.UpperLeft);
        }
    }
}