using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Calendar.Data;
using Core.Calendar.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Calendar.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly APIContext _apiContext;

        public UserRepository(APIContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<bool> Insert(User user)
        {
            this._apiContext.Users.Add(user);
            await this._apiContext.SaveChangesAsync();

            return true;
        }

        public Task<List<User>> Find()
        {
            return this._apiContext.Users.ToListAsync();
        }

        public async Task<User> FindUser(string username, string password)
        {
            return await this._apiContext.Users.SingleOrDefaultAsync(x =>
                x.Username.ToLower() == username.ToLower() && x.Password == password);
        }

        public async Task<User> FindUserByUsername(string username)
        {
            return await this._apiContext.Users.SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }
    }
}