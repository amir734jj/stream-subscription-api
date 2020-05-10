using System;
using System.Collections.Generic;
using StreamRipper.Models.Song;

namespace Logic.Models
{
    public class ExtendedSongMetadata : SongMetadata
    {
        public int PlayCount { get; set; }

        public string Url { get; set; }
        
        public string Album { get; set; } = string.Empty;
        
        public List<string> Tags { get; set; } = new List<string>();

        public double Duration { get; set; }

        public static ExtendedSongMetadata Extend(SongMetadata songMetadata, Action<ExtendedSongMetadata> extras)
        {
            var result = new ExtendedSongMetadata
            {
                Artist = songMetadata.Artist,
                Raw = songMetadata.Raw,
                Title = songMetadata.Title
            };

            extras(result);

            return result;
        }
    }
}