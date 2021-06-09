using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dal.Extensions;
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
            Result = (await Collect()).Shuffle();
        }

        public async Task<string> Url(int id)
        {
            using var client = new HttpClient();

            if (Result == null || Result?.Find(x => x.Id == id) != null)
            {
                var mp3U = await client.GetStringAsync($"http://yp.shoutcast.com/sbin/tunein-station.m3u?id={id}");
            
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
                .AsParallel()
                .Where(async x =>
                {
                    try
                    {
                        var url =  await Url(x.Id);

                        if (string.IsNullOrWhiteSpace(url))
                        {
                            throw new Exception("Loading URL failed");
                        }

                        var req = WebRequest.Create(url);
                        req.Method = "HEAD";

                        var res = await req.GetResponseAsync();

                        return res != null;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }).ToListAsync();

            return tasks.Result.ToList();
        }

        public List<ShoutCastStream> Result { get; private set; }
    }
}
