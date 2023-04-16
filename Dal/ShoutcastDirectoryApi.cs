using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dal.Interfaces;
using Models.ViewModels.Shoutcast;
using Newtonsoft.Json;
using StructureMap;

namespace Dal;

[Singleton]
public class ShoutcastDirectoryApi : IShoutcastDirectoryApi
{
    public ShoutcastDirectoryApi()
    {
        Setup().Wait();
    }
        
    public async Task Setup()
    {
        Result = JsonConvert.DeserializeObject<List<ShoutCastStream>>(await File.ReadAllTextAsync("shoutcast-directory.json"));
    }

    public async Task<string> Url(int id)
    {
        using var client = new HttpClient();

        if (Result.Find(x => x.Id == id) != null)
        {
            var mp3U = await client.GetStringAsync($"https://yp.shoutcast.com/sbin/tunein-station.m3u?id={id}");
            
            var url = mp3U.Split(Environment.NewLine).FirstOrDefault(token => token.StartsWith("http"));

            return url;
        }

        throw new Exception("Failed to resolve streamURL");
    }

    public List<ShoutCastStream> Result { get; private set; }
}