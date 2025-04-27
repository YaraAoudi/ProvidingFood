using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProvidingFood.Model;

namespace ProvidingFood.Repository
{
	public class LoginRepository:ILoginRepository
	{
		private readonly string _connectionString;

		public LoginRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection"); ;
		}

		public async Task<bool> Login(Login login)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				var query = "SELECT * FROM [User] WHERE Email = @Email  AND password = @password";

				int userExists = await connection.ExecuteScalarAsync<int>(query, login);

				return userExists > 0;
			}
		}
	}
}
