using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
	public class Order
	{
		public Order()
		{
			Overseas = false;
		}

		[Required]
		public int Id { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		[Required]
		public string OrderNumber { get; set; }
		[Required]
		public decimal OrderTotal { get; set; }
		[Required]
		public ICollection<OrderItem> Items { get; set; }
		[Required]
		public StoreUser User { get; set; }
		[Required]
		public Boolean Overseas { get; set; }
		[Required]
		public OrderStatusValue Status { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime ModifiedOn { get; set; }
	}

	public enum OrderStatusValue
	{
		Pending = 0,
		Fulfilled = 1,
		Cancelled = 2,
		Returned = 3
	}
}
