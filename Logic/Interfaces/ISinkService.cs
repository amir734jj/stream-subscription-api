using System;
using System.IO;
using System.Threading.Tasks;

namespace Logic.Interfaces;

public interface ISinkService
{
    Task<Func<MemoryStream, string, Task>> ResolveStreamSink(int streamId);

    Task<Func<string, MemoryStream, Task>> ResolveFavoriteStream(int userId);
}