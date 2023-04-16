using System;
using System.IO;
using System.Threading.Tasks;
using Models.Models;
using Stream = Models.Models.Stream;

namespace Logic.Interfaces;

public interface ISinkService
{
    Func<MemoryStream, string, Task> ResolveStreamSink(Stream stream);

    Func<string, MemoryStream, Task> ResolveFavoriteStream(User user);
}