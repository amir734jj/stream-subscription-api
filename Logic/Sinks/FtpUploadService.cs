using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using Logic.Interfaces;
using Models.Models.Sinks;
using Stream = Models.Models.Stream;
namespace Logic.Sinks;

public class FtpUploadService : IUploadService
{
    private readonly FtpSink _ftpSink;

    private readonly AsyncFtpClient _client;

    public FtpUploadService(FtpSink ftpSink)
    {
        _ftpSink = ftpSink;
            
        _client = new AsyncFtpClient(ftpSink.Host, ftpSink.Username, ftpSink.Password, ftpSink.Port);
    }

    public async Task UploadStream(Stream stream, string filename, MemoryStream data)
    {
        await Upload(stream.Name, filename, data);
    }

    public async Task UploadToFavorite(string filename, MemoryStream data)
    {
        await Upload("favorite", filename, data);
    }

    private async Task Upload(string folder, string filename, System.IO.Stream data)
    {
        var directory = Path.Join(_ftpSink.Path, folder);
            
        await _client.Connect();

        await _client.CreateDirectory(directory);
            
        await _client.UploadStream(data, Path.Join(directory, filename));

        await _client.Disconnect();
    }
}