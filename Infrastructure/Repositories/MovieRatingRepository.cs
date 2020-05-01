using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Data;

namespace zMovies.Infrastructure.Repositories
{
    public class MovieRatingRepository : GenericRepository<MovieRating>, IMovieRatingRepository
    {
        private readonly DataContext context;

        public MovieRatingRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MovieRating>> GetAllByMovieId(int movieid)
        {
            return await this.context.MovieRatings
                    .Include(mr => mr.Rating)
                    .Include(mr => mr.User)
                    .Where(m => m.MovieId == movieid)
                    .ToListAsync();
        }

        public async Task<IEnumerable<MovieRating>> GetAllByUserId(int uid)
        {
            return await this.context.MovieRatings
                    .Include(mr => mr.Rating)
                    .Include(mr => mr.User)
                    .Where(m => m.UserId == uid)
                    .ToListAsync();
        }

        public async Task<bool> Exists(int uid, int movieId)
        {
            return await this.context.MovieRatings.AnyAsync(mr => mr.MovieId == movieId && mr.UserId == uid);
        }

        public async Task<MovieRating> GetById(int uid, int movieId)
        {
            return await this.context.MovieRatings.FirstOrDefaultAsync(mr => mr.MovieId == movieId && mr.UserId == uid);
        }


        public new async Task<MovieRating> Update(MovieRating movieRating)
         {
            MovieRating movieRatingToDelete = await this.GetById(movieRating.UserId, movieRating.MovieId);

            if (movieRatingToDelete == null)
                return null;
            
            // Have to delete in order to modify a PK value; Ef Core restriction
            await Delete(movieRatingToDelete);
            movieRating = await this.Add(movieRating);

            return movieRating;
        }
    }
}