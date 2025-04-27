using Dapper;
using Microsoft.Data.SqlClient;
using ProvidingFood.Model;

namespace ProvidingFood.Repository
{
	public class ResturantRepository : IResturantRepository
	{
		private readonly string _connectionString;

		public ResturantRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		public async Task<IEnumerable<Restaurant>> GetRestaurantAsync()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				string query = "SELECT * FROM [Restaurant]";
				return await connection.QueryAsync<Restaurant>(query);
			}
		}
		private async Task<int> AddBaseUserAsync(User user)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				string query = @"INSERT INTO [User] 
                                ( FullName, Email, Password, PhoneNumber, Address)
                         VALUES (@FullName, @Email, @Password, @PhoneNumber, @Address);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

				return await connection.ExecuteScalarAsync<int>(query, user);
			}
		}
		public async Task<bool> AddRestaurantUserAsync(User user, Restaurant restaurant)
		{
			int userId = await AddBaseUserAsync(user);

			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				string query = @"INSERT INTO [Restaurant] 
                                (UserId, RestaurantName,RestaurantEmail,RestaurantPhone,Address, WorkingHours)
                         VALUES (@UserId, @RestaurantName,@RestaurantEmail, @RestaurantPhone,@Address,@WorkingHours);";

				restaurant.UserId = userId;
				int rows = await connection.ExecuteAsync(query, restaurant);
				return rows > 0;
			}
		}
		public async Task<bool> DeleteRestaurantUserAsync(int userId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				// بدء عملية transaction لضمان تكامل البيانات
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						// 1. أولاً: حذف بيانات المطعم من جدول Restaurant
						string deleteRestaurantQuery = @"DELETE FROM [Restaurant] 
                                              WHERE UserId = @UserId";

						int restaurantRows = await connection.ExecuteAsync(
							deleteRestaurantQuery,
							new { UserId = userId },
							transaction);

						// 2. ثانياً: حذف المستخدم من جدول User الأساسي
						string deleteUserQuery = @"DELETE FROM [User] 
                                         WHERE Id = @UserId";

						int userRows = await connection.ExecuteAsync(
							deleteUserQuery,
							new { UserId = userId },
							transaction);

						// إذا نجحت العمليتين نكمل العملية
						transaction.Commit();

						// نعود بـ true إذا تم حذف سجل واحد على الأقل
						return (restaurantRows > 0 || userRows > 0);
					}
					catch
					{
						// في حالة حدوث خطأ نرجع Transaction
						transaction.Rollback();
						throw; // يمكنك معالجة الخطأ بشكل مناسب هنا
					}
				}
			}
		}
		public async Task<bool> UpdateRestaurantUserAsync(User user, Restaurant restaurant)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						// 1. تحديث بيانات المستخدم الأساسية
						string updateUserQuery = @"UPDATE [User] 
                                        SET FullName = @FullName,
                                            Email = @Email,
                                            PhoneNumber = @PhoneNumber,
                                            Address = @Address
                                        WHERE Id = @Id";

						int userRows = await connection.ExecuteAsync(
							updateUserQuery,
							user,
							transaction);

						// 2. تحديث بيانات المطعم
						string updateRestaurantQuery = @"UPDATE [Restaurant] 
                                               SET RestaurantName = @RestaurantName,
                                                   RestaurantEmail = @RestaurantEmail,
                                                   RestaurantPhone = @RestaurantPhone,
                                                   Address = @Address,
                                                   WorkingHours = @WorkingHours
                                               WHERE UserId = @UserId";

						restaurant.UserId = user.Id;
						int restaurantRows = await connection.ExecuteAsync(
							updateRestaurantQuery,
							restaurant,
							transaction);

						transaction.Commit();

						// نعود بـ true إذا تم تحديث سجل واحد على الأقل
						return (userRows > 0 && restaurantRows > 0);
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		} 


		}
}
