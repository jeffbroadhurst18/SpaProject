using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
    public class Delivery
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public DateTime DeliveryDate { get; set; }
		[Required]
		public string DeliveryNumber { get; set; }
		[Required]
		public ICollection<DeliveryItem> Items { get; set; }
		[Required]
		public StoreUser User { get; set; }
	}
}
