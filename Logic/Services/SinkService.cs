using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Crud;
using Logic.Interfaces;
using Logic.Sinks;

namespace Logic.Services;

public class SinkService : ISinkService
{
    private readonly FtpSinkLogic _ftpSinkLogic;
    private readonly IStreamLogic _streamLogic;
    private readonly IUserLogic _userLogic;

    public SinkService(FtpSinkLogic ftpSinkLogic, IStreamLogic streamLogic, IUserLogic userLogic)
    {
        _ftpSinkLogic = ftpSinkLogic;
        _streamLogic = streamLogic;
        _userLogic = userLogic;
    }
        
    // TODO: use pure functions
    public async Task<Func<MemoryStream, string, Task>> ResolveStreamSink(int streamId)
    {
        var stream = await _streamLogic.Get(streamId);
        
        var sinks = stream.StreamFtpSinkRelationships
            .Select(x => new FtpUploadService(x.FtpSink))
            .Cast<IUploadService>()
            .ToList();
            
        return async (data, filename) =>
        {
            var uploadTasks = sinks.Select(x => x.UploadStream(stream, filename, data));
                
            await Task.WhenAll(uploadTasks);
        };
    }

    public async Task<Func<string, MemoryStream, Task>> ResolveFavoriteStream(int userId)
    {
        var user = await _userLogic.Get(userId);
        
        var sinks = user.FtpSinks
            .Where(x => x.Favorite)
            .Select(x => new FtpUploadService(x))
            .Cast<IUploadService>()
            .ToList();

        return async (filename, stream) =>
        {
            var uploadTasks = sinks.Select(x => x.UploadToFavorite(filename, stream));
                
            await Task.WhenAll(uploadTasks);
        };
    }
}