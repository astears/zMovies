using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using zMovies.Infrastructure.Data;

namespace zMovies.Infrastructure.Repositories
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly DataContext context;

        public RatingRepository(DataContext context) : base(context) 
        {
            this.context = context;
        }

        public async Task<Rating> GetByValue(int value)
        {
            return await this.context.Ratings.FirstOrDefaultAsync(r => r.Value == value);
        }
    }
}