using System.Threading.Tasks;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using Dapper;

namespace zMovies.Infrastructure.Repositories.RawSQL
{
  public class MovieCollectionItemRepository : IMovieCollectionItemRepository
  {
    public Task<MovieCollectionItem> Add(MovieCollectionItem movieCollectionItem)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> Delete(MovieCollectionItem movieCollectionItem)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> Exists(int movieId, int collectionId)
    {
      throw new System.NotImplementedException();
    }

    public Task<MovieCollectionItem> GetById(int movieId, int collectionId)
    {
      throw new System.NotImplementedException();
    }
  }
}