using Models.Interfaces;
using Models.Models.Sinks;

namespace Models.Models.Relationships;

public class StreamFtpSinkRelationship : IEntity
{
    public int Id { get; set; }
        
    public Stream Stream { get; set; }
        
    public FtpSink FtpSink { get; set; }
        
    public int StreamId { get; set; }
        
    public int FtpSinkId { get; set; }
}