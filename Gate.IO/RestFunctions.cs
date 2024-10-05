using Nancy.Json;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOCK_8.Gate.IO
{
    internal class RestFunctions
    {
        //static RestClient client = new RestClient("https://api.gateio.ws/api/v4/spot/candlesticks?currency_pair=BLOCKASSET_USDT&limit=10&interval=10s");
        static RestClient client = new RestClient("https://api.gateio.ws/api/v4/spot/candlesticks");

        static RestRequest request = new RestRequest();
        static RestFunctions()
        {
            request.Method = Method.Get;
            request.AddHeader("Accept", "application/json");
            //'Accept': 'application/json', 'Content-Type': 'application/json'
            request.AddHeader("Content-Type", "application/json");
        }
        public static void GetRows(Coin.Data coinInfo)
        {
            ClearParameters();

            //request.Parameters.Clear();
            //request.AddParameter("application/json", strJSONContent, ParameterType.RequestBody);

            //request.Parameters.RemoveParameter("Header User-Agent");
            request.AddParameter("currency_pair", coinInfo.Id);
            request.AddParameter("limit", "999");
            request.AddParameter("interval", coinInfo.Interval_string);
            request.AddParameter("from", coinInfo.SecondStart);

            //currency_pair=BLOCKASSET_USDT&limit=10&interval=10s
            var response = client.Execute(request);
            //var _rows = JsonConvert.DeserializeObject<Coin.Data>(response.Content);
            Coin.DataRow[] orderList = new JavaScriptSerializer().Deserialize<Coin.DataRow[]>(response.Content);

            //List<Coin.DataRow> _rows = JsonConvert.DeserializeObject<List<Coin.DataRow>>(response.Content);
            foreach (var item in response.Content)
            {

            }

            ClearParameters();
        }

        public static void GetInfoCurrency()
        {
            RestClient _client = new RestClient("https://api.gateio.ws/api/v4/spot/currency_pairs/BLOCKASSET_USDT");
            var response = _client.Execute(request);
            
            Coin.Data coinInfo = JsonConvert.DeserializeObject<Coin.Data>(response.Content);
            coinInfo.Interval_string = "1m";
            GetRows(coinInfo);
            ClearParameters();
        }
        static void ClearParameters()
        {
            var list = request.Parameters.ToList();
            foreach (var param in list)
            {
                request.RemoveParameter(param);
            }
        }
    }
}
