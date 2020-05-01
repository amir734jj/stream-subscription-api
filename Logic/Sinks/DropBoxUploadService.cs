using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Logic.Interfaces;
using Models.Constants;
using Stream = Models.Models.Stream;

namespace Logic.Sinks
{
    /// <summary>
    /// TODO: NotImplemented
    /// </summary>
    public class DropBoxUploadService : IUploadService
    {
        private readonly DropboxClient _dropBoxClient;

        /// <summary>
        /// Pass teh token
        /// </summary>
        /// <param name="token"></param>
        public DropBoxUploadService(string token)
        {
            _dropBoxClient = new DropboxClient(token);
        }

        /// <summary>
        /// Upload the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        public Task UploadStream(Stream stream, string filename, MemoryStream data)
        {
            return _dropBoxClient.Files.UploadAsync(
                $@"{StreamConstants.UploadFolder}/{filename}",
                WriteMode.Overwrite.Instance,
                body: data,
                autorename: true);
        }

        public Task UploadToFavorite(string filename, MemoryStream data)
        {
            throw new System.NotImplementedException();
        }
    }
}