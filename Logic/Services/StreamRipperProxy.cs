using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using StreamRipper;
using StreamRipper.Interfaces;
using StreamRipper.Models;
using StreamRipper.Models.Events;

namespace Logic.Services;

public class StreamRipperProxy : IStreamRipperProxy
{
    private readonly IDictionary<Uri, StreamRipperItemProxy> _streamRippers =
        new ConcurrentDictionary<Uri, StreamRipperItemProxy>();
        
    private readonly ILogger<StreamRipperProxy> _logger;

    public StreamRipperProxy(ILogger<StreamRipperProxy> logger)
    {
        _logger = logger;
    }
        
    public IStreamRipper Proxy(Uri uri)
    {
        var existingPair = _streamRippers.FirstOrDefault(x =>
            Uri.Compare(x.Key, uri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase) == 0);

        var instance = existingPair.Value;

        if (default(KeyValuePair<Uri, StreamRipperItemProxy>).Equals(existingPair))
        {
            instance = new StreamRipperItemProxy(StreamRipperFactory.New(new StreamRipperOptions
            {
                Url = uri,
                Logger = _logger,
                MaxBufferSize = 15 * 1000000 // stop when buffer size passes 15 megabytes
            }), proxy => _streamRippers.Remove(uri));

            _streamRippers.Add(uri, instance);
        }

        return instance.Fork();
    }
}

/// <summary>
///     Dummy instance
/// </summary>
public class StreamRipperItemFork : IStreamRipper
{
    public bool Active;

    public Action<StreamRipperItemFork> OnStart { get; set; } = _ => { };

    public Action<StreamRipperItemFork> OnDispose { get; set; } = _ => { };

    public void Dispose()
    {
        Active = false;
            
        OnDispose(this);
    }

    public void Start()
    {
        Active = true;

        OnStart(this);
    }

    public EventHandler<MetadataChangedEventArg> MetadataChangedHandlers { get; set; } = (sender, arg) => { };
        
    public EventHandler<StreamUpdateEventArg> StreamUpdateEventHandlers { get; set; } = (sender, arg) => { };
        
    public EventHandler<StreamStartedEventArg> StreamStartedEventHandlers { get; set; } = (sender, arg) => { };
        
    public EventHandler<StreamEndedEventArg> StreamEndedEventHandlers { get; set; } = (sender, arg) => { };
        
    public EventHandler<SongChangedEventArg> SongChangedEventHandlers { get; set; } = (sender, arg) => { };
        
    public EventHandler<StreamFailedEventArg> StreamFailedHandlers { get; set; } = (sender, arg) => { };
}
    
public class StreamRipperItemProxy : IStreamRipper
{
    private readonly List<StreamRipperItemFork> _forks = new List<StreamRipperItemFork>();

    private readonly IStreamRipper _streamRipper;

    private bool _running;
        
    private readonly Action<StreamRipperItemProxy> _onDispose;

    public StreamRipperItemProxy(IStreamRipper streamRipper, Action<StreamRipperItemProxy> onDispose)
    {
        _streamRipper = streamRipper;
        _onDispose = onDispose;

        MetadataChangedHandlers += (sender, arg) => _forks.ForEach(fork =>
        {
            if (fork.Active) fork.MetadataChangedHandlers.Invoke(sender, arg);
        });
            
        StreamUpdateEventHandlers += (sender, arg) => _forks.ForEach(fork =>
        {
            if (fork.Active) fork.StreamUpdateEventHandlers.Invoke(sender, arg);
        }); 
            
        StreamStartedEventHandlers += (sender, arg) => _forks.ForEach(fork =>
        {
            if (fork.Active) fork.StreamStartedEventHandlers.Invoke(sender, arg);
        }); 
            
        StreamEndedEventHandlers += (sender, arg) => _forks.ForEach(fork =>
        {
            _running = false;
                
            if (fork.Active) fork.StreamEndedEventHandlers.Invoke(sender, arg);
        }); 
            
        SongChangedEventHandlers += (sender, arg) => _forks.ForEach(fork =>
        {
            if (fork.Active) fork.SongChangedEventHandlers.Invoke(sender, arg);
        }); 
            
        StreamFailedHandlers += (sender, arg) => _forks.ForEach(fork =>
        {
            _running = false;

            if (fork.Active) fork.StreamFailedHandlers.Invoke(sender, arg);
        });

        _streamRipper.MetadataChangedHandlers += MetadataChangedHandlers;
        _streamRipper.StreamUpdateEventHandlers += StreamUpdateEventHandlers;
        _streamRipper.StreamStartedEventHandlers += StreamStartedEventHandlers;
        _streamRipper.StreamEndedEventHandlers += StreamEndedEventHandlers;
        _streamRipper.SongChangedEventHandlers += SongChangedEventHandlers;
        _streamRipper.StreamFailedHandlers += StreamFailedHandlers;
    }

    public void Dispose()
    {
        _running = false;

        _onDispose(this);

        _streamRipper.Dispose();
    }

    public void Start()
    {
        if (_running)
        {
            return;
        }
            
        _running = true;

        _streamRipper.Start();
    }

    public EventHandler<MetadataChangedEventArg> MetadataChangedHandlers { get; set; } 
        
    public EventHandler<StreamUpdateEventArg> StreamUpdateEventHandlers { get; set; }
        
    public EventHandler<StreamStartedEventArg> StreamStartedEventHandlers { get; set; }
        
    public EventHandler<StreamEndedEventArg> StreamEndedEventHandlers { get; set; }
        
    public EventHandler<SongChangedEventArg> SongChangedEventHandlers { get; set; }
        
    public EventHandler<StreamFailedEventArg> StreamFailedHandlers { get; set; }

    public IStreamRipper Fork()
    {
        var fork = new StreamRipperItemFork
        {
            OnDispose = self =>
            {
                _forks.Remove(self);

                if (_forks.Count == 0)
                {
                    Dispose();
                }
            },
            OnStart = _ => Start(),
        };

        _forks.Add(fork);

        return fork;
    }
}