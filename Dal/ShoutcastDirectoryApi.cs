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

            Result = Collect().Result;
        }

        private async Task<List<ShoutCastStream>> Collect()
        {
            using var client = new HttpClient();
            
            var request = new RestRequest("shoutcast-directory.json", DataFormat.Json)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
            };

            var response = await _restClient.GetAsync<Dictionary<string, List<ShoutCastStream>>>(request);

            var tasks = response.Values.SelectMany(x => x)
                .Select(async x =>
                {
                    var mp3U = await client.GetStringAsync($"http://yp.shoutcast.com/sbin/tunein-station.m3u?id={x.Id}");
                    x.Url = mp3U.Split(Environment.NewLine).FirstOrDefault(token => token.StartsWith("http"));

                    return x;
                })
                .ToList();

            await Task.WhenAll(tasks);

            return tasks.Select(x => x.Result).Where(x => x.Url != null).ToList();
        }

        public List<ShoutCastStream> Result { get; private set; }
    }
}