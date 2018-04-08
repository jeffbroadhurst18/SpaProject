using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaProject.Data.Items;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SpaProject.Data
{
	public class SpaRepository : ISpaRepository
	{
		private readonly SpaContext _ctx;
		private readonly ILogger<SpaRepository> logger;

		public SpaRepository(SpaContext ctx, ILogger<SpaRepository> logger)
		{
			this._ctx = ctx;
			this.logger = logger;
		}

		public IEnumerable<Product> GetAllProducts()
		{
			try
			{
				logger.LogInformation("In GetAllProducts");
				return _ctx.Products.OrderBy(i => i.Id);
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
				return _ctx.Products.Where(i => i.Id == id).FirstOrDefault();
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
				return _ctx.Products.Where(i => i.Category == category).OrderBy(i => i.Id);
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
					return _ctx.Orders.Include(u => u.User).Include(i => i.Items).ThenInclude(v => v.Product).OrderByDescending(o => o.OrderDate).ToList();
				}
				return _ctx.Orders.Include(u => u.User).OrderByDescending(o => o.OrderDate).ToList();
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
				return _ctx.Orders.Where(i => i.Id == id).Include(i => i.Items)
					.ThenInclude(p => p.Product).Include(u => u.User)
					.OrderByDescending(o => o.OrderDate).ToList();
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to GetOrdersById {ex.Message} {ex.StackTrace}");
				return null;
			}
		}
		
		public void AddOrder(Order order)
		{
			//Convert new product to lookup of product
			foreach (var item in order.Items)
			{
				item.Product = _ctx.Products.Find(item.Product.Id);
			}
			order.CreatedOn = DateTime.Now;
			order.ModifiedOn = DateTime.Now;
			AddEntity(order);

			//Subtract the order from stock levels
			foreach (var item in order.Items)
			{
				var product = _ctx.Products.Find(item.Product.Id);
				if (product == null) { return; }
				product.StockLevel = product.StockLevel - item.Quantity;
				_ctx.Update(product);
			}
		}

		public Order UpdateOrder(Order order)
		{
			order.ModifiedOn = DateTime.Now;
			_ctx.Update(order);

			if (order.Status == OrderStatusValue.Completed) { return order; }

			foreach (var item in order.Items)
			{
				var product = _ctx.Products.Find(item.Product.Id);
				if (product == null) { return order; }
				switch (order.Status)
				{
					case OrderStatusValue.Cancelled:
					case OrderStatusValue.Returned:
						product.StockLevel = product.StockLevel + item.Quantity;
						break;
				}
				_ctx.Update(product);
			}
			return order;
		}

		public void AddEntity(object model)
		{
			_ctx.Add(model); //The context can work out which type the object is and save it appropriately.
		}

		public bool SaveAll()
		{
			return _ctx.SaveChanges() > 0;
		}

		public IEnumerable<Order> GetOrdersByUser(string user)
		{
			return _ctx.Orders.Where(l => l.User.UserName == user).Include(i => i.Items)
					.ThenInclude(p => p.Product).Include(u => u.User)
					.OrderByDescending(x => x.OrderNumber);
		}

		public int GetNextOrderId()
		{
			var returnVal = _ctx.Config.FirstOrDefault().NextOrderNumber;
			var config = _ctx.Config.FirstOrDefault();
			config.NextOrderNumber = returnVal + 1;
			SaveAll();
			return returnVal;
		}

		public IEnumerable<OrderItem> GetOrderItems(int id)
		{
			var order = _ctx.Orders.Where(i => i.Id == id).Include(t => t.Items)
				.ThenInclude(p => p.Product).FirstOrDefault();
			return order.Items;
		}

		public StoreUser GetLastUser()
		{
			var lastuser = _ctx.Users.OrderByDescending(o => o.PersonalIdNumber).FirstOrDefault();
			return lastuser;
		}

		public Address GetAddress(string user)
		{
			var address = _ctx.Address.Where(a => a.Username == user).FirstOrDefault();
			return address;
		}

		public Address UpdateAddress(Address address)
		{
			var itExists = _ctx.Address.Any(a => a.Username == address.Username);
			
			if (itExists)
			{
				_ctx.Update(address);
			}
			else
			{
				return null;
			}
			_ctx.SaveChanges();
			return _ctx.Address.Where(a => a.Username == address.Username).FirstOrDefault();
		}

		public Address AddAddress(Address address)
		{
				_ctx.Add(address);
				_ctx.SaveChanges();
			return _ctx.Address.Where(a => a.Username == address.Username).FirstOrDefault();
		}

		public bool AddAddressToUser(StoreUser currentUser, Address updatedAddress)
		{
			currentUser.Address = updatedAddress;
			_ctx.Update(currentUser);
			_ctx.SaveChanges();
			return true;
		}
	}
}
