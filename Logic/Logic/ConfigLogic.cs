using System;
using System.Collections.Immutable;
using System.Net;
using System.Threading.Tasks;
using Dal.Extensions;
using Dal.Interfaces;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Models.ViewModels.Config;
using static Models.Constants.GlobalConfigs;
using static Models.Constants.ApplicationConstants;

namespace Logic.Logic
{
    public class ConfigLogic : IConfigLogic
    {
        private readonly IS3Service _s3Service;

        private readonly ILogger<ConfigLogic> _logger;

        public ConfigLogic(IS3Service s3Service, ILogger<ConfigLogic> logger)
        {
            _s3Service = s3Service;
            _logger = logger;
        }

        private async Task SetGlobalConfig(GlobalConfigViewModel globalConfigViewModel)
        {
            UpdateGlobalConfigs(globalConfigViewModel);

            var response = await _s3Service.Upload(ConfigFile, globalConfigViewModel.ObjectToByteArray(),
                ImmutableDictionary.Create<string, string>().Add("Description", "Application config file"));

            if (response.Status == HttpStatusCode.BadRequest)
            {
                _logger.LogError("Failed to sync config file with S3");
            }
        }

        public GlobalConfigViewModel ResolveGlobalConfig()
        {
            return ToViewModel();
        }

        public async Task UpdateGlobalConfig(Func<GlobalConfigViewModel, GlobalConfigViewModel> update)
        {
            var re = update(ResolveGlobalConfig());
            
            await SetGlobalConfig(re);
        }

        public async Task Refresh()
        {
            var response = await _s3Service.Download(ConfigFile);
            
            if (response.Status == HttpStatusCode.OK)
            {
                _logger.LogInformation("Successfully fetched the config from S3");

                UpdateGlobalConfigs(response.Data.Deserialize<GlobalConfigViewModel>());
            }
            else
            {
                _logger.LogError("Failed to fetch the config from S3");
            }
        }
    }
}