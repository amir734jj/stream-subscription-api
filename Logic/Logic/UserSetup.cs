using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Models.Models;
using Newtonsoft.Json;
using Stream = Models.Models.Stream;
using static Models.Constants.ApplicationConstants;

namespace Logic.Logic
{
    public class UserSetup : IUserSetup
    {
        private readonly IStreamLogic _streamLogic;
        
        private readonly IStreamRipperManager _streamRipperManager;

        private readonly IUserLogic _userLogic;

        public UserSetup(IStreamLogic streamLogic, IStreamRipperManager streamRipperManager, IUserLogic userLogic)
        {
            _streamLogic = streamLogic;
            _streamRipperManager = streamRipperManager;
            _userLogic = userLogic;
        }
        
        public async Task Setup(string username)
        {
            var user = (await _userLogic.GetAll()).First(x => x.UserName == username);
            
            var fileString = await File.ReadAllTextAsync(SetupUserRecipe);

            await Task.WhenAll(JsonConvert.DeserializeAnonymousType(fileString, new {Streams = new List<Stream>()})
                .Streams
                .Select(x => _streamLogic.For(user).Save(x)));

            await _streamRipperManager.StartMany(await _streamLogic.For(user).GetAll());
        }
    }
}