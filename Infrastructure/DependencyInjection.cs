using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using zMovies.Infrastructure.Data;
using zMovies.Infrastructure.Repositories;
using zMovies.Core.Interfaces;

namespace zMovies.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));

            services.AddTransient<IMovieCollectionRepository, zMovies.Infrastructure.Repositories.RawSQL.MovieCollectionRepository>();
            services.AddTransient<IUserRepository, zMovies.Infrastructure.Repositories.RawSQL.UserRepository>();
            services.AddTransient<IMovieCollectionItemRepository, zMovies.Infrastructure.Repositories.RawSQL.MovieCollectionItemRepository>();
            services.AddTransient<IMovieRatingRepository, zMovies.Infrastructure.Repositories.RawSQL.MovieRatingRepository>();
            services.AddTransient<IRatingRepository, zMovies.Infrastructure.Repositories.RawSQL.RatingRepository>();

            return services;
        }
    }
}