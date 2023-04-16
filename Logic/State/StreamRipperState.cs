using System.Collections.Concurrent;
using System.Collections.Generic;
using Logic.Models;
using StructureMap;

namespace Logic.State;

[Singleton]
public class StreamRipperState
{
    public IDictionary<int, StreamItem> StreamItems { get; set; } = new ConcurrentDictionary<int, StreamItem>();
}