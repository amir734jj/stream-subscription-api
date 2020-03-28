using System;
using System.Collections.Immutable;
using System.Net;
using System.Threading.Tasks;
using DAL.Extensions;
using DAL.Interfaces;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Models.ViewModels.Config;
using static Models.Constants.GlobalConfigs;
using static Models.Constants.ApplicationConstants;
using static Logic.Utilities.LambdaUtility;

namespace Logic
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

            await IgnoreException(async () =>
                    await _s3Service.Upload(ConfigFile, globalConfigViewModel.ToByteArray(),
                        ImmutableDictionary.Create<string, string>().Add("Description", "Application config file")),
                _ => Task.CompletedTask,
                e => Then(() => _logger.LogError("Failed to sync config file with S3", e), Task.CompletedTask));
        }

        public GlobalConfigViewModel ResolveGlobalConfig()
        {
            return ToViewModel();
        }

        public async Task UpdateGlobalConfig(Func<GlobalConfigViewModel, GlobalConfigViewModel> update)
        {
            await SetGlobalConfig(update(ResolveGlobalConfig()));
        }

        public async Task Refresh()
        {
            await IgnoreException(async () => await _s3Service.Download(ConfigFile), async responseTsk =>
            {
                var response = await responseTsk;

                if (response.Status == HttpStatusCode.OK)
                {
                    _logger.LogInformation("Successfully fetched the config from S3");

                    var globalConfigViewModel = response.Data.Deserialize<GlobalConfigViewModel>();

                    UpdateGlobalConfigs(globalConfigViewModel);
                }
            }, e => Then(() => _logger.LogError("Failed to fetch the config from S3", e), Task.CompletedTask));
        }
    }
}