using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IUserSetup
    {
        Task Setup(string username);
    }
}