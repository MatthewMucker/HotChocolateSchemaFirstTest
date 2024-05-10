using System.ComponentModel.DataAnnotations;

namespace customers.Types
{
	public class Address
	{
		[Key]
		public string ID { get; set; }
		public string CustomerID { get; set; }
		public string Line1 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZIPCode { get; set; }

		public virtual Customer Customer { get; set; }
	}
}
