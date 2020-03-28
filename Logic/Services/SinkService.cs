using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Crud;
using Logic.Interfaces;
using Logic.Sinks;
using Stream = Models.Models.Stream;

namespace Logic.Services
{
    public class SinkService : ISinkService
    {
        private readonly FtpSinkLogic _ftpSinkLogic;

        public SinkService(FtpSinkLogic ftpSinkLogic)
        {
            _ftpSinkLogic = ftpSinkLogic;
        }
        
        public async Task<Func<MemoryStream, string, Task>> Resolve(Stream stream)
        {
            var sinks = stream.FtpSinkRelationships.Select(x => new FtpUploadService(x.FtpSink)).Cast<IUploadService>().ToList();
            
            return async (data, filename) =>
            {
                var uploadTasks = sinks.Select(x => x.UploadStream(stream, filename, data));
                
                await Task.WhenAll(uploadTasks);
            };
        }
    }
}