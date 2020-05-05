using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Data;

namespace zMovies.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context) : base(context) 
        {
            this.context = context;
        }
        public async Task<User> GetById(int uid)
        {
            return await this.context.Users.FirstOrDefaultAsync(u => u.Id == uid);
        }
    }
}