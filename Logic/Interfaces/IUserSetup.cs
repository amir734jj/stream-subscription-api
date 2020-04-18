using System.Threading.Tasks;
using Models.Models;

namespace Logic.Interfaces
{
    public interface IUserSetup
    {
        Task Setup(User user);
    }
}