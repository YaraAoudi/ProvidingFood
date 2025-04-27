using System.ComponentModel.DataAnnotations;

namespace ProvidingFood.DTO
{
	public class RestaurantUpdateModel
	{
		[Required]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[Phone]
		public string PhoneNumber { get; set; }

		public string Address { get; set; }

		[Required]
		public string RestaurantName { get; set; }

		[EmailAddress]
		public string RestaurantEmail { get; set; }

		[Phone]
		public string RestaurantPhone { get; set; }
		public string RestaurantAddress { get; set; }
		public string WorkingHours { get; set; }
	}
}
