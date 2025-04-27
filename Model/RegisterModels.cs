namespace ProvidingFood.Model
{
	public class RegisterModels
	{
		public class RestaurantRegisterModel
		{
			public User User { get; set; }
			public Restaurant Restaurant { get; set; }
		}

		public class BeneficiaryRegisterModel
		{
			public User User { get; set; }
			public Beneficiary Beneficiary { get; set; }
		}

		public class DonorRegisterModel
		{
			public User User { get; set; }
			public Donor Donor { get; set; }
		}
	}
}
