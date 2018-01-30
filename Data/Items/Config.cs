using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data.Items
{
    public class Config
    {
			[Required]
			public int Id { get; set; }
			[Required]
			public int NextOrderNumber { get; set; }
	}
}
