using System.Threading.Tasks;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;

namespace zMovies.Core.Interfaces
{
    public interface IMovieCollectionItemRepository : IGenericRepository<MovieCollectionItem>
    {
         Task<bool> Exists(int movieId, int collectionId);
         Task<MovieCollectionItem> GetById(int movieId, int collectionId);
    }
}