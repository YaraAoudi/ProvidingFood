using Microsoft.Data.SqlClient;
using ProvidingFood.Model;
using Dapper;


namespace ProvidingFood.Repository
{
	public class UserRepository:IUserRepository
	{
		private readonly string _connectionString;

		public UserRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				string query = "SELECT * FROM [User]";
				return await connection.QueryAsync<User>(query);
			}
		}
		

		
		private async Task<int> AddBaseUserAsync(User user)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				string query = @"INSERT INTO [User] 
                                (FullName, Email, Password, PhoneNumber, Address)
                         VALUES (@FulltName, @Email, @Password, @PhoneNumber, @Address);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

				return await connection.ExecuteScalarAsync<int>(query, user);
			}
		}

		

		public async Task<bool> AddBeneficiaryUserAsync(User user, Beneficiary beneficiary)
		{
			int userId = await AddBaseUserAsync(user);

			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				string query = @"INSERT INTO [Beneficiary] 
                                (UserId, NumberOfBeneficiary,Status ,  QRCode)
                                VALUES (@UserId, @NumberOfBeneficiary, @Status, @QRCode);";

				beneficiary.UserId = userId;
				int rows = await connection.ExecuteAsync(query, beneficiary);
				return rows > 0;
			}
		}

		public async Task<bool> AddDonorUserAsync(User user, Donor donor)
		{
			int userId = await AddBaseUserAsync(user);

			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				string query = @"INSERT INTO [Donor] 
                                (UserId,Amount, DateDonated)
                                VALUES (@UserId,@Amount ,@DateDonated);";

				donor.UserId = userId;
				int rows = await connection.ExecuteAsync(query, donor);
				return rows > 0;
			}
		}

	}
}
