using Microsoft.EntityFrameworkCore;

namespace customers.Types
{
    public  class Query
    {
        public static async Task<Customer?> Customer([Service] ApplicationDbContext ctx, string ID)
        {
            return await ctx.Customers.SingleOrDefaultAsync(x => x.ID == ID);
        }

        public static async Task<IEnumerable<Customer>> Customers(ApplicationDbContext ctx) => await ctx.Customers.ToListAsync();

        public static async Task<Address?> Address([Service] ApplicationDbContext ctx, string ID)
        {
            return await ctx.Addresses.SingleOrDefaultAsync(x => x.ID == ID);
        }

        public static async Task<IEnumerable<Address>> Addresses(ApplicationDbContext ctx) => await ctx.Addresses.ToListAsync();
    }
}