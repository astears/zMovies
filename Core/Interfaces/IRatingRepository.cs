using System.Threading.Tasks;
using zMovies.Core.Entities;

namespace zMovies.Core.Interfaces
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
         Task<Rating> GetByValue(int value);
    }
}