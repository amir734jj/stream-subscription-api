using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Logic.Sinks;

namespace Logic.Services;

public class SinkService : ISinkService
{
    private readonly IStreamLogic _streamLogic;
    private readonly IUserLogic _userLogic;

    public SinkService(IStreamLogic streamLogic, IUserLogic userLogic)
    {
        _streamLogic = streamLogic;
        _userLogic = userLogic;
    }
    
    public async Task ResolveStreamSink(int streamId, MemoryStream data, string filename)
    {
        var stream = await _streamLogic.Get(streamId);
        
        var sinks = stream.StreamFtpSinkRelationships
            .Select(x => new FtpUploadService(x.FtpSink))
            .Cast<IUploadService>()
            .ToList();
            
        var uploadTasks = sinks.Select(x => x.UploadStream(stream, filename, data));
                
        await Task.WhenAll(uploadTasks);
    }

    public async Task ResolveFavoriteStream(int userId, MemoryStream data, string filename)
    {
        var user = await _userLogic.Get(userId);
        
        var sinks = user.FtpSinks
            .Where(x => x.Favorite)
            .Select(x => new FtpUploadService(x))
            .Cast<IUploadService>()
            .ToList();

        var uploadTasks = sinks.Select(x => x.UploadToFavorite(filename, data));
                
        await Task.WhenAll(uploadTasks);
    }
}