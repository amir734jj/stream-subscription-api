using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Logic.Interfaces;
using Models.Constants;

namespace Logic.Sinks
{
    /// <summary>
    /// TODO: NotImplemented
    /// </summary>
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
        public Task UploadStream(MemoryStream stream, string filename)
        {
            return _dropboxClient.Files.UploadAsync(
                $@"{StreamConstants.UploadFolder}/{filename}",
                WriteMode.Overwrite.Instance,
                body: stream,
                autorename: true);
        }
    }
}