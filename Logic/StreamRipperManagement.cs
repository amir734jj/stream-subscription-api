using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Logic.DataStructures;
using Logic.Interfaces;
using Models.Enums;
using Models.Models;
using Models.ViewModels.Streams;
using StreamRipper.Builders;
using Stream = Models.Models.Stream;

namespace Logic
{
    public class StreamRipperManagement : IStreamRipperManagement
    {
        private readonly IStreamingLogic _streamingLogic;

        private readonly IUserLogic _userLogic;
        
        private readonly IMapper _mapper;
        
        private readonly IStreamState _state;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="state"></param>
        /// <param name="streamingLogic"></param>
        /// <param name="userLogic"></param>
        /// <param name="mapper"></param>
        public StreamRipperManagement(IStreamState state, IStreamingLogic streamingLogic, IUserLogic userLogic, IMapper mapper)
        {
            _state = state;
            _streamingLogic = streamingLogic;
            _userLogic = userLogic;
            _mapper = mapper;
        }

        /// <summary>
        /// Pass username to GetAll
        /// </summary>
        /// <returns></returns>
        public async Task<StreamsStatusViewModel> Status(User user)
        {
            var streams = await _streamingLogic.Get(x => x.User.UserName == user.UserName);

            return new StreamsStatusViewModel
            {
                Status = streams.ToDictionary(x => x,
                    x => _state.Resolve().FirstOrDefault()?.State ?? StreamStatusEnum.Stopped)
            };
        }

        /// <summary>
        /// Pass username to Save
        /// </summary>
        /// <param name="addStreamViewModel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Stream> Save(AddStreamViewModel addStreamViewModel, User user)
        {
            return await Save(_mapper.Map<Stream>(addStreamViewModel), user);
        }

        public async Task<IEnumerable<Stream>> Get(User user)
        {
            var streams = await _streamingLogic.Get(x => x.User.UserName == user.UserName);

            return streams;
        }

        public Task<Stream> Save(Stream instance, User user)
        {
            instance.User = user;

            return _streamingLogic.Save(instance);
        }

        /// <summary>
        /// Start the stream
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Start(int id)
        {
            // Already started
            if (_state.Resolve().Any(x => x.Stream.Id == id))
            {
                return false;
            }

            // Stream key
            var key = Guid.NewGuid();

            // Get the model from database
            var streamRipper = await _streamingLogic.Get(id);

            var streamRipperInstance = StreamRipperBuilder.New()
                .WithUrl(new Uri(streamRipper.Url))
                .FinalizeFilters()
                .Build();

            var uploadService = GetUploadService(streamRipper);

            streamRipperInstance.SongChangedEventHandlers += async (_, arg) =>
            {
                // Needed
                arg.SongInfo.Stream.Seek(0, SeekOrigin.Begin);

                // Create filename
                var filename = $"{arg.SongInfo.SongMetadata.Artist}-{arg.SongInfo.SongMetadata.Title}";

                // Upload the stream
                await uploadService.UploadStream(arg.SongInfo.Stream, $"{filename}.mp3");
            };
            
            streamRipperInstance.StreamEndedEventHandlers += (sender, arg) =>
            {
                _state.UpdateState((state, _) =>
                {
                    var prev = state.Find(y => y.Key == key);
                    var item = (StreamItem) prev.Clone();
                    item.State = StreamStatusEnum.Stopped;
                    return state.Replace(prev, item);
                });
            };

            streamRipperInstance.StreamFailedHandlers += (sender, arg) =>
            {
                _state.UpdateState((state, _) =>
                {
                    var prev = state.Find(y => y.Key == key);
                    var item = (StreamItem) prev.Clone();
                    item.State = StreamStatusEnum.Fail;
                    return state.Replace(prev, item);
                });
            };

            // Start the ripper
            streamRipperInstance.Start();

            // Add to the dictionary
            _state.UpdateState((state, _) => state.Add(new StreamItem
            {
                Key = key,
                Stream = streamRipper,
                State = StreamStatusEnum.Started,
                UploadService = uploadService,
                User = streamRipper.User
            }));
                
            return true;
        }

        /// <summary>
        /// Stop the stream given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Stop(int id)
        {
            if (_state.Resolve().Any(x => x.Stream.Id == id))
            {
                _state.UpdateState((state, find) => state.Remove(find(x => x.Stream.Id == id)));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns new instance of upload service
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static IUploadService GetUploadService(Stream stream)
        {
            /*switch (streamingSubscription.ServiceType)
            {
                case ServiceTypeEnum.DropBox:
                    return new DropBoxUploadService(streamingSubscription.Token);
                // case ServiceTypeEnum.BoxDotCom:
                //    return new BoxDotComUploadService();
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }*/
            throw new NotImplementedException();
        }
    }
}