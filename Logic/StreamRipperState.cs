using System.Collections.Concurrent;
using System.Collections.Generic;
using Logic.Models;

namespace Logic
{
    public class StreamRipperState
    {
        public IDictionary<int, StreamItem> StreamItems { get; set; } = new ConcurrentDictionary<int, StreamItem>();
    }
}