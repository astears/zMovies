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
  public class MovieCollectionRepository : IMovieCollectionRepository
  {
    private readonly IConfiguration config;

    public MovieCollectionRepository(IConfiguration config)
    {
      this.config = config;
    }
    public async Task<MovieCollection> Add(MovieCollection movieCollection)
    {
      int id = 0;
      string sql = @"
        INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES (@Id, @Name, @Description, @UserId);
        SELECT LAST_INSERT_ID();
      ";

      using(var conn = GetConnection())
      {
        id = await conn.ExecuteScalarAsync<int>(sql, new {Id = movieCollection.Id, Name = movieCollection.Name, Description = movieCollection.Description, UserId = movieCollection.User.Id});
      }

      movieCollection.Id = id;
      return movieCollection;
    }

    public async Task<MovieCollection> Update(MovieCollection movieCollection)
    {
      string sql = @"
        UPDATE MovieCollections
        SET `Name` = @Name, Description = @Description, UserId = @UserId
        WHERE Id = @Id;
      ";

      using(var conn = GetConnection())
      {
        await conn.ExecuteAsync(sql, new {Id = movieCollection.Id, Name = movieCollection.Name, Description = movieCollection.Description, UserId = movieCollection.User.Id});
      }

      return movieCollection;
    }

    public async Task<bool> Delete(MovieCollection movieCollection)
    {
      string sql = @"
        DELETE FROM MovieCollections
        WHERE Id = @Id;
      ";

      using(var conn = GetConnection())
      {
        await conn.ExecuteAsync(sql, new {Id = movieCollection.Id});
      }

      return true;
    }

    public async Task<bool> CollectionNameExists(MovieCollection collection)
    {
      string sql = @"
        SELECT `Id`, `Name`, `Description`, `UserId`
        FROM MovieCollections
        WHERE UserId = @UserId AND `Name` = @Name;";

      using (var conn = GetConnection())
      {
          var queryResult = await conn.QueryFirstOrDefaultAsync<MovieCollection>(sql, new {UserId = collection.User.Id, Name = collection.Name});
          return queryResult == null ? false : true;
      }
    }
    
    public async Task<IEnumerable<MovieCollection>> GetAllByUserId(int uid)
    {
      List<MovieCollection> movieCollections = new List<MovieCollection>();

      string sql = @"
        SELECT `Name`, `Description`, `UserId`, `c`.`Id`, `MovieCollectionId`, `MovieId`,`u`.`Id`, `FirstName`, `LastName`, `Username`
        FROM MovieCollections as c
        LEFT JOIN MovieCollectionItems as i
          ON c.Id = i.MovieCollectionId
		    JOIN Users as u
          ON c.UserId = u.Id
        WHERE c.UserId = @UserId;";
      
      using (var conn = GetConnection())
      {
          var queryResults = await conn.QueryAsync<MovieCollection, MovieCollectionItem, User ,MovieCollection>(sql,
            (currentMovieCollection, movieItem, user) => {

              var movieCollection = movieCollections.FirstOrDefault(mc => mc.Id == currentMovieCollection.Id);

              if (movieCollection == null) 
              { 
                currentMovieCollection.User = user;
                movieCollections.Add(currentMovieCollection);
                movieCollection = currentMovieCollection;
              }
              if (movieItem != null)
              {
                movieCollection.MovieCollectionItems.Add(movieItem);
              }
              return null;
            }, new {UserId = uid}, splitOn: "MovieCollectionId, Id");
      }

      return movieCollections;
    }

    public async Task<MovieCollection> GetById(int id)
    {
      MovieCollection movieCollection = null;

      string sql = @"
        SELECT `Name`, `Description`, `UserId`, `c`.`Id`, `MovieCollectionId`, `MovieId`,`u`.`Id`, `FirstName`, `LastName`, `Username`
        FROM MovieCollections as c
        LEFT JOIN MovieCollectionItems as i
          ON c.Id = i.MovieCollectionId
        JOIN Users as u
          ON c.UserId = u.Id
        WHERE c.Id = @id;";

      using (var conn = GetConnection())
      {
          var queryResults = await conn.QueryAsync<MovieCollection, MovieCollectionItem, User, MovieCollection>(sql,
            (currentMovieCollection, movieItem, user) => {
              if (movieCollection == null) 
              { 
                currentMovieCollection.User = user;
                movieCollection = currentMovieCollection;
              }
              if (movieItem != null)
              {
                movieCollection.MovieCollectionItems.Add(movieItem);
              }
              return null;
            }, new {id = id}, splitOn: "MovieCollectionId, Id");
      }

      return movieCollection;
    }

    private IDbConnection GetConnection()
    {
        return new MySql.Data.MySqlClient.MySqlConnection(this.config.GetConnectionString("DefaultConnection"));
    }
  }
}