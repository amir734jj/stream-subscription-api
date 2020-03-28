using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels.S3;

namespace Dal.Interfaces
{
    public interface IS3Service
    {
        Task<SimpleS3Response> Upload(string fileKey,
            byte[] data,
            IDictionary<string, string> metadata);

        Task<DownloadS3Response> Download(string keyName);

        Task<List<string>> List();
    }
}