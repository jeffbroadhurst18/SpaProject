using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SpaProject.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpaProject.Data
{
    public class SpaSeeder
    {
		private readonly SpaContext _ctx;
		private readonly IHostingEnvironment _hosting;
		private readonly UserManager<StoreUser> _userManager;
		private readonly ISpaRepository _repository;

		public SpaSeeder(SpaContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager, ISpaRepository repository)
		{
			_ctx = ctx;
			_hosting = hosting;
			_userManager = userManager;
			_repository = repository;
		}

		public async Task Seed()
		{
			_ctx.Database.EnsureCreated();

			var user = await _userManager.FindByEmailAsync("jeffbroadhurst18@outlook.com");
			var user2 = await _userManager.FindByEmailAsync("tombroadhurst18@outlook.com");

			if (user == null)
			{
				user = new StoreUser
				{
					FirstName = "jeff",
					LastName = "broadhurst",
					UserName = "jeff",
					Email = "jeffbroadhurst18@outlook.com",
					PersonalIdNumber = "1000"
				};

				var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

				if (result != IdentityResult.Success)
				{
					throw new InvalidOperationException("Failed to create default user");
				}
			}

			if (user2 == null)
			{
				user2 = new StoreUser
				{
					FirstName = "tom",
					LastName = "broadhurst",
					UserName = "tom",
					Email = "tombroadhurst18@outlook.com",
					PersonalIdNumber = "1001"
				};

				var result = await _userManager.CreateAsync(user2, "P@ssw0rd!");

				if (result != IdentityResult.Success)
				{
					throw new InvalidOperationException("Failed to create default user");
				}
			}

			if (!_ctx.Orders.Any())
			{
				//var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
				//var json = File.ReadAllText(filepath);
				//var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
				//_ctx.Products.AddRange(products); //Adds more than one at once.

				var products = _repository.GetAllProducts();

				var order = new Order
				{
					OrderDate = DateTime.Now,
					OrderNumber = "12345",
					Overseas = false,
					Items = new List<OrderItem>()
					{
						new OrderItem {
						Product = products.First(),
						Quantity = 5,
						UnitPrice = products.First().Price
						}
					},
					User = user
				};
				_ctx.Orders.Add(order);

				_ctx.SaveChanges();
			}
		}
	}
}
