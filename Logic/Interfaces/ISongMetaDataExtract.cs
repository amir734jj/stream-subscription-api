using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Interfaces;

public interface ISongMetaDataExtract
{
    Task<ExtendedSongMetadata> Populate(ExtendedSongMetadata songMetadata);
}