using ScottPlot;
using ScottPlot.Plottables;
using static log4net.Appender.ColoredConsoleAppender;

namespace BLOCK_8
{
    public partial class Form1 : Form
    {
        private void Setups()
        {

        }
        public Form1()
        {
            InitializeComponent();
            Setups();
            this.Load += This_Load;

            Setup_Graph();

            //Coin.Data coinData = f.CreateData("BLOCKASSET_USDT", "1h");
            Coin.Data coinData = f.CreateData("BLOCKASSET_USDT", "4h");

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
            double everyUSDT = 3;

            logger = formsPlot1.Plot.Add.DataLogger(); logger.ManageAxisLimits = false; logger.Color = ScottPlot.Colors.Green;
            logger_USDT = formsPlot1.Plot.Add.DataLogger(); logger_USDT.ManageAxisLimits = false; logger_USDT.Color = ScottPlot.Colors.Red; logger_USDT.LineWidth = 2;
            logger_USDT_locked = formsPlot1.Plot.Add.DataLogger(); logger_USDT_locked.ManageAxisLimits = false; logger_USDT_locked.Color = ScottPlot.Colors.DarkRed;
            logger_COIN = formsPlot1.Plot.Add.DataLogger(); logger_COIN.ManageAxisLimits = false; logger_COIN.Color = ScottPlot.Colors.Blue; logger_COIN.LineWidth = 2;
            logger_COIN_locked = formsPlot1.Plot.Add.DataLogger(); logger_COIN_locked.ManageAxisLimits = false; logger_COIN_locked.Color = ScottPlot.Colors.DarkBlue;
            formsPlot1.Plot.Axes.DateTimeTicksBottom();
            formsPlot1.Plot.ShowLegend(Alignment.UpperLeft);

            int jumpthrough = 500;// 500;
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
                        balance.COIN -= 5000;
                        balance.USDT += item.OpenPrice * 5000;
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
                // Yeni order ekleme
                double buyPrice = item.OpenPrice * 0.99;
                double sellPrice = item.OpenPrice * 1.01;

                var buyCoinAmount = everyUSDT / buyPrice;
                var buyOrder = Coin.Order.CreateOrder("limit", "buy", buyPrice.ToString(), buyCoinAmount.ToString());
                balance.USDT_locked += everyUSDT;
                balance.USDT -= everyUSDT;
                orders.Add(buyOrder);

                var sellCoinAmount = everyUSDT / sellPrice;
                var sellOrder = Coin.Order.CreateOrder("limit", "sell", sellPrice.ToString(), sellCoinAmount.ToString());
                balance.COIN_locked += double.Parse(sellOrder.Amount);
                balance.COIN -= double.Parse(sellOrder.Amount);
                orders.Add(sellOrder);

                // Zaman�n akmas�
                //System.Threading.Thread.Sleep(60000); // Her d�ng�de 1 dakika bekle

                // D�ng� s�ras�nda openPrice g�ncellenebilir

                //Check Orders
                for (int i = orders.Count - 1; i >= 0; i--)
                {
                    if (orders[i].Side == "buy" && Convert.ToDouble(orders[i].Price) > item.LowestPrice)
                    {
                        balance.USDT_locked -= everyUSDT;
                        balance.COIN += Convert.ToDouble(orders[i].Amount) * 0.997;
                        orders_Filled.Add(orders[i]);
                        orders.RemoveAt(i);
                    }
                    else if (orders[i].Side == "sell" && Convert.ToDouble(orders[i].Price) < item.HighestPrice)
                    {
                        balance.COIN_locked -= Convert.ToDouble(orders[i].Amount);
                        balance.USDT += Convert.ToDouble(orders[i].Price) * Convert.ToDouble(orders[i].Amount) * 0.997;
                        orders_Filled.Add(orders[i]);
                        orders.RemoveAt(i);
                    }
                    //if (orders[i].Type == "filled")
                    //{
                    //    balance.UpdateBalance(orders[i]);
                    //    orders.RemoveAt(i);
                    //}
                }
                if (balance.COIN_locked > 9000)
                {

                }
            }
        }

        private void This_Load(object? sender, EventArgs e)
        {
            CommonFunctions.Form.Setup_Form_Right_2K(this);
        }

        Gate.IO.Functions f = new Gate.IO.Functions();

        List<OHLC> OHLCs = new List<OHLC>();

        DataLogger logger = new DataLogger();
        DataLogger logger_USDT = new DataLogger();
        DataLogger logger_USDT_locked = new DataLogger();
        DataLogger logger_COIN = new DataLogger();
        DataLogger logger_COIN_locked = new DataLogger();


        //double balance_USDT = 600.0;
        //double balance_Coin = 0.0;
        Coin.Balance balance = new Coin.Balance();
        List<Coin.Order> orders = new List<Coin.Order>();
        List<Coin.Order> orders_Filled = new List<Coin.Order>();
         
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
        }
    }
}
