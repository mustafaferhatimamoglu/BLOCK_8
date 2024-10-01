using Io.Gate.GateApi.Api;
using Io.Gate.GateApi.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace BLOCK_8.Gate.IO
{
    internal class Functions
    {
        private const string ApiKey = "7b376761dab8e7188724f51744a9f9c8";
        private const string ApiSecret = "08f96b4f75947b29ef74e242907e2b7083b356d8c245c34d8568d7ffe1691ad8";

        private readonly SpotApi _spotApi;
        //public Functions(string apiKey, string apiSecret)
        public Functions()
        {
            var config = new Io.Gate.GateApi.Client.Configuration
            {
                BasePath = "https://api.gateio.ws/api/v4"
            };
            config.SetGateApiV4KeyPair(ApiKey, ApiSecret);
            _spotApi = new SpotApi(config);
        }
        private DateTime GetServerTime()
        {
            try
            {
                var result = _spotApi.GetSystemTime();
                return DateTimeOffset.FromUnixTimeMilliseconds(result.ServerTime).DateTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving server time: {ex.Message}");
                return DateTime.Now;
            }
        }
        private CurrencyPair GetCurrencyPair(string currencyPair)
        {
            var result = _spotApi.GetCurrencyPair(currencyPair);
            return result;
        }
        private Coin.Data GetNewerData(Coin.Data r_CoinData)
        {
            var time = r_CoinData.Rows[^1].TimeStamp + r_CoinData.Interval;
            while (true)
            {
                var result = _spotApi.ListCandlesticks(r_CoinData.Id, 999, time, null, r_CoinData.Interval_string);
                if (result.Count == 0) break;
                foreach (var item in result)
                {
                    Coin.DataRow row = new Coin.DataRow();
                    row.TimeStamp = Convert.ToInt64(item[0], CultureInfo.InvariantCulture);
                    row.SizeVolume = Convert.ToDouble(item[1], CultureInfo.InvariantCulture);
                    row.ClosePrice = Convert.ToDouble(item[2], CultureInfo.InvariantCulture);
                    row.HighestPrice = Convert.ToDouble(item[3], CultureInfo.InvariantCulture);
                    row.LowestPrice = Convert.ToDouble(item[4], CultureInfo.InvariantCulture);
                    row.OpenPrice = Convert.ToDouble(item[5], CultureInfo.InvariantCulture);
                    row.TradingVolume = Convert.ToDouble(item[6], CultureInfo.InvariantCulture);
                    row.IsClosed = Convert.ToBoolean(item[7], CultureInfo.InvariantCulture);
                    if (r_CoinData.Rows[^1].TimeStamp == row.TimeStamp)
                    {

                    }
                    else
                    {
                        r_CoinData.Rows.Add(row);
                    }
                    //if (!row.IsClosed) { break; }
                }
                time = long.Parse(result[^1][0]) + r_CoinData.Interval;
                if (!r_CoinData.Rows[^1].IsClosed) break;
            }
            return r_CoinData;
        }
        private Coin.Data GetOldData(Coin.Data r_CoinData)
        {
            var time = r_CoinData.SecondStart;
            while (true)
            {
                List<List<string>> result = new List<List<string>>();
                try
                {
                    result = _spotApi.ListCandlesticks(r_CoinData.Id, 999, null, time, r_CoinData.Interval_string);
                }
                catch (Exception)
                {
                    break;
                    //throw;
                }
                if (result.Count == 0) break;
                foreach (var item in result)
                {
                    Coin.DataRow row = new Coin.DataRow();
                    row.TimeStamp = Convert.ToInt64(item[0], CultureInfo.InvariantCulture);
                    row.SizeVolume = Convert.ToDouble(item[1], CultureInfo.InvariantCulture);
                    row.ClosePrice = Convert.ToDouble(item[2], CultureInfo.InvariantCulture);
                    row.HighestPrice = Convert.ToDouble(item[3], CultureInfo.InvariantCulture);
                    row.LowestPrice = Convert.ToDouble(item[4], CultureInfo.InvariantCulture);
                    row.OpenPrice = Convert.ToDouble(item[5], CultureInfo.InvariantCulture);
                    row.TradingVolume = Convert.ToDouble(item[6], CultureInfo.InvariantCulture);
                    row.IsClosed = Convert.ToBoolean(item[7], CultureInfo.InvariantCulture);
                    r_CoinData.Rows.Add(row);
                }
                time = long.Parse(result[0][0]) - r_CoinData.Interval;

            }
            r_CoinData.Rows = r_CoinData.Rows.OrderBy(o => o.TimeStamp).ToList();
            return r_CoinData;
        }

        public Coin.Data CreateData(string coinPair_Name, string interval)
        {
            var coinData = new Coin.Data();
            coinData.Id = coinPair_Name;
            coinData.Interval_string = interval;
            var text_Coin = CommonFunctions.File.Open(coinData.path);
            if (text_Coin != "NULL")
            {
                coinData = JsonConvert.DeserializeObject<Coin.Data>(text_Coin);
                coinData.Rows.RemoveAt(coinData.Rows.Count - 1);
                coinData = GetNewerData(coinData);

            }
            else
            {
                coinData = CreateData_Header(coinData);
                coinData = GetOldData(coinData);
                coinData = GetNewerData(coinData);
            }

            string json = JsonConvert.SerializeObject(coinData, Formatting.Indented);
            CommonFunctions.File.Save(coinData.path, json);

            return coinData;

        }
        private Coin.Data CreateData_Header(Coin.Data r_CoinData)
        {
            var coinData_Header = GetCurrencyPair(r_CoinData.Id);
            //var r_CoinData = new Coin.Data();
            //r_CoinData.Id = coinData_Header.Id;
            r_CoinData.Base = coinData_Header.Base;
            r_CoinData.Quote = coinData_Header.Quote;
            r_CoinData.Fee = coinData_Header.Fee;
            r_CoinData.MinBaseAmount = coinData_Header.MinBaseAmount;
            r_CoinData.MinQuoteAmount = coinData_Header.MinQuoteAmount;
            r_CoinData.MaxBaseAmount = coinData_Header.MaxBaseAmount;
            r_CoinData.MaxQuoteAmount = coinData_Header.MaxQuoteAmount;
            r_CoinData.AmountPrecision = coinData_Header.AmountPrecision;
            r_CoinData.Precision = coinData_Header.Precision;
            r_CoinData.SellStart = coinData_Header.SellStart;
            r_CoinData.BuyStart = coinData_Header.BuyStart;

            return r_CoinData;
        }
        private Coin.Data CreateData_Rows(Coin.Data r_CoinData)
        {

            return r_CoinData;
        }
    }
}
