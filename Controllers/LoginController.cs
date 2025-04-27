using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidingFood.Model;
using ProvidingFood.Repository;

namespace ProvidingFood.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly ILoginRepository _userRepository;
		public LoginController(ILoginRepository userRepository)
		{
			_userRepository = userRepository;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] Login login)
		{
			if (login == null || string.IsNullOrWhiteSpace(login.Email) ||
				string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
			{
				return BadRequest("Invalid login data.");
			}

			bool isValidUser = await _userRepository.Login(login);

			if (isValidUser)
			{
				return Ok(new { message = "Login successful" });
			}
			else
			{
				return Unauthorized(new { message = "Invalid credentials" });
			}
		}
	}
}

