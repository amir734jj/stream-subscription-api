using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Newtonsoft.Json;
using StructureMap;
using Stream = Models.Models.Stream;
using static Models.Constants.ApplicationConstants;

namespace Logic.Logic
{
    public class UserSetup : IUserSetup
    {
        private readonly DbContext _dbContext;
        private readonly IStreamRipperManager _streamRipperManager;

        public UserSetup(DbContext dbContext, IStreamRipperManager streamRipperManager)
        {
            _dbContext = dbContext;
            _streamRipperManager = streamRipperManager;
        }

        public async Task Setup(User user)
        {
            var fileString = await File.ReadAllTextAsync(SetupUserRecipe);

            JsonConvert.DeserializeAnonymousType(fileString, new {Streams = new List<Stream>()})
                .Streams
                .ForEach(x => user.Streams.Add(x));

            await _dbContext.SaveChangesAsync();

            await _streamRipperManager.StartMany(user.Streams);
        }
    }
}