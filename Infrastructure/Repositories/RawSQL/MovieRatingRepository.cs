extern alias MySqlConnectorAlias;

using System.Collections.Generic;
using System.Threading.Tasks;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System.Linq;

namespace zMovies.Infrastructure.Repositories.RawSQL
{
    public class MovieRatingRepository : IMovieRatingRepository
    {
        private readonly IConfiguration config;

        public MovieRatingRepository(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<MovieRating> Add(MovieRating rating)
        {
            int id = 0;

            string sql = @"
                INSERT INTO `MovieDB`.`MovieRatings` (`Id`, `MovieId`, `RatingId`, `UserId`, `Review`) VALUES (@Id, @MovieId, @RatingId, @UserId, @Review);
                SELECT LAST_INSERT_ID();
            ";

            using(var conn = GetConnection())
            {
                id = await conn.ExecuteScalarAsync<int>(sql, rating);
            }

            rating.Id = id;

            return rating;
        }

        public Task<IEnumerable<MovieRating>> AddRange(IEnumerable<MovieRating> entities)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(MovieRating rating)
        {
            string sql = @"
                DELETE FROM `MovieDB`.`MovieRatings` 
                WHERE `Id` = @Id;
            ";

            using(var conn = GetConnection())
            {
                 await conn.ExecuteScalarAsync<int>(sql,new {Id = rating.Id});
            }

            return true;
        }

        public async Task<bool> Exists(int uid, int movieId)
        {
            MovieRating rating;

            string sql = @"
                SELECT `Id`, `MovieId`, `RatingId`, `UserId`, `Review` 
                FROM `MovieDB`.`MovieRatings` 
                WHERE `UserId` = @Uid AND `MovieId` = @MovieId;
            ";

            using(var conn = GetConnection())
            {
                rating = await conn.QueryFirstOrDefaultAsync<MovieRating>(sql, new {Uid = uid, MovieId = movieId});
            }

            return rating == null ? false : true;
        }

        public Task<IEnumerable<MovieRating>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<MovieRating>> GetAllByMovieId(int movieId)
        {
            List<MovieRating> movieRatings = new List<MovieRating>();

            string sql = @"
                SELECT `mr`.`Id`, `MovieId`, `RatingId`, `UserId`, `Review`, `r`.`Id`, `Value`, `u`.`Id`, `FirstName`, `LastName`, `Username`
                FROM `MovieRatings` as mr
                JOIN `Ratings` as r
                 ON `mr`.`RatingId` = `r`.`Id`
                JOIN `Users` as u
                 ON `u`.Id = `mr`.`UserId`
                WHERE `MovieId` = @MovieId;
            ";

            using(var conn = GetConnection())
            {
                var results = await conn.QueryAsync<MovieRating, Rating, User, MovieRating>(sql,
                     (movieRating, rating, user) => {
                        movieRating.Rating = rating;
                        movieRating.User = user;

                        movieRatings.Add(movieRating);
                        return null;
                }, new {MovieId = movieId});
            }

            return movieRatings;
        }

        public async Task<IEnumerable<MovieRating>> GetAllByUserId(int uid)
        {
            List<MovieRating> movieRatings = new List<MovieRating>();

            string sql = @"
                SELECT `mr`.`Id`, `MovieId`, `RatingId`, `UserId`, `Review`, `r`.`Id`, `Value`, `u`.`Id`, `FirstName`, `LastName`, `Username`
                FROM `MovieRatings` as mr
                JOIN `Ratings` as r
                 ON `mr`.`RatingId` = `r`.`Id`
                JOIN `Users` as u
                 ON `u`.Id = `mr`.`UserId`
                WHERE `UserId` = @UserId;
            ";

            using(var conn = GetConnection())
            {
                var results = await conn.QueryAsync<MovieRating, Rating, User, MovieRating>(sql,
                     (movieRating, rating, user) => {
                        movieRating.Rating = rating;
                        movieRating.User = user;

                        movieRatings.Add(movieRating);
                        return null;
                }, new {UserId = uid});
            }

            return movieRatings;
        }

        public async Task<MovieRating> GetById(int uid, int movieId)
        {
            MovieRating rating;

            string sql = @"
                SELECT `Id`, `MovieId`, `RatingId`, `UserId`, `Review`
                FROM `MovieDB`.`MovieRatings` 
                WHERE `UserId` = @Uid AND `MovieId` = @MovieId;
            ";

            using(var conn = GetConnection())
            {
                rating = await conn.QueryFirstOrDefaultAsync<MovieRating>(sql, new {Uid = uid, MovieId = movieId});
            }

            return rating;
        }

        public Task Save()
        {
            throw new System.NotImplementedException();
        }

        public async Task<MovieRating> Update(MovieRating rating)
        {

            string sql = @"
                UPDATE `MovieDB`.`MovieRatings`
                SET `Id` = @Id, `MovieId` = @MovieId, `RatingId` = @RatingId, `UserId` = @UserId, `Review` = @Review
                WHERE `Id` = @Id;
            ";

            using(var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, rating);
            }

            return rating;
        }

        private IDbConnection GetConnection()
        {
            return new MySql.Data.MySqlClient.MySqlConnection(this.config.GetConnectionString("DefaultConnection"));
        }
    }
}