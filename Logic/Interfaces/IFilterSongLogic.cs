using System.IO;
using StreamRipper.Models.Song;

namespace Logic.Interfaces;

public interface IFilterSongLogic
{
    bool ShouldInclude(MemoryStream stream, SongMetadata track, string pattern, out double duration);
}