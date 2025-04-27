using ProvidingFood.Model;

namespace ProvidingFood.Repository
{
	public interface ILoginRepository
	{
		Task<bool> Login(Login login);
	}
}
