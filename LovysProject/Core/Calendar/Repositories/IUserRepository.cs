using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Calendar.Models;

namespace Core.Calendar.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Insert(User user);
        Task<List<User>> Find();        
        Task<User> FindUser(string username, string password);
        Task<int> FindUserByUsername(string username);
    }
}