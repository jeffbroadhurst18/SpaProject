﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.ViewModels
{
    public class OrderViewModel
    {
		[Required]
		public int OrderId { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		[Required]
		public string OrderNumber { get; set; }
		[Required]
		public ICollection<OrderItemViewModel> Items { get; set; }
	}
}
