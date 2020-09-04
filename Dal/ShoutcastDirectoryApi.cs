using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dal.Interfaces;
using Models.ViewModels.Shoutcast;
using RestSharp;
using StructureMap;

namespace Dal
{
    [Singleton]
    public class ShoutcastDirectoryApi : IShoutcastDirectoryApi
    {
        private readonly IRestClient _restClient;

        public ShoutcastDirectoryApi(IRestClient restClient)
        {
            _restClient = restClient;
        }
        
        public async Task Setup()
        {
            Result = await Collect();
        }

        public async Task<string> Url(int id)
        {
            using var client = new HttpClient();

            var stream = Result.Find(x => x.Id == id);

            if (stream != null)
            {
                var mp3U = await client.GetStringAsync($"http://yp.shoutcast.com/sbin/tunein-station.m3u?id={stream.Id}");
            
                var url = mp3U.Split(Environment.NewLine).FirstOrDefault(token => token.StartsWith("http"));

                return url;
            }

            throw new Exception("Failed to resolve streamURL");
        }

        private async Task<List<ShoutCastStream>> Collect()
        {

            var request = new RestRequest("shoutcast-directory.json", DataFormat.Json)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
            };

            var response = await _restClient.GetAsync<Dictionary<string, List<ShoutCastStream>>>(request);

            var tasks = response.Values.SelectMany(x => x)
                .ToList();

            return tasks;
        }

        public List<ShoutCastStream> Result { get; private set; }
    }
}