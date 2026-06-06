using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Models.Models.Sinks;
using Renci.SshNet;
using Stream = Models.Models.Stream;

namespace Logic.Sinks;

public class SftpUploadService : IUploadService
{
    private readonly FtpSink _sink;

    public SftpUploadService(FtpSink sink)
    {
        _sink = sink;
    }

    public Task UploadStream(Stream stream, string filename, MemoryStream data)
    {
        return Upload(stream.Name, filename, data);
    }

    public Task UploadToFavorite(string filename, MemoryStream data)
    {
        return Upload("favorite", filename, data);
    }

    private Task Upload(string folder, string filename, MemoryStream data)
    {
        try
        {
            var endpoint = SinkEndpointUtility.ResolveEndpoint(_sink);
            var port = SinkEndpointUtility.ResolvePort(_sink);

            using var client = new SftpClient(endpoint.Host, port, _sink.Username, _sink.Password);

            client.Connect();

            var directory = CombineRemotePath(_sink.Path, folder);
            EnsureDirectory(client, directory);

            var remoteFilePath = CombineRemotePath(directory, filename);

            data.Position = 0;
            client.UploadFile(data, remoteFilePath, true);

            client.Disconnect();
        }
        catch (Exception)
        {
            // Swallow SFTP errors to prevent crashing the app
        }

        return Task.CompletedTask;
    }

    private static void EnsureDirectory(SftpClient client, string directory)
    {
        var parts = directory.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
        var current = "/";

        foreach (var part in parts)
        {
            current = current.EndsWith("/") ? current + part : current + "/" + part;

            if (!client.Exists(current))
            {
                client.CreateDirectory(current);
            }
        }
    }

    private static string CombineRemotePath(params string[] parts)
    {
        var cleaned = parts
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Replace("\\", "/").Trim('/'))
            .Where(x => x.Length > 0)
            .ToArray();

        return "/" + string.Join("/", cleaned);
    }
}
