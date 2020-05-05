using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using zMovies.Core.Entities;
using zMovies.Core.Interfaces;

namespace zMovies.Infrastructure.Repositories.RawSQL
{
  public class UserRepository : IUserRepository
  { 
    private readonly IConfiguration config;

    public UserRepository(IConfiguration config)
    {
      this.config = config;
    }

    public async Task<User> GetById(int uid)
    {
      User user;
      
      string sql = @"
        SELECT `Id`, `FirstName`, `LastName`, `Username`
        FROM Users
        WHERE Id = @Id;";

      using (var conn = GetConnection())
      {
        user = await conn.QuerySingleOrDefaultAsync<User>(sql, new {Id = uid});
      }
    
      return user;
    }

    public Task<User> Add(User entity)
    {
      throw new System.NotImplementedException();
    }

    public Task<bool> Delete(User entity)
    {
      throw new System.NotImplementedException();
    }

    public Task<User> Update(User entity)
    {
      throw new System.NotImplementedException();
    }
    private IDbConnection GetConnection()
    {
        return new MySql.Data.MySqlClient.MySqlConnection(this.config.GetConnectionString("DefaultConnection"));
    }
  }
}