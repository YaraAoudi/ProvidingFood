using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidingFood.Model;
using ProvidingFood.Repository;
using static ProvidingFood.Model.RegisterModels;

namespace ProvidingFood.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userRepository.GetUsersAsync();
			return Ok(users);
		}
		

		[HttpPost("register/beneficiary")]
		public async Task<IActionResult> RegisterBeneficiary([FromBody] BeneficiaryRegisterModel model)
		{
			var result = await _userRepository.AddBeneficiaryUserAsync(model.User, model.Beneficiary);
			if (result)
				return Ok("تم تسجيل المستفيد بنجاح");
			return BadRequest("فشل تسجيل المستفيد");
		}

		[HttpPost("register/donor")]
		public async Task<IActionResult> RegisterDonor([FromBody] DonorRegisterModel model)
		{
			var result = await _userRepository.AddDonorUserAsync(model.User, model.Donor);
			if (result)
				return Ok("تم تسجيل المتبرع بنجاح");
			return BadRequest("فشل تسجيل المتبرع");
		}
	}
}

