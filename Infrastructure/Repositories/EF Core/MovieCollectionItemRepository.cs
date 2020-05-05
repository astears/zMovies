using System.Threading.Tasks;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Data;
using zMovies.Infrastructure.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace zMovies.Infrastructure.Repositories
{
    public class MovieCollectionItemRepository : GenericRepository<MovieCollectionItem>, IMovieCollectionItemRepository
    {
        private readonly DataContext context;

        public MovieCollectionItemRepository(DataContext context) : base(context)
        {
            this.context = context;

        }

        public async Task<MovieCollectionItem> GetById(int movieId, int collectionId)
        {
            return await this.context.MovieCollectionItems.FirstOrDefaultAsync(i => i.MovieId == movieId && i.MovieCollectionId == collectionId);
        }

        public async Task<bool> Exists(int movieId, int collectionId)
        {
            return await this.context.MovieCollectionItems.AnyAsync(i => i.MovieId == movieId && i.MovieCollectionId == collectionId);
        }
    }
}