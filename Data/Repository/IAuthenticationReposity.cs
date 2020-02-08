using System.Threading.Tasks;
using newNet.Models;

namespace newNet.Data.Repository
{
    public interface IAuthenticationRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}