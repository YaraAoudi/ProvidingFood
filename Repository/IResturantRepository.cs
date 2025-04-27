using Microsoft.AspNetCore.Mvc;
using ProvidingFood.Model;
using System.Threading.Tasks;

namespace ProvidingFood.Repository
{
	public interface IResturantRepository
	{
	    Task<IEnumerable<Restaurant>> GetRestaurantAsync();
		Task<bool> AddRestaurantUserAsync(User user, Restaurant restaurant);
		Task<bool> UpdateRestaurantUserAsync(User user, Restaurant restaurant);
		Task<bool> DeleteRestaurantUserAsync(int userId);
	}
}
