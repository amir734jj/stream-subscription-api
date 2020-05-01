using Models.Models;

namespace Logic.Interfaces
{
    public interface IFavoriteLogic
    {
        IFavoriteLogicUserBound For(User user);
    }
}