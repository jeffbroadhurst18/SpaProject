using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
    public class Product
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public string Category { get; set; }
		[Required]
		public string Size { get; set; }
		[Required]
		public decimal Price { get; set; }
		public string Title { get; set; }
		[Required]
		public int StockLevel { get; set; }
	}
}
