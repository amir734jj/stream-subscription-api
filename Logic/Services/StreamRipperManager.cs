﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Logic.Extensions;
using Logic.Interfaces;
using Logic.Models;
using Logic.State;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Models.Enums;
using Models.Models;
using Models.ViewModels.Config;
using StreamRipper.Interfaces;
using Stream = Models.Models.Stream;

namespace Logic.Services;

public class StreamRipperManager : IStreamRipperManager
{
    private readonly IStreamLogic _streamLogic;

    private readonly ISinkService _sinkService;

    private readonly StreamRipperState _state;

    private readonly ILogger<IStreamRipper> _logger;

    private readonly IHubContext<MessageHub> _hub;

    private readonly IConfigLogic _configLogic;

    private readonly IFilterSongLogic _filterSongLogic;
        
    private readonly ISongMetaDataExtract _songMetaDataExtract;
        
    private readonly IStreamRipperProxy _streamRipperProxy;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="state"></param>
    /// <param name="streamLogic"></param>
    /// <param name="sinkService"></param>
    /// <param name="hub"></param>
    /// <param name="configLogic"></param>
    /// <param name="filterSongLogic"></param>
    /// <param name="streamRipperProxy"></param>
    /// <param name="songMetaDataExtract"></param>
    /// <param name="logger"></param>
    public StreamRipperManager(StreamRipperState state, IStreamLogic streamLogic, ISinkService sinkService,
        IHubContext<MessageHub> hub, IConfigLogic configLogic, IFilterSongLogic filterSongLogic,
        IStreamRipperProxy streamRipperProxy,
        ISongMetaDataExtract songMetaDataExtract, ILogger<IStreamRipper> logger)
    {
        _state = state;
        _streamLogic = streamLogic;
        _sinkService = sinkService;
        _hub = hub;
        _configLogic = configLogic;
        _filterSongLogic = filterSongLogic;
        _songMetaDataExtract = songMetaDataExtract;
        _streamRipperProxy = streamRipperProxy;
        _logger = logger;
    }

    public IStreamRipperManagerImpl For(User user)
    {
        return new StreamRipperManagerImpl(_state, _streamLogic, _sinkService, user, _hub, _configLogic,
            _filterSongLogic, _songMetaDataExtract, _streamRipperProxy, _logger);
    }

    public async Task Refresh()
    {
        var startedStreamIds = _configLogic.ResolveGlobalConfig().StartedStreams;
        var streams = await _streamLogic.GetAll();

        await StartMany(streams.Join(startedStreamIds,
                stream => stream.Id,
                streamId => streamId,
                (stream, _) => stream)
            // in the last 3 days user should have logged in for stream to auto start
            .Where(x => DateTimeOffset.Now - x.User?.LastLoginTime <= TimeSpan.FromDays(3)));
    }

