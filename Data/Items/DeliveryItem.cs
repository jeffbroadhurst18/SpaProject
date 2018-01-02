using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
    public class DeliveryItem
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public Product Product { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public decimal UnitCost { get; set; }
		[Required]
		public Delivery Delivery { get; set; }
	}
}
