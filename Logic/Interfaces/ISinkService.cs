using System;
using System.IO;
using System.Threading.Tasks;
using Stream = Models.Models.Stream;

namespace Logic.Interfaces
{
    public interface ISinkService
    {
        Task<Func<MemoryStream, string, Task>> Resolve(Stream stream);
    }
}