    public async Task StartMany(IEnumerable<Stream> streams)
    {
        var mutex = new SemaphoreSlim(1);

        var tasks = streams.Select(async stream =>
        {
            await mutex.WaitAsync();
            try
            {
                await For(stream.User).Start(stream.Id);

                _logger.LogInformation($"Started stream {stream.Id} yielded");
            }
            finally
            {
                mutex.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
}

public class StreamRipperManagerImpl : IStreamRipperManagerImpl
{
    private readonly IStreamLogic _streamLogic;

    private readonly ISinkService _sinkService;

    private readonly StreamRipperState _state;

    private readonly User _user;

    private readonly ILogger<IStreamRipper> _logger;

    private readonly IHubContext<MessageHub> _hub;

    private readonly IConfigLogic _configLogic;

    private readonly IFilterSongLogic _filterSongLogic;
        
    private readonly ISongMetaDataExtract _songMetaDataExtract;
        
    private readonly IStreamRipperProxy _streamRipperProxy;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="state"></param>
    /// <param name="streamLogic"></param>
    /// <param name="sinkService"></param>
    /// <param name="user"></param>
    /// <param name="hub"></param>
    /// <param name="configLogic"></param>
    /// <param name="filterSongLogic"></param>
    /// <param name="songMetaDataExtract"></param>
    /// <param name="streamRipperProxy"></param>
    /// <param name="logger"></param>
    public StreamRipperManagerImpl(StreamRipperState state, IStreamLogic streamLogic, ISinkService sinkService,
        User user, IHubContext<MessageHub> hub, IConfigLogic configLogic, IFilterSongLogic filterSongLogic,
        ISongMetaDataExtract songMetaDataExtract, IStreamRipperProxy streamRipperProxy,
        ILogger<IStreamRipper> logger)
    {
        _state = state;
        _streamLogic = streamLogic;
        _sinkService = sinkService;
        _user = user;
        _hub = hub;
        _configLogic = configLogic;
        _filterSongLogic = filterSongLogic;
        _songMetaDataExtract = songMetaDataExtract;
        _streamRipperProxy = streamRipperProxy;
        _logger = logger;
    }

    /// <summary>
    /// Pass username to GetAll
    /// </summary>
    /// <returns></returns>
    public async Task<Dictionary<int, StreamStatusEnum>> Status()
    {
        var streams = await _streamLogic.For(_user).GetAll();

        return streams
            .ToDictionary(x => x.Id,
                x => _state.StreamItems.FirstOrDefault(y => y.Key == x.Id).Value?.State ??
                     StreamStatusEnum.Stopped);
    }

    /// <summary>
    /// Start the stream
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> Start(int id)
    {
        Stream stream;

        // Stream does not exist
        if ((stream = await _streamLogic.For(_user).Get(id)) == null)
        {
            return false;
        }

        // Stream already started
        if (_state.StreamItems.ContainsKey(id) && _state.StreamItems[id].State == StreamStatusEnum.Started)
        {
            return false;
        }

        var streamRipperInstance = _streamRipperProxy.Proxy(new Uri(stream.Url));

        streamRipperInstance.SongChangedEventHandlers += async (_, arg) =>
        {
            var songMetaData = arg.SongInfo.SongMetadata;

            var track = $"{songMetaData.Artist}-{songMetaData.Title}";
            var filename = $"{track}.mp3";

            if (_filterSongLogic.ShouldInclude(arg.SongInfo.Stream, track, stream.Filter, out var duration))
            {
                var aggregatedSink = _sinkService.ResolveStreamSink(stream);

                // Upload the stream
                await aggregatedSink(arg.SongInfo.Stream.Reset(), filename);

                var extendedSongMetadata = await _songMetaDataExtract.Populate(ExtendedSongMetadata.Extend(
                    arg.SongInfo.SongMetadata, x =>
                    {
                        x.Duration = duration;
                    }));

                // Invoke socket
                await _hub.Clients.User(_user.Id.ToString()).SendAsync("download",
                    filename,
                    extendedSongMetadata,
                    arg.SongInfo.Stream.Reset().ConvertToBase64(),
                    new { stream.Name, stream.Id }
                );
            }
            else
            {
                await _hub.Clients.User(_user.Id.ToString())
                    .SendAsync("log", $"Stream {id} with name {filename} skipped");
            }
        };

        streamRipperInstance.StreamEndedEventHandlers += async (sender, arg) =>
        {
            await _hub.Clients.User(_user.Id.ToString()).SendAsync("log", $"Stream {id} ended");

            _state.StreamItems[id].State = StreamStatusEnum.Stopped;
        };

        streamRipperInstance.StreamFailedHandlers += async (sender, arg) =>
        {
            await _hub.Clients.User(_user.Id.ToString()).SendAsync("log", $"Stream {id} failed", arg.Exception?.Message);

            _state.StreamItems[id].State = StreamStatusEnum.Fail;

            _logger.LogError($"Failed to start {id}", arg.Exception);
        };

        streamRipperInstance.StreamStartedEventHandlers += async (sender, arg) =>
        {
            await _hub.Clients.User(_user.Id.ToString()).SendAsync("log", $"Stream {id} started");
        };

        streamRipperInstance.StreamUpdateEventHandlers += async (sender, arg) =>
        {
            await _hub.Clients.User(_user.Id.ToString()).SendAsync("log",
                $"Stream {id} updated with {arg.SongRawPartial.Length} bytes");
        };

        // Start the ripper
        streamRipperInstance.Start();

        // Add to the dictionary
        _state.StreamItems[id] = new StreamItem
        {
            User = _user,
            StreamRipper = streamRipperInstance,
            State = StreamStatusEnum.Started
        };

        await _configLogic.UpdateGlobalConfig(c => new GlobalConfigViewModel(c)
        {
            StartedStreams = c.StartedStreams.Add(id)
        });

        return true;
    }

    /// <summary>
    /// Stop the stream given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> Stop(int id)
    {
        if (_state.StreamItems.ContainsKey(id) && _state.StreamItems[id].User.Id == _user.Id)
        {
            _state.StreamItems[id].StreamRipper.Dispose();

            _state.StreamItems.Remove(id);

            await _configLogic.UpdateGlobalConfig(c => new GlobalConfigViewModel(c)
            {
                StartedStreams = c.StartedStreams.Remove(id)
            });

            return true;
        }

        return false;
    }
}