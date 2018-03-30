﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
    public class Address
    {
		[Required]
		public int Id { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string AddressLine1 { get; set; }

		public string AddressLine2 { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Postcode { get; set; }

		[Required]
		public string County { get; set; }

		[Required]
		public string Telephone { get; set; }
	}
}
