using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Logic.Sinks;
using Models.Models.Sinks;

namespace Logic.Services;

public class SinkService : ISinkService
{
    private readonly IUserLogic _userLogic;
    private readonly IStreamLogic _streamLogic;

    public SinkService(IUserLogic userLogic, IStreamLogic streamLogic)
    {
        _userLogic = userLogic;
        _streamLogic = streamLogic;
    }
    
    public async Task UploadToSinks(int userId, int streamId, MemoryStream data, string filename)
    {
        var stream = await _streamLogic.Get(streamId);
        
        var sinks = stream.StreamFtpSinkRelationships
            .Select(x => ResolveUploadService(x.FtpSink))
            .Cast<IUploadService>()
            .ToList();

        var uploadTasks = sinks.Select(x => x.UploadStream(stream, filename, data));
                
        await Task.WhenAll(uploadTasks);
    }

    public async Task UploadToFavoriteSinks(int userId, MemoryStream data, string filename)
    {
        var user = await _userLogic.Get(userId);
        
        var sinks = user.FtpSinks
            .Where(x => x.Favorite)
            .Select(ResolveUploadService)
            .Cast<IUploadService>()
            .ToList();

        var uploadTasks = sinks.Select(x => x.UploadToFavorite(filename, data));
                
        await Task.WhenAll(uploadTasks);
    }

    private static IUploadService ResolveUploadService(FtpSink sink)
    {
        return SinkEndpointUtility.ResolveProtocol(sink) == "sftp"
            ? new SftpUploadService(sink)
            : new FtpUploadService(sink);
    }
}