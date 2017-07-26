using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNet.WebApi.Client;


namespace CongressWatchUWP.RemoteAPIs
{
    class Representative
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    class RESTClient
    {
        static HttpClient client = new HttpClient();

        public static async Task<List<Representative>> GetRepresentativesAsync(string path)
        {
            List<Representative> res = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //res = await response.Content.ReadAsAsync<ZipcodeReps>();
                string data = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<List<Representative>>(data);
            }
            return res;
        }

    }
}
