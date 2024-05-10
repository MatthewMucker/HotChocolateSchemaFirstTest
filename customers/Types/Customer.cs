using System.ComponentModel.DataAnnotations;

namespace customers.Types
{
	public class Customer
	{
		[Key]
		public string ID { get; set; }
		public string CompanyName { get; set; }
		public string ContactPersonName { get; set; }
		public string PhoneNumber { get; set; }
		public float? DiscountPercent { get; set; }

		public virtual ICollection<Address> Addresses { get; set; }
	}

	public class AddCustomerInput
	{
		public string CompanyName { get; set; }
		public string ContactPersonName { get; set;}
		public string PhoneNumber { get; set;}
		public float? DiscountPercent { get; set; }
	}

	public class AddCustomerResponse
	{
		public Customer Customer { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
		public int Code { get; set; }
	}
}
