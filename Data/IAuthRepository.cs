using System.Threading.Tasks;
using Stazo.API.Models;

namespace Stazo.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string email, string password);
         Task<User> ChangeUserPrivileges(int id);
         Task<bool> UserExists(string username, string email);
         Task<bool> UsernameExists(string username);
         Task<bool> EmailExists(string email);
    }
}