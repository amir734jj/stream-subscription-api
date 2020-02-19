using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Crud;
using Logic.Interfaces;
using Logic.Sinks;
using Models.Enums;
using Stream = Models.Models.Stream;

namespace Logic
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
            var resolveSinksTasks = stream.SubscribedSinks.Select(async x =>
            {
                var (id, type) = x;

                return (IUploadService) (type switch
                {
                    SinkTypeEnum.Ftp => new FtpUploadService(await _ftpSinkLogic.Get(id)),
                    _ => throw new ArgumentOutOfRangeException()
                });
            }).ToArray();

            await Task.WhenAll(resolveSinksTasks);

            var sinks = resolveSinksTasks.Select(x => x.Result).ToList();

            return async (data, filename) =>
            {
                var uploadTasks = sinks.Select(x => x.UploadStream(data, filename));
                
                await Task.WhenAll(resolveSinksTasks);

                await Task.WhenAll(uploadTasks);
            };
        }
    }
}