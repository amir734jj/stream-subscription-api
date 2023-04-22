using Newtonsoft.Json;

namespace Models.ViewModels.Shoutcast;

public class ShoutCastStream
{
    [JsonProperty("ID")]
    public int Id { get; set; }
        
    public string Name { get; set; }
        
    public string Format { get; set; }
        
    public int Bitrate { get; set; }
        
    public string Genre { get; set; }
        
    public string CurrentTrack { get; set; }
        
    public int Listeners { get; set; }
        
    public bool IsRadionomy { get; set; }
        
    public string IceUrl { get; set; }
        
    public object StreamUrl { get; set; }
        
    [JsonProperty("AACEnabled")]
    public int AacEnabled { get; set; }
        
    public bool IsPlaying { get; set; }
        
    [JsonProperty("IsAACEnabled")]
    public bool IsAacEnabled { get; set; }
    
    public string Url { get; set; }
}