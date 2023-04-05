using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dal.Utilities;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stream = Models.Models.Stream;
using static Models.Constants.ApplicationConstants;

namespace Logic.Logic
{
    public class UserSetup : IUserSetup
    {
        private readonly EntityDbContext _dbContext;
        
        private readonly IStreamRipperManager _streamRipperManager;
        
        private readonly ILogger<UserSetup> _logger;

        public UserSetup(EntityDbContext dbContext, IStreamRipperManager streamRipperManager, ILogger<UserSetup> logger)
        {
            _dbContext = dbContext;
            _streamRipperManager = streamRipperManager;
            _logger = logger;
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

            try
            {
                await _streamRipperManager.StartMany(user.Streams);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to start all streams");
            }
        }
    }
}