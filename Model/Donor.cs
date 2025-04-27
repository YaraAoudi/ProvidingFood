namespace ProvidingFood.Model
{
	public class Donor
	{
		public int DonorId { get; set; }
		public int UserId { get; set; }
		public int Amount { get; set; }
		public DateTime DateDonated { get; set; }
	}
}
