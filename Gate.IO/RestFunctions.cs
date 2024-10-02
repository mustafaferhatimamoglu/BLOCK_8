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
        static RestClient client = new RestClient("https://api.gateio.ws/api/v4/spot/candlesticks?currency_pair=BLOCKASSET_USDT&limit=10&interval=10s");
       static RestRequest request = new RestRequest();
        static RestFunctions()
        {
            request.Method = Method.Get;
            request.AddHeader("Accept", "application/json");
            //'Accept': 'application/json', 'Content-Type': 'application/json'
            request.AddHeader("Content-Type", "application/json");
        }
        public static void f1()
        {
            

            //request.Parameters.Clear();
            //request.AddParameter("application/json", strJSONContent, ParameterType.RequestBody);

            //request.Parameters.RemoveParameter("Header User-Agent");
            var response = client.Execute(request);
            request.P
        }
        void ClearParameters()
        {

        }
    }
}
