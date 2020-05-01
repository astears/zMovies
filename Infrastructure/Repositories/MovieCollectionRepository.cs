using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Data;

namespace zMovies.Infrastructure.Repositories
{
    public class MovieCollectionRepository : GenericRepository<MovieCollection>, IMovieCollectionRepository
    {
        private readonly DataContext context;

        public MovieCollectionRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MovieCollection>> GetAllByUserId(int uid)
        {
            return await this.context.MovieCollections
                .Include(c => c.User)
                .Include(c => c.MovieCollectionItems)
                .Where(c => c.User.Id == uid)
                .ToListAsync();
        }

        public async Task<MovieCollection> GetById(int collectionId)
        {
            return await this.context.MovieCollections
                .Include(c => c.MovieCollectionItems)
                .FirstOrDefaultAsync(c => c.Id == collectionId);
        }

        public async Task<MovieCollection> GetByName(string collectionName)
        {
            return await this.context.MovieCollections
                .Include(c => c.MovieCollectionItems)
                .FirstOrDefaultAsync(c => c.Name == collectionName);
        }

        public async Task<bool> CollectionNameExists(MovieCollection collection)
        {    
            bool exists = await this.context.MovieCollections
                .Include(c => c.User)
                .AnyAsync(c => c.User.Id == collection.User.Id && c.Name == collection.Name);
            return exists;
        }
    }
}