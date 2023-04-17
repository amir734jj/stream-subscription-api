using System.IO;
using System.Threading.Tasks;
using Logic.Interfaces;

namespace Logic.Logic;

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
        return new FavoriteLogicUserBound(userId, _sinkService);
    }
}

internal class FavoriteLogicUserBound : IFavoriteLogicUserBound
{
    private readonly ISinkService _sinkService;

    private readonly int _userId;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="sinkService"></param>
    public FavoriteLogicUserBound(int userId, ISinkService sinkService)
    {
        _userId = userId;
        _sinkService = sinkService;
    }

    public async Task UploadFavorite(string filename, MemoryStream stream)
    {
        await _sinkService.ResolveFavoriteStream(_userId, stream, filename);
    }
}