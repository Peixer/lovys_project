using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Calendar.Data;
using Core.Calendar.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Calendar.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<bool> Insert(User user)
        {
            this._userContext.Users.Add(user);
            await this._userContext.SaveChangesAsync();

            return true;
        }

        public Task<List<User>> Find()
        {
            return this._userContext.Users.ToListAsync();
        }
    }
}