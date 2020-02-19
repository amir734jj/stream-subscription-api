using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using Logic.Interfaces;
using Models.Models.Sinks;

namespace Logic.Sinks
{
    public class FtpUploadService : IUploadService
    {
        private readonly FtpSink _ftpSink;

        private readonly FtpClient _client;

        public FtpUploadService(FtpSink ftpSink)
        {
            _ftpSink = ftpSink;
            
            _client = new FtpClient(ftpSink.Host, ftpSink.Port, ftpSink.Username, ftpSink.Password);
        }

        public async Task UploadStream(MemoryStream stream, string filename)
        {
            await _client.UploadAsync(stream, Path.Join(_ftpSink.Path, filename));
        }
    }
}