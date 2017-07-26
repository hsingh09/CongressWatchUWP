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
        public string representativeId { get; set; }
        public int chamber { get; set; }
        public string party { get; set; }
        public override bool Equals(Object obj)
        {
            return representativeId.Equals(((Representative)obj).representativeId);
        }
        public override int GetHashCode()
        {
            return representativeId.GetHashCode();
        }
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
