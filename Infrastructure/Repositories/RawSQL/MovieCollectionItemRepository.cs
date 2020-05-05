using System.Threading.Tasks;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;

namespace zMovies.Infrastructure.Repositories.RawSQL
{
  public class MovieCollectionItemRepository : IMovieCollectionItemRepository
  {
    private readonly IConfiguration config;

    public MovieCollectionItemRepository(IConfiguration config)
    {
        this.config = config;
    }

    public async Task<MovieCollectionItem> Add(MovieCollectionItem movieCollectionItem)
    {
      string sql = @"
        INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES (@MovieId, @MovieCollectionId);
      ";

      using(var conn = GetConnection())
      {
        await conn.ExecuteAsync(sql, movieCollectionItem);
      }

      return movieCollectionItem;

    }

    public async Task<bool> Delete(MovieCollectionItem movieCollectionItem)
    {
      string sql = @"
        DELETE FROM `MovieDB`.`MovieCollectionItems` WHERE `MovieId` = @MovieId AND `MovieCollectionId`= @MovieCollectionId;
      ";

       using(var conn = GetConnection())
      {
        await conn.ExecuteAsync(sql, movieCollectionItem);
      }

      return true;
    }

    public async Task<bool> Exists(int movieId, int movieCollectionId)
    {
      MovieCollectionItem item;

      string sql = @"
        SELECT `MovieId`, `MovieCollectionId` FROM `MovieDB`.`MovieCollectionItems` WHERE `MovieId` = @MovieId AND `MovieCollectionId`= @MovieCollectionId;
      ";

      using(var conn = GetConnection())
      {
        item = await conn.QueryFirstOrDefaultAsync<MovieCollectionItem>(sql, new {MovieId = movieId, MovieCollectionId = movieCollectionId});
      }

      return item == null ? false : true;
    }

    public async Task<MovieCollectionItem> GetById(int movieId, int movieCollectionId)
    {
      MovieCollectionItem item;

      string sql = @"
        SELECT `MovieId`, `MovieCollectionId` FROM `MovieDB`.`MovieCollectionItems` WHERE `MovieId` = @MovieId AND `MovieCollectionId`= @MovieCollectionId;
      ";

      using(var conn = GetConnection())
      {
        item = await conn.QueryFirstOrDefaultAsync<MovieCollectionItem>(sql, new {MovieId = movieId, MovieCollectionId = movieCollectionId});
      }

      return item;
    }

    private IDbConnection GetConnection()
    {
        return new MySql.Data.MySqlClient.MySqlConnection(this.config.GetConnectionString("DefaultConnection"));
    }
  }
}