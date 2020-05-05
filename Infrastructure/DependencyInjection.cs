using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using zMovies.Infrastructure.Data;
using zMovies.Infrastructure.Repositories.RawSQL;
using zMovies.Core.Interfaces;

namespace zMovies.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            /*services.AddDbContext<DataContext>(options =>
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));*/

            // Adding RawSQL implementations of repositories
            services.AddTransient<IMovieCollectionRepository, MovieCollectionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMovieCollectionItemRepository, MovieCollectionItemRepository>();
            services.AddTransient<IMovieRatingRepository, MovieRatingRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();

            return services;
        }
    }
}