using System.Collections.Generic;
using Models.Enums;
using Models.Models;

namespace Models.ViewModels.Streams;

public class StreamsStatusViewModel
{
    public Dictionary<Stream, StreamStatusEnum> Status { get; set; }
}