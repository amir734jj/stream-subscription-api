using System;
using System.IO;
using System.Threading.Tasks;

namespace Logic.Interfaces;

public interface ISinkService
{
    Task ResolveStreamSink(int streamId, MemoryStream data, string filename);

    Task ResolveFavoriteStream(int userId, MemoryStream data, string filename);
}