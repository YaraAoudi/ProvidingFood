using ProvidingFood.Model;

namespace ProvidingFood.Repository
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetUsersAsync();
		Task<bool> AddBeneficiaryUserAsync(User user, Beneficiary beneficiary);
		Task<bool> AddDonorUserAsync(User user, Donor donor);
	}
}
