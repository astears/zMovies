using System.Collections.Generic;
using System.Threading.Tasks;
using zMovies.Core.Entities;

namespace zMovies.Core.Interfaces
{
    public interface IMovieRatingRepository : IGenericRepository<MovieRating>
    {
         Task<IEnumerable<MovieRating>> GetAllByUserId(int uid);
         Task<IEnumerable<MovieRating>> GetAllByMovieId(int movieid);
         Task<bool> Exists(int uid, int movieId);
         Task<MovieRating> GetById(int uid, int movieId);
    }
}