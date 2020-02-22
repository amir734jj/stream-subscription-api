using System;
using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using Logic.Interfaces;
using Models.Models.Sinks;
using Stream = Models.Models.Stream;
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

        public async Task UploadStream(Stream stream, string filename, MemoryStream data)
        {
            var directory = Path.Join(_ftpSink.Path, new Uri(stream.Url).Host);

            await _client.CreateDirectoryAsync(directory);
            
            await _client.UploadAsync(data, Path.Join(directory, filename));
        }
    }
}