namespace ProvidingFood.Model
{
	public class Restaurant
	{
		public int RestaurantId { get; set; }
		public int UserId { get; set; }
		public string RestaurantName { get; set; }
		public string RestaurantEmail { get; set; }
		public string RestaurantPhone { get; set; }
		public string Address { get; set; }
		public string WorkingHours { get; set; }
	}
}
