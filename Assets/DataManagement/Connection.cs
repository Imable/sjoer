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
            this.connected = true;
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

            string content = "";

            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
                this.token = JObject.Parse(content)["access_token"].ToString();
                this.connected = true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private Uri getUriwithParams(string lonMin, string lonMax, string latMin, string latMax)
        {
            string uri = String.Format(Config.Instance.barentswatch.ais_url, lonMin, lonMax, latMin, latMax);
            return new Uri(uri);
        }

        // Lat Min, Lon Min, Lat Max, Lon Max
        public override async Task<string> get(params string[] param)
        {
            //return await Task.Run(() => "[{\"timeStamp\":\"2021-10-26T18:04:11Z\",\"sog\":0.0,\"rot\":0.0,\"navstat\":5,\"mmsi\":258465000,\"cog\":142.3,\"geometry\":{\"type\":\"Point\",\"coordinates\":[5.317615,60.398463]},\"shipType\":60,\"name\":\"TROLLFJORD\",\"imo\":9233258,\"callsign\":\"LLVT\",\"country\":\"Norge\",\"eta\":\"2021-05-03T17:00:00\",\"destination\":\"BERGEN\",\"isSurvey\":false,\"heading\":142,\"draught\":5.5,\"a\":19,\"b\":117,\"c\":11,\"d\":11}]");


            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = getUriwithParams(param[1], param[3], param[0], param[2])
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return await Task.Run(() => "[]");

            }

        }

    }

}
