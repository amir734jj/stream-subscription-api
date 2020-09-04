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
            var request = new RestRequest("shoutcast-directory.json", DataFormat.Json)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
            };

            var response = await _restClient.GetAsync<Dictionary<string, List<ShoutCastStream>>>(request);

            return response.Values.SelectMany(x => x).ToList();
        }

        public List<ShoutCastStream> Result { get; private set; }
    }
}