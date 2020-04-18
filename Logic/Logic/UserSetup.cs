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
        private readonly IContainer _container;

        public UserSetup(IContainer container)
        {
            _container = container;
        }

        public async Task Setup(User user)
        {
            using var nestedContainer = _container.GetNestedContainer();

            var dbContext = nestedContainer.GetInstance<DbContext>();
            var streamRipperManager = nestedContainer.GetInstance<IStreamRipperManager>();

            var fileString = await File.ReadAllTextAsync(SetupUserRecipe);

            JsonConvert.DeserializeAnonymousType(fileString, new {Streams = new List<Stream>()})
                .Streams
                .ForEach(x => user.Streams.Add(x));

            await dbContext.SaveChangesAsync();

            await streamRipperManager.StartMany(user.Streams);
        }
    }
}