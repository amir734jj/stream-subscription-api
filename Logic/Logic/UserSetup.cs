using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
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

        public async Task Setup(string username)
        {
            using var nestedContainer = _container.GetNestedContainer();

            var streamRipperManager = nestedContainer.GetInstance<IStreamRipperManager>();
            var userLogic = nestedContainer.GetInstance<IUserLogic>();
            var streamLogic = nestedContainer.GetInstance<IStreamLogic>();

            var user = (await userLogic.GetAll()).First(x => x.UserName == username);

            var fileString = await File.ReadAllTextAsync(SetupUserRecipe);

            await Task.WhenAll(JsonConvert.DeserializeAnonymousType(fileString, new {Streams = new List<Stream>()})
                .Streams
                .Select(x => streamLogic.For(user).Save(x)));

            await streamRipperManager.StartMany(await streamLogic.For(user).GetAll());
        }
    }
}