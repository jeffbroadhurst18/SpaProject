using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SpaProject.Data.Items
{
	public class StoreUser : IdentityUser
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string PersonalIdNumber { get; set; }
	
		public Address Address { get; set; }
	}
}
