using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLOCK_8
{
    public partial class Form3: Form
    {
        public Form3()
        {
            InitializeComponent();


            Gate.IO.RestFunctions.f1();


            a1();

        }
        async void a1()
        {
            //using RestSharp;
            //using RestSharp.Authenticators;
            CancellationToken cancellationToken = new CancellationToken();

            var options = new RestClientOptions("https://api.gateio.ws/api/v4");
            //{
            //    Authenticator = new HttpBasicAuthenticator("username", "password")
            //};
            //{
            //    Parameter =
            //}
            var client = new RestClient(options);
            var request = new RestRequest("/spot/candlesticks%currency_pair=BLOCKASSET_USDT&limit=10&interval=10s&from=1457481600");
            // The cancellation token comes from the caller. You can still make a call without it.
            var response = await client.GetAsync(request, cancellationToken);
        }
    }
}
