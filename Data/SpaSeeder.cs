﻿using Microsoft.AspNetCore.Hosting;
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

		public SpaSeeder(SpaContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
		{
			_ctx = ctx;
			_hosting = hosting;
			_userManager = userManager;
		}

		public async Task Seed()
		{
			_ctx.Database.EnsureCreated();

			var user = await _userManager.FindByEmailAsync("jeffbroadhurst18@outlook.com");

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

			if (!_ctx.Products.Any())
			{
				var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
				var json = File.ReadAllText(filepath);
				var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
				_ctx.Products.AddRange(products); //Adds more than one at once.

				var order = new Order
				{
					OrderDate = DateTime.Now,
					OrderNumber = "12345",
					Items = new List<OrderItem>()
					{
						new OrderItem {
						Product = products.First(),
						Quantity = 5,
						UnitPrice = products.First().Price
						}
					}
				};
				_ctx.Orders.Add(order);

				_ctx.SaveChanges();
			}
		}
	}
}