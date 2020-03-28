using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interfaces;
using Models.ViewModels.S3;

namespace DAL.ServiceApi
{
    public class MockS3Service : IS3Service
    {
        public Task<SimpleS3Response> Upload(string fileKey, byte[] data, IDictionary<string, string> metadata)
        {
            throw new NotImplementedException();
        }

        public Task<DownloadS3Response> Download(string keyName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> List()
        {
            throw new NotImplementedException();
        }
    }
}