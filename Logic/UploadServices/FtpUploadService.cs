using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using Logic.Interfaces;
using Models.ViewModels.UploadServices;

namespace Logic.UploadServices
{
    public class FtpUploadService : IUploadService
    {
        private readonly FtpUploadViewModel _ftpUploadViewModel;

        private readonly FtpClient _client;

        public FtpUploadService(FtpUploadViewModel ftpUploadViewModel)
        {
            _ftpUploadViewModel = ftpUploadViewModel;
            
            _client = new FtpClient(ftpUploadViewModel.Host, ftpUploadViewModel.Port, ftpUploadViewModel.Username, ftpUploadViewModel.Password);
        }

        public async Task UploadStream(MemoryStream stream, string filename)
        {
            await _client.UploadAsync(stream, Path.Join(_ftpUploadViewModel.Path, filename));
        }
    }
}