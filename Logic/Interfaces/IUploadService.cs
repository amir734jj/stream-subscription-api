using System.IO;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IUploadService
    {
        Task UploadStream(MemoryStream stream, string filename);
    }
}