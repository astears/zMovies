using System.Collections.Generic;
using System.Threading.Tasks;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Repositories;

namespace Infrastructure.Repositories.RawSQL
{
    public class MovieCollectionRepository : GenericRepository<MovieCollection>, IMovieCollectionRepository
    {
        public Task<bool> CollectionNameExists(MovieCollection collection)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MovieCollection>> GetAllByUserId(int uid)
        {
            throw new System.NotImplementedException();
        }

        public Task<MovieCollection> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MovieCollection> GetByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}