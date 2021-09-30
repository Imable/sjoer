using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using UnityEngine;

namespace Assets.DataManagement
{
    abstract class Connection
    {
        public bool connected = false;
        protected Connection()
        {
            this.connect();
        }

        protected abstract void connect();
        public abstract Task<string> get(params string[] param);
    }

    class GPSInfoConnection : Connection
    {
        protected override void connect()
        {

        }
        public override Task<string> get(params string[] param)
        {
            // FROM ED: return "$GPRMC,071228.00,A,5402.6015,N,00025.9797,E,0.2,332.1,180921,0.2,W,A,S*50";
            return Task.Run(() => "$GPRMC,071228.00,A,60.403029,N,5.322799,E,0.2,0,180921,0.2,W,A,S*50");
        }
    }

    class AISConnection : Connection
    {
        private HttpClient httpClient = new HttpClient();
        private string token = "";

        protected override void connect()
        {
            this.myConnect();
        }

        protected async void myConnect()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Config.Instance.barentswatch.token_url),
                Content = new StringContent(
                    String.Format(
                        Config.Instance.barentswatch.auth_format,
                        Config.Instance.barentswatch.client_id,
                        Config.Instance.barentswatch.client_secret
                    ),
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded"
                )
            };

            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            this.token = JObject.Parse(content)["access_token"].ToString();
            this.connected = true;
        }

        private Uri getUriwithParams(string lonMin, string lonMax, string latMin, string latMax)
        {
            string uri = String.Format(Config.Instance.barentswatch.ais_url, lonMin, lonMax, latMin, latMax);
            return new Uri(uri);
        }

        public override async Task<string> get(params string[] param)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = getUriwithParams(param[0], param[1], param[2], param[3])
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
 
            //return "{\"Identifier\":\"VesselA\",\"Latitude\":60.402957,\"Longitude\":5.322762}";
        }

    }

}
