using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaProject.Data.Items;

namespace SpaProject.Data
{
	public class SpaContext : IdentityDbContext<StoreUser>  //dbcontext which works with identity. Store User is where the identity stuff is.
	{
		//Passes the options parameter to the base class
		public SpaContext(DbContextOptions<SpaContext> options) : base(options)
		{

		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Delivery> Deliveries { get; set; }
		public DbSet<Config> Config { get; set; }
		public DbSet<Address> Address { get; set; }
	}
}
