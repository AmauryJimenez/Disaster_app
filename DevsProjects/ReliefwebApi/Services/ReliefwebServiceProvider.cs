using Contracts.Services;
using Models.Disasters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReliefwebApi.Services
{
    public class ReliefwebServiceProvider : IDisasterInfoProvider
    {
        private const string _requestUrl = "https://api.reliefweb.int/v1/disasters?appname=DevPrj";

        private readonly HttpClient _client;

        public ReliefwebServiceProvider()
        {
            _client = new HttpClient();
        }

        public IList<Disaster> RequestDisasters(DisasterType[] requestDisasterTypes)
        {
            List<Disaster> result = new List<Disaster>();

            foreach (var requestDisasterType in requestDisasterTypes)
                RequestDisasterType(result, _client, requestDisasterType);

            return result;
        }

        private static void RequestDisasterType(List<Disaster> result, HttpClient client, DisasterType requestDisasterType)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent content = new StringContent($"{{ \"filter\": {{ \"field\": \"type\", \"value\": \"{requestDisasterType.ToString()}\" }}, \"fields\": {{ \"include\": [\"date\"] }} }}");
            Task<HttpResponseMessage> postTask = client.PostAsync(_requestUrl, content);
            postTask.Wait();

            HttpResponseMessage response = postTask.Result;

            // check to see if we have a succesfull respond
            if (response.IsSuccessStatusCode)
                LoadResponseData(result, requestDisasterType, response);
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            else
                throw new InvalidOperationException(string.Format("{0}:{1}", response.StatusCode, response.ReasonPhrase));
        }

        private static void LoadResponseData(List<Disaster> result, DisasterType requestDisasterType, HttpResponseMessage response)
        {
            Task<string> contentTask = response.Content.ReadAsStringAsync();
            contentTask.Wait();

            JToken jsonResponse = JToken.Parse(contentTask.Result);

            foreach (JObject jsonDisaster in jsonResponse["data"].Values<JObject>())
            {
                int id = jsonDisaster["id"].Value<int>();
                string name = jsonDisaster["fields"]["name"].Value<string>();

                if (!result.Any(d => d.Id.Equals(id)))
                {
                    Disaster disaster = CreateDisaster(requestDisasterType, jsonDisaster, id, name);
                    result.Add(disaster);
                }
            }
        }

        private static Disaster CreateDisaster(DisasterType requestDisasterType, JObject jsonDisaster, int id, string name)
        {
            Disaster disaster = null;

            switch (requestDisasterType)
            {
                case DisasterType.Flood:
                    disaster = new FloodDisaster();
                    break;
                case DisasterType.Epidemic:
                    disaster = new EpidemicDisaster();
                    break;
                case DisasterType.Earthquake:
                    disaster = new EarthquakeDisaster();
                    break;
            }

            disaster.Id = id;
            disaster.Name = name;
            disaster.ReportedOn = jsonDisaster["fields"]["date"]["created"].Value<DateTime>();
            return disaster;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
