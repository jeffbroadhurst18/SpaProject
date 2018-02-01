﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.ViewModels
{
    public class OrderItemViewModel
    {
		[Required]
		public int Id { get; set; }
		
		[Required]
		public int Quantity { get; set; }
		[Required]
		public decimal UnitPrice { get; set; }

		[Required]
		public int ProductId { get; set; }

		public string ProductCategory { get; set; }
		public string ProductSize { get; set; }
		public decimal ProductPrice { get; set; }
		public string ProductTitle { get; set; }
		public string ProductFilePath { get; set; }
	}
}
