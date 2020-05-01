using System.IO;
using System.Threading.Tasks;
using Logic.Interfaces;
using Models.Models;

namespace Logic.Logic
{
    public class FavoriteLogic : IFavoriteLogic
    {
        private readonly ISinkService _sinkService;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="sinkService"></param>
        public FavoriteLogic(ISinkService sinkService)
        {
            _sinkService = sinkService;
        }

        public IFavoriteLogicUserBound For(User user)
        {
            return new FavoriteLogicUserBound(user, _sinkService);
        }
    }

    public class FavoriteLogicUserBound : IFavoriteLogicUserBound
    {
        private readonly ISinkService _sinkService;

        private readonly User _user;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sinkService"></param>
        public FavoriteLogicUserBound(User user, ISinkService sinkService)
        {
            _user = user;
            _sinkService = sinkService;
        }

        public async Task UploadFavorite(string filename, MemoryStream stream)
        {
            await _sinkService.ResolveFavoriteStream(_user)(filename, stream);
        }
    }
}