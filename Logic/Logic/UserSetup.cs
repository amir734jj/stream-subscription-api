using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dal.Utilities;
using Logic.Interfaces;
using Newtonsoft.Json;
using Stream = Models.Models.Stream;
using static Models.Constants.ApplicationConstants;

namespace Logic.Logic
{
    public class UserSetup : IUserSetup
    {
        private readonly EntityDbContext _dbContext;
        private readonly IStreamRipperManager _streamRipperManager;

        public UserSetup(EntityDbContext dbContext, IStreamRipperManager streamRipperManager)
        {
            _dbContext = dbContext;
            _streamRipperManager = streamRipperManager;
        }

        public async Task Setup(int userId)
        {
            var user = _dbContext.Users
                .Include(x => x.Streams)
                .First(x => x.Id == userId);
            
            var fileString = await File.ReadAllTextAsync(SetupUserRecipe);

            user.Streams.AddRange(JsonConvert.DeserializeAnonymousType(fileString, new {Streams = new List<Stream>()})
                .Streams);

            await _dbContext.SaveChangesAsync();

            await _streamRipperManager.StartMany(user.Streams);
        }
    }
}