using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidingFood.DTO;
using ProvidingFood.Model;
using ProvidingFood.Repository;
using static ProvidingFood.Model.RegisterModels;

namespace ProvidingFood.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResturantController : ControllerBase
	{
		private readonly IResturantRepository _resturantRepository;

		public ResturantController(IResturantRepository resturantRepository)
		{
			_resturantRepository = resturantRepository;
		}
		[HttpGet]
		public async Task<IActionResult> GetRestaurant()
		{
			var restaurants = await _resturantRepository.GetRestaurantAsync();
			return Ok(restaurants);
		}
		/////////////////////////////////////////////////////////////////////////////////////////////////////////


		[HttpPost("register/restaurant")]
		public async Task<IActionResult> RegisterRestaurant([FromBody] RestaurantRegisterModel model)
		{
			var result = await _resturantRepository.AddRestaurantUserAsync(model.User, model.Restaurant);
			if (result)
				return Ok("تم تسجيل المطعم بنجاح");
			return BadRequest("فشل تسجيل المطعم");
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////
		[HttpDelete("delete/{userId}")]
		public async Task<IActionResult> DeleteRestaurant(int userId)
		{
			try
			{
				bool isDeleted = await _resturantRepository.DeleteRestaurantUserAsync(userId);

				if (isDeleted)
				{
					return Ok(new
					{
						Success = true,
						Message = "تم حذف المطعم والمستخدم المرتبط به بنجاح"
					});
				}
				else
				{
					return NotFound(new
					{
						Success = false,
						Message = "لم يتم العثور على المطعم أو المستخدم"
					});
				}
			}
			catch (System.Exception ex)
			{
				// تسجيل الخطأ في نظام التسجيل (Logging)


				return StatusCode(500, new
				{
					Success = false,
					Message = "حدث خطأ أثناء محاولة حذف المطعم",
					Error = ex.Message
				});
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		[HttpPut("update/{userId}")]
		public async Task<IActionResult> UpdateRestaurant(int userId, [FromBody] RestaurantUpdateModel model)
		{

			// التحقق من صحة المدخلات
			if (userId <= 0)
			{
				return BadRequest(new
				{
					Success = false,
					Message = "معرف المستخدم غير صالح"
				});
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// إنشاء كائن User من النموذج
			var user = new User
			{
				Id = userId,
				FullName = model.FullName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				Address = model.Address
			};

			// إنشاء كائن Restaurant من النموذج
			var restaurant = new Restaurant
			{
				RestaurantName = model.RestaurantName,
				RestaurantEmail = model.RestaurantEmail,
				RestaurantPhone = model.RestaurantPhone,
				Address = model.RestaurantAddress,
				WorkingHours = model.WorkingHours
			};

			bool isUpdated = await _resturantRepository.UpdateRestaurantUserAsync(user, restaurant);

			if (isUpdated)
			{
				return Ok(new
				{
					Success = true,
					Message = "تم تحديث بيانات المطعم والمستخدم بنجاح"
				});
			}
			else
			{
				return NotFound(new
				{
					Success = false,
					Message = "لم يتم العثور على المطعم أو المستخدم للتحديث"
				});
			}


		}
		
	}
}
