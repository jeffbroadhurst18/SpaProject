using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaProject.Data;
using Microsoft.Extensions.Logging;

namespace SpaProject.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
		private readonly ISpaRepository repository;
		private readonly ILogger<ProductsController> logger;

		public ProductsController(ISpaRepository repository, ILogger<ProductsController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try {
				return Ok(repository.GetAllProducts());
			}
			catch(Exception ex)
			{
				logger.LogError($"Failed to get result {ex.Message}");
				return BadRequest("Bad Result"); //return 400;
			}
		}

		[HttpGet("{category}")]
		public IActionResult Get(string category)
		{
			try
			{
				return Ok(repository.GetProductByCategory(category));
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to get result {ex.Message}");
				return BadRequest("Bad Result"); //return 400;
			}
		}
	}
}