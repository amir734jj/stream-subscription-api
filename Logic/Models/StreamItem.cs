using Models.Enums;
using Models.Models;
using StreamRipper.Interfaces;

namespace Logic.Models;

public class StreamItem
{
    public User User { get; set; }
        
    public IStreamRipper StreamRipper { get; set; }
        
    public StreamStatusEnum State { get; set; }
}