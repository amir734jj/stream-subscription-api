using System;
using System.Threading.Tasks;
using Dal.Interfaces;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Models.ViewModels.Config;
using static Models.Constants.GlobalConfigs;

namespace Logic.Logic;

public class ConfigLogic : IConfigLogic
{
    private readonly ISimpleConfigServer _configServer;

    private readonly SimpleConfigServerApiKey _configServerApiKey;
        
    private readonly ILogger<ConfigLogic> _logger;

    public ConfigLogic(ISimpleConfigServer configServer, SimpleConfigServerApiKey configServerApiKey, ILogger<ConfigLogic> logger)
    {
        _configServer = configServer;
        _configServerApiKey = configServerApiKey;
        _logger = logger;
    }

    private async Task SetGlobalConfig(GlobalConfigViewModel globalConfigViewModel)
    {
        UpdateGlobalConfigs(globalConfigViewModel);

        await _configServer.Update(_configServerApiKey.ApiKey, globalConfigViewModel);
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
        try
        {
            var response = await _configServer.Load(_configServerApiKey.ApiKey);

            _logger.LogInformation("Successfully fetched the config from config server");

            UpdateGlobalConfigs(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to fetch the config from config server");
        }
    }
}