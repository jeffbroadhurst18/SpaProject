using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
    public class Order
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		[Required]
		public string OrderNumber { get; set; }
		[Required]
		public ICollection<OrderItem> Items { get; set; }
		[Required]
		public StoreUser User { get; set; }
	}
}
