using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Logic.Interfaces;
using Models.Enums;
using Models.Models;

namespace Logic.DataStructures
{
    public class StreamState : IStreamState
    {
        private ImmutableList<StreamItem> StreamItems { get; set; }

        public void UpdateState(Func<ImmutableList<StreamItem>, Func<Func<StreamItem, bool>, StreamItem>, ImmutableList<StreamItem>> modify)
        {
            StreamItems = modify(StreamItems, Resolve().First);
        }

        public IEnumerable<StreamItem> Resolve()
        {
            return StreamItems.AsEnumerable();
        }

        public StreamState()
        {
            StreamItems  = ImmutableList<StreamItem>.Empty;
        }
    }

    public class StreamItem : ICloneable
    {
        public Guid Key { get; set; }
        
        public User User { get; set; }
        
        public Stream Stream { get; set; }
        
        public StreamStatusEnum State { get; set; }
        
        public IUploadService UploadService { get; set; }
        public object Clone()
        {
            return new StreamItem
            {
                Key = Key,
                User = User,
                Stream = Stream,
                State = State,
                UploadService = UploadService
            };
        }
    }
}