using System.ComponentModel.DataAnnotations;

namespace SpaProject.ViewModels
{
	public class AddressViewModel
	{
		public int id { get; set; }
		[Required]
		public string username { get; set; }
		[Required]
		public string addressline1 { get; set; }
		public string addressline2 { get; set; }
		[Required]
		public string city { get; set; }
		[Required]
		public string postcode { get; set; }
		[Required]
		public string country { get; set; }
		[Required]
		public string telephone { get; set; }
	}
}
