namespace customers.Types
{
	public class Mutation
	{
		public async Task<AddCustomerResponse> AddCustomer(AddCustomerInput cust, ApplicationDbContext ctx)
		{
			Customer newCustomer = new Customer();
			newCustomer.ID = Guid.NewGuid().ToString();
			newCustomer.CompanyName = cust.CompanyName;
			newCustomer.ContactPersonName = cust.ContactPersonName;
			newCustomer.PhoneNumber = cust.PhoneNumber;
			newCustomer.DiscountPercent = cust.DiscountPercent;

			AddCustomerResponse retVal = new AddCustomerResponse();
			retVal.Customer = newCustomer;

			try
			{
				ctx.Customers.Add(newCustomer);
			}
			catch (Exception ex)
			{
				retVal.Success = false;
				retVal.Code = -1;
				retVal.Message = ex.Message;
				return retVal;
			}

			retVal.Success = true;
			retVal.Code = 0;
			retVal.Message = "Success";
			return retVal;
		}
	}
}
