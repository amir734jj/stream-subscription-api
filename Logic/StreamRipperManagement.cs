using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Interfaces;
using Logic.UploadServices;
using Models.Enums;
using Models.Models;
using StreamRipper.Interfaces;

namespace Logic
{
    public class StreamRipperManagement : IStreamRipperManagement
    {
        private readonly IStreamingSubscriptionLogic _streamingSubscriptionLogic;

        /// <summary>
        /// Hold on to the instances
        /// </summary>
        private readonly Dictionary<int, KeyValuePair<IUploadService, IStreamRipper>> _streamRippers = new Dictionary<int, KeyValuePair<IUploadService, IStreamRipper>>();
        
        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingSubscriptionLogic"></param>
        public StreamRipperManagement(IStreamingSubscriptionLogic streamingSubscriptionLogic)
        {
            _streamingSubscriptionLogic = streamingSubscriptionLogic;
        }

        /// <summary>
        /// Returns the stream status
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, StreamStatus> Status() =>
            _streamingSubscriptionLogic.GetAll().Select(x => x).ToDictionary(x => x.Id, x => _streamRippers.ContainsKey(x.Id) ? StreamStatus.Started : StreamStatus.Stopped);
        
        /// <summary>
        /// Start the stream
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Start(int id)
        {
            // Already started
            if (_streamRippers.ContainsKey(id))
            {
                return false;
            }
            
            // Get the model from database
            var streamRipper = _streamingSubscriptionLogic.Get(id);

            var streamRipperInstance = StreamRipper.StreamRipper.New(new Uri(streamRipper.Url));

            var uploadService = GetUploadService(streamRipper);
            
            streamRipperInstance.SongChangedEventHandlers += (_, arg) =>
            {
                var filename = $"{arg.SongInfo.SongMetadata.Artist}-{arg.SongInfo.SongMetadata.Title}";

                // Upload the stream
                uploadService.UploadStream(arg.SongInfo.Stream, $"{filename}.mp3");
            };
            
            // Start the ripper
            streamRipperInstance.Start();

            // Add to the dictionary
            _streamRippers[id] = new KeyValuePair<IUploadService, IStreamRipper>(uploadService, streamRipperInstance);

            return true;
        }

        /// <summary>
        /// Stop the stream given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Stop(int id)
        {
            if (_streamRippers.ContainsKey(id))
            {
                var streamRipperInstance = _streamRippers[id].Value;

                // Stop the ripper
                streamRipperInstance.Dispose();

                // Remove from dictionary
                _streamRippers.Remove(id);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns new instance of upload service
        /// </summary>
        /// <param name="streamingSubscription"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static IUploadService GetUploadService(StreamingSubscription streamingSubscription)
        {
            switch (streamingSubscription.ServiceType)
            {
                case ServiceTypeEnum.DropBox:
                    return new DropBoxUploadService(streamingSubscription.Token);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}