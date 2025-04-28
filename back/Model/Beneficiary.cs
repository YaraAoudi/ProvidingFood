namespace ProvidingFood.Model
{
	public class Beneficiary
	{
		public int BeneficiaryId { get; set; }
		public int UserId { get; set; }
		public int NumberOfBeneficiary  { get; set; }
		public string Status { get; set; }
		public string QRCode { get; set; }
	}
}
