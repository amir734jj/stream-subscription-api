using System;
using Models.Models.Sinks;

namespace Logic.Sinks;

internal static class SinkEndpointUtility
{
    public static Uri ResolveEndpoint(FtpSink sink)
    {
        if (string.IsNullOrWhiteSpace(sink.Host))
        {
            throw new Exception("Sink host is required.");
        }

        var rawHost = sink.Host.Trim();

        if (!rawHost.Contains("://", StringComparison.Ordinal))
        {
            rawHost = $"ftp://{rawHost}";
        }

        if (!Uri.TryCreate(rawHost, UriKind.Absolute, out var uri))
        {
            throw new Exception("Sink host is invalid. Use a hostname or FTP/SFTP URL.");
        }

        if (uri.Scheme != Uri.UriSchemeFtp && uri.Scheme != "sftp")
        {
            throw new Exception("Only FTP and SFTP sink protocols are supported.");
        }

        return uri;
    }

    public static string ResolveProtocol(FtpSink sink)
    {
        return ResolveEndpoint(sink).Scheme;
    }

    public static int ResolvePort(FtpSink sink)
    {
        if (sink.Port > 0)
        {
            return sink.Port;
        }

        return ResolveProtocol(sink) == "sftp" ? 22 : 21;
    }

    public static string NormalizeHost(FtpSink sink)
    {
        var endpoint = ResolveEndpoint(sink);
        return $"{endpoint.Scheme}://{endpoint.Host}";
    }
}
