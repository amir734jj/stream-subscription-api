using System.IO;

namespace Logic.Interfaces;

public interface IFilterSongLogic
{
    bool ShouldInclude(MemoryStream stream, string track, string pattern, out double duration);
}