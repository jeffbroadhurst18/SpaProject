using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaProject.Data.Items;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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

		public IEnumerable<Order> GetOrders(bool includeItems)
		{
			try
			{
				logger.LogInformation("In GetOrders");
				if (includeItems)
				{
					return ctx.Orders.Include(i => i.Items).OrderByDescending(o => o.OrderDate);
				}
				return ctx.Orders.OrderByDescending(o => o.OrderDate);
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to GetOrders {ex.Message} {ex.StackTrace}");
				return null;
			}
		}

		public IEnumerable<Order> GetOrdersById(int id)
		{
			try
			{
				logger.LogInformation("In GetOrders");
				return ctx.Orders.Where(i => i.Id == id).Include(i => i)
						.OrderByDescending(o => o.OrderDate);
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to GetOrdersById {ex.Message} {ex.StackTrace}");
				return null;
			}
		}
	}
}
;