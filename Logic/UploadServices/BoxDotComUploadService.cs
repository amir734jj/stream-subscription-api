using System;
using System.IO;
using System.Threading.Tasks;
using Box.V2;
using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Models;
using Logic.Interfaces;

namespace Logic.UploadServices
{
    // TODO
    public class BoxDotComUploadService : IUploadService
    {
        private readonly BoxClient _boxClient;

        /// <summary>
        /// Pass teh token
        /// </summary>
        /// <param name="token"></param>
        public BoxDotComUploadService(string token)
        {
            var config = new BoxConfig("", "", new Uri("http://localhost"));
            var session = new OAuthSession(token, "NOT_NEEDED", 3600, "bearer");
            _boxClient = new BoxClient(config, session);
        }

        public Task UploadStream(MemoryStream stream, string filename)
        {
            var req = new BoxFileRequest()
            {
                Name = filename,
                Parent = new BoxRequestEntity { Id = "0" },
                Description = "ripped_music"
            };

            return _boxClient.FilesManager.UploadAsync(req, stream);
        }
    }
}