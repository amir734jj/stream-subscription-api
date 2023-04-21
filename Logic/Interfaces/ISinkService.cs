using System.IO;
using System.Threading.Tasks;

namespace Logic.Interfaces;

public interface ISinkService
{
    Task UploadToSinks(int userId, int streamId, MemoryStream data, string filename);

    Task UploadToFavoriteSinks(int userId, MemoryStream data, string filename);
}