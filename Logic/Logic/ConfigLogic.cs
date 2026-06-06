using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using EfCoreRepository.Interfaces;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Models;
using Models.ViewModels.Config;
using static Models.Constants.GlobalConfigs;

namespace Logic.Logic;

public class ConfigLogic : IConfigLogic
{
    private readonly IEfRepository _repository;

    private readonly ILogger<ConfigLogic> _logger;

    public ConfigLogic(IEfRepository repository, ILogger<ConfigLogic> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private IBasicCrud<GlobalConfig> Dal => _repository.For<GlobalConfig>();

    private async Task SetGlobalConfig(GlobalConfigViewModel globalConfigViewModel)
    {
        UpdateGlobalConfigs(globalConfigViewModel);

        var rows = (await Dal.GetAll()).ToList();

        // Serialize StartedStreams as comma-separated string
        var startedStreamsValue = string.Join(",", globalConfigViewModel.StartedStreams);

        var existing = rows.FirstOrDefault(r => r.Key == "StartedStreams");
        if (existing != null)
        {
            await Dal.Update(existing.Id, e => e.Value = startedStreamsValue);
        }
        else
        {
            await Dal.Save(new GlobalConfig
            {
                Key = "StartedStreams",
                Value = startedStreamsValue
            });
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
        try
        {
            var rows = (await Dal.GetAll()).ToList();

            var startedStreamsRow = rows.FirstOrDefault(r => r.Key == "StartedStreams");
            var startedStreams = ImmutableHashSet<int>.Empty;

            if (startedStreamsRow != null && !string.IsNullOrWhiteSpace(startedStreamsRow.Value))
            {
                startedStreams = startedStreamsRow.Value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToImmutableHashSet();
            }

            var config = new GlobalConfigViewModel { StartedStreams = startedStreams };

            _logger.LogInformation("Successfully loaded config from database");

            UpdateGlobalConfigs(config);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to load config from database");
        }
    }
}