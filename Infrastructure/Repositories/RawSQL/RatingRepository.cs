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
    public class RatingRepository : IRatingRepository
    {
        private readonly IConfiguration config;

        public RatingRepository(IConfiguration config)
        {
            this.config = config;
        }

        public Task<Rating> Add(Rating entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Rating>> AddRange(IEnumerable<Rating> entities)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(Rating entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Rating>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Rating> GetByValue(int value)
        {
            Rating rating;

            string sql =  @"
                SELECT `Id`, `Value` FROM `MovieDB`.`Ratings` WHERE `Value` = @Value;
            ";

            using(var conn = GetConnection())
            {
                rating = await conn.QueryFirstOrDefaultAsync<Rating>(sql, new {Value = value});
            }

            return rating;
        }

        public Task Save()
        {
            throw new System.NotImplementedException();
        }

        public Task<Rating> Update(Rating entity)
        {
            throw new System.NotImplementedException();
        }

        private IDbConnection GetConnection()
        {
            return new MySql.Data.MySqlClient.MySqlConnection(this.config.GetConnectionString("DefaultConnection"));
        }
    }
}