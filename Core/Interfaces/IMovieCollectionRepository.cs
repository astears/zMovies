using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using zMovies.Core.Entities;

namespace zMovies.Core.Interfaces
{
    public interface IMovieCollectionRepository //: IGenericRepository<MovieCollection>
    {
        Task<MovieCollection> Add(MovieCollection movieCollection);
        Task<MovieCollection> Update(MovieCollection movieCollection);
        Task<bool> Delete(MovieCollection movieCollection);
        Task<IEnumerable<MovieCollection>> GetAllByUserId(int uid);
        Task<MovieCollection> GetById(int id);
        Task<bool> CollectionNameExists(MovieCollection collection);
    }
}