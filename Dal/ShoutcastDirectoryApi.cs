using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dal.Interfaces;
using Microsoft.Extensions.Logging;
using Models.ViewModels.Shoutcast;
using Newtonsoft.Json;

namespace Dal;

public class ShoutcastDirectoryApi : IShoutcastDirectoryApi
{
    private static readonly HttpClient HttpClient = new();

    private const string LatestReleaseUrl =
        "https://api.github.com/repos/amir734jj/shoutcast-directory-crawler/releases/latest";

    private const string LocalFilePath = "shoutcast-directory.json";

    private readonly ILogger<ShoutcastDirectoryApi> _logger;

    public ShoutcastDirectoryApi(ILogger<ShoutcastDirectoryApi> logger)
    {
        _logger = logger;

        if (File.Exists(LocalFilePath))
        {
            var json = File.ReadAllText(LocalFilePath);
            Result = JsonConvert.DeserializeObject<Dictionary<string, List<ShoutCastStream>>>(json);
            _logger.LogInformation("Loaded shoutcast directory from local file: {FilePath}", LocalFilePath);
        }
        else
        {
            try
            {
                Setup().Wait();
                _logger.LogInformation("Loaded shoutcast directory from GitHub release");
            }
            catch (Exception)
            {
                Result = new Dictionary<string, List<ShoutCastStream>>();
                _logger.LogWarning("Failed to load shoutcast directory from GitHub release, using empty directory");
            }
        }
    }
        
    public async Task Setup()
    {
        HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("stream-subscription-api");

        var releaseJson = await HttpClient.GetStringAsync(LatestReleaseUrl);
        var release = JsonConvert.DeserializeObject<GitHubRelease>(releaseJson);
        var asset = release?.Assets?.FirstOrDefault(a => a.Name == "shoutcast-directory.json");

        if (asset == null)
        {
            throw new Exception("Could not find shoutcast-directory.json in latest release");
        }

        var json = await HttpClient.GetStringAsync(asset.BrowserDownloadUrl);
        Result = JsonConvert.DeserializeObject<Dictionary<string, List<ShoutCastStream>>>(json);
    }

    public Task<string> Url(int id)
    {
        return Task.FromResult(Result.Values.SelectMany(x => x).FirstOrDefault(x => x.Id == id)?.Url ?? throw new Exception("Failed to resolve streamURL"));
    }

    public Dictionary<string, List<ShoutCastStream>> Result { get; private set; }

    private class GitHubRelease
    {
        [JsonProperty("assets")]
        public List<GitHubAsset> Assets { get; set; }
    }

    private class GitHubAsset
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}