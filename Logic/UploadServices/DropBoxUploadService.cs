using System;
using System.IO;
using Dropbox.Api;
using Dropbox.Api.Files;
using Logic.Interfaces;
using Models.Constants;

namespace Logic.UploadServices
{
    public class DropBoxUploadService : IUploadService
    {
        private readonly DropboxClient _dropboxClient;

        /// <summary>
        /// Pass teh token
        /// </summary>
        /// <param name="token"></param>
        public DropBoxUploadService(string token)
        {
            _dropboxClient = new DropboxClient(token);
        }

        /// <summary>
        /// Upload the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        public async void UploadStream(MemoryStream stream, string filename)
        {
            await _dropboxClient.Files.UploadAsync(
                StreamConstants.UploadFolder + "/" + filename,
                WriteMode.Overwrite.Instance,
                body: stream);
        }
    }
}