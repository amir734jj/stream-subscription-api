using System.IO;
using System.Threading.Tasks;
using Logic.Interfaces;

namespace Logic.Logic
{
    public class FavoriteLogic : IFavoriteLogic
    {
        private readonly ISinkService _sinkService;

        private readonly IUserLogic _userLogic;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="sinkService"></param>
        /// <param name="userLogic"></param>
        public FavoriteLogic(ISinkService sinkService, IUserLogic userLogic)
        {
            _sinkService = sinkService;
            _userLogic = userLogic;
        }

        public IFavoriteLogicUserBound For(int userId)
        {
            return new FavoriteLogicUserBound(userId, _userLogic, _sinkService);
        }
    }

    public class FavoriteLogicUserBound : IFavoriteLogicUserBound
    {
        private readonly ISinkService _sinkService;

        private readonly int _userId;

        private readonly IUserLogic _userLogic;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLogic"></param>
        /// <param name="sinkService"></param>
        public FavoriteLogicUserBound(int userId, IUserLogic userLogic, ISinkService sinkService)
        {
            _userId = userId;
            _userLogic = userLogic;
            _sinkService = sinkService;
        }

        public async Task UploadFavorite(string filename, MemoryStream stream)
        {
            var user = await _userLogic.Get(_userId);

            await _sinkService.ResolveFavoriteStream(user)(filename, stream);
        }
    }
}