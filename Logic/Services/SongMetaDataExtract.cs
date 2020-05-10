using System;
using System.Linq;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using Logic.Interfaces;
using Logic.Models;

namespace Logic.Services
{
    public class SongMetaDataExtract : ISongMetaDataExtract
    {
        private readonly LastfmClient _lastFmClient;

        public SongMetaDataExtract(LastfmClient lastFmClient)
        {
            _lastFmClient = lastFmClient;
        }

        public async Task<ExtendedSongMetadata> Populate(ExtendedSongMetadata songMetadata)
        {
            var track = $"{songMetadata.Artist}-{songMetadata.Title}";

            var trackSearchResult = await _lastFmClient.Track.SearchAsync(track);

            // Nothing to add
            if (!trackSearchResult.Success || !trackSearchResult.Content.Any())
            {
                return songMetadata;
            }

            var trackResultItem = trackSearchResult.Content.First();
            var trackInfo =
                await _lastFmClient.Track.GetInfoAsync(trackResultItem.Name, trackResultItem.ArtistName);

            if (trackInfo.Success && trackInfo.Content != null)
            {
                songMetadata.Url = trackInfo.Content.Url?.AbsoluteUri;
                songMetadata.PlayCount = trackInfo.Content.PlayCount ?? 0;
            }
            
            var albumInfo = await _lastFmClient.Album.SearchAsync(track);
            if (albumInfo.Success && albumInfo.Content.Any())
            {
                songMetadata.Album = albumInfo.Content.First().Name;
            }

            return songMetadata;
        }
    }
}