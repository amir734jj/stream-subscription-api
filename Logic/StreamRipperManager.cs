using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Logic.Models;
using Models.Enums;
using Models.Models;
using StreamRipper.Builders;
using Stream = Models.Models.Stream;

namespace Logic
{
    public class StreamRipperManager : IStreamRipperManagement
    {
        private readonly IStreamLogic _streamLogic;

        private readonly IUserLogic _userLogic;

        private readonly ISinkService _sinkService;

        private readonly StreamRipperState _state;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="state"></param>
        /// <param name="streamLogic"></param>
        /// <param name="userLogic"></param>
        /// <param name="sinkService"></param>
        public StreamRipperManager(StreamRipperState state, IStreamLogic streamLogic, IUserLogic userLogic, ISinkService sinkService)
        {
            _state = state;
            _streamLogic = streamLogic;
            _userLogic = userLogic;
            _sinkService = sinkService;
        }

        /// <summary>
        /// Pass username to GetAll
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<Stream, StreamStatusEnum>> Status(User user)
        {
            var streams = await _streamLogic.For(user).GetAll();

            return streams
                .ToDictionary(x => x,
                    x => _state.StreamItems.FirstOrDefault(x => x.Value.User.Id == user.Id).Value?.State ??
                         StreamStatusEnum.Stopped);
        }

        /// <summary>
        /// Start the stream
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Start(User user, int id)
        {
            // Already started
            if (_state.StreamItems.ContainsKey(id))
            {
                return false;
            }

            // Get the model from database
            var stream = await _streamLogic.For(user).Get(id);

            var streamRipperInstance = StreamRipperBuilder.New()
                .WithUrl(new Uri(stream.Url))
                .FinalizeFilters()
                .Build();

            var aggregatedSink = await _sinkService.Resolve(stream);

            streamRipperInstance.SongChangedEventHandlers += async (_, arg) =>
            {
                // Needed
                arg.SongInfo.Stream.Seek(0, SeekOrigin.Begin);

                // Create filename
                var filename = $"{arg.SongInfo.SongMetadata.Artist}-{arg.SongInfo.SongMetadata.Title}";

                // Upload the stream
                await aggregatedSink(arg.SongInfo.Stream, $"{filename}.mp3");
            };

            streamRipperInstance.StreamEndedEventHandlers += (sender, arg) =>
            {
                _state.StreamItems[id].State = StreamStatusEnum.Stopped;
            };

            streamRipperInstance.StreamFailedHandlers += (sender, arg) =>
            {
                _state.StreamItems[id].State = StreamStatusEnum.Fail;
            };

            // Start the ripper
            streamRipperInstance.Start();

            // Add to the dictionary
            _state.StreamItems[id] = new StreamItem
            {
                User = user,
                StreamRipper = streamRipperInstance,
                State = StreamStatusEnum.Started
            };

            return true;
        }

        /// <summary>
        /// Stop the stream given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Stop(int id)
        {
            if (_state.StreamItems.ContainsKey(id))
            {
                _state.StreamItems[id].StreamRipper.Dispose();

                _state.StreamItems.Remove(id);

                return true;
            }

            return false;
        }
    }
}