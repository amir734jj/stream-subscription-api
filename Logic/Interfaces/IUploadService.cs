using System.IO;

namespace Logic.Interfaces
{
    public interface IUploadService
    {
        void UploadStream(MemoryStream stream, string filename);
    }
}