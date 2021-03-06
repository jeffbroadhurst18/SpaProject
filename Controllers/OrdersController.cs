﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaProject.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SpaProject.Data.Items;
using SpaProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SpaProject.Controllers
{
	[Produces("application/json")]
	[Route("api/Orders")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class OrdersController : Controller
	{
		private readonly ISpaRepository _repository;
		private readonly ILogger<OrdersController> _logger;
		private readonly IMapper _mapper;
		private readonly UserManager<StoreUser> _userManager;

		public OrdersController(ISpaRepository repository, ILogger<OrdersController> logger, IMapper mapper,
			UserManager<StoreUser> userManager)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
			_userManager = userManager;
		}

		[HttpGet("{includeItems:bool}")]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				_logger.LogTrace("Calling Get");
				var results = _repository.GetOrders(includeItems);
				var userList = _userManager.Users;

				foreach (var result in results)
				{
					result.User = userList.Where(u => u.Id == result.User.Id).FirstOrDefault();
				}

				return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}

		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			try
			{
				_logger.LogTrace("Calling Get with Id");
				var results = _repository.GetOrdersById(id);
				return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}

		[HttpGet("{user}")]
		public IActionResult Get(string user)
		{
			try
			{
				_logger.LogTrace("Calling Get for User");
				var results = _repository.GetOrdersByUser(user);
				return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result");
			}
		}

		[HttpGet("orderitems/{id:int}")]
		public IActionResult GetOrderItems(int id)
		{
			try
			{
				_logger.LogTrace("Calling GetOrderItems");
				var results = _repository.GetOrderItems(id);
				return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(results));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]OrderViewModel model)
		{
			//add to db
			try
			{
				if (ModelState.IsValid)
				{
					_logger.LogTrace("Calling Post");
					var newOrder = _mapper.Map<OrderViewModel, Order>(model);

					if (newOrder.OrderDate == DateTime.MinValue)
					{
						newOrder.OrderDate = DateTime.Now;
					}
					// User = list of claims from token. Convertes this to a store user
					var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
					newOrder.User = currentUser;
					newOrder.OrderNumber = _repository.GetNextOrderId().ToString();

					_repository.AddOrder(newOrder);
					if (_repository.SaveAll())
					{
						var vm = _mapper.Map<Order, OrderViewModel>(newOrder);
						//model.id will be updated after save.
						return Created($"/api/orders/{vm.OrderId}", vm); //Return 201 code which is created
					}
				}
				else
				{
					return BadRequest(ModelState);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to save new order - {ex.Message}");
			}
			return BadRequest("Post failed");
		}

		[HttpPost("setstatus")]
		public IActionResult SetStatus([FromBody]OrderViewModel model)
		{
			try
			{
				_logger.LogTrace("Calling SetStatus");
				var updatedOrder = Mapper.Map<OrderViewModel, Order>(model);

				var retrievedOrder = _repository.GetOrdersById(updatedOrder.Id).FirstOrDefault();
				retrievedOrder.Status = updatedOrder.Status;
				retrievedOrder = _repository.UpdateOrder(retrievedOrder);

				if (_repository.SaveAll())
				{
					var vm = _mapper.Map<Order, OrderViewModel>(retrievedOrder);
					return Ok(vm); //Return 200
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to update status on order - {ex.Message}");
			}
			return BadRequest("Post Failed");

		}
	}
}