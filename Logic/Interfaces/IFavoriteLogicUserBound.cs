using System.IO;
using System.Threading.Tasks;

namespace Logic.Interfaces;

public interface IFavoriteLogicUserBound
{
    Task UploadFavorite(string filename, MemoryStream stream);
}