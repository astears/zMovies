using System.Threading.Tasks;
using zMovies.Core.Entities;

namespace zMovies.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
         Task<User> GetById(int uid);
    }
}