using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dal.Interfaces;
using Models.ViewModels.Shoutcast;
using Newtonsoft.Json;

namespace Dal;

public class ShoutcastDirectoryApi : IShoutcastDirectoryApi
{
    public ShoutcastDirectoryApi()
    {
        Setup().Wait();
    }
        
    public async Task Setup()
    {
        Result = JsonConvert.DeserializeObject<Dictionary<string, List<ShoutCastStream>>>(await File.ReadAllTextAsync("shoutcast-directory.json"));
    }

    public Task<string> Url(int id)
    {
        return Task.FromResult(Result.Values.SelectMany(x => x).FirstOrDefault(x => x.Id == id)?.Url ?? throw new Exception("Failed to resolve streamURL"));
    }

    public Dictionary<string, List<ShoutCastStream>> Result { get; private set; }
}