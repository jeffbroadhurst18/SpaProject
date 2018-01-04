using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaProject.Data.Items;
using Microsoft.Extensions.Logging;

namespace SpaProject.Data
{
	public class SpaRepository : ISpaRepository
	{
		private readonly SpaContext ctx;
		private readonly ILogger<SpaRepository> logger;

		public SpaRepository(SpaContext ctx, ILogger<SpaRepository> logger)
		{
			this.ctx = ctx;
			this.logger = logger;
		}

		public IEnumerable<Product> GetAllProducts()
		{
			try
			{
				logger.LogInformation("In GetAllProducts");
				return ctx.Products.OrderBy(i => i.Id);
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to GetAllProducts {ex.Message} {ex.StackTrace}");
				return null;
			}
		}

		public Product GetProductById(int id)
		{
			try
			{
				logger.LogInformation("In GetProductById");
				return ctx.Products.Where(i => i.Id == id).FirstOrDefault();
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to GetProductById {ex.Message} {ex.StackTrace}");
				return null;
			}
		}

		public IEnumerable<Product> GetProductByCategory(string category) 
		{
			try
			{
				logger.LogInformation("In GetProductByCategory");
				return ctx.Products.Where(i => i.Category == category).OrderBy(i => i.Id);
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to GetProductByCategory {ex.Message} {ex.StackTrace}");
				return null;
			}
		}
	}
}
