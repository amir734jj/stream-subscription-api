using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stream = Models.Models.Stream;
using static Models.Constants.ApplicationConstants;

namespace Logic.Logic
{
    public class UserSetup : IUserSetup
    {
        private readonly DbContext _dbContext;
        private readonly IStreamRipperManager _streamRipperManager;
        private readonly IUserLogic _userLogic;

        public UserSetup(DbContext dbContext, IStreamRipperManager streamRipperManager, IUserLogic userLogic)
        {
            _dbContext = dbContext;
            _streamRipperManager = streamRipperManager;
            _userLogic = userLogic;
        }

        public async Task Setup(int userId)
        {
            var user = await _userLogic.Get(userId);
            
            var fileString = await File.ReadAllTextAsync(SetupUserRecipe);

            JsonConvert.DeserializeAnonymousType(fileString, new {Streams = new List<Stream>()})
                .Streams
                .ForEach(x => user.Streams.Add(x));

            await _dbContext.SaveChangesAsync();

            await _streamRipperManager.StartMany(user.Streams);
        }
    }
}