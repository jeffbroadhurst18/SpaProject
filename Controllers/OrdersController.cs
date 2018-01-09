using System;
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

namespace SpaProject.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
		private readonly ISpaRepository _repository;
		private readonly ILogger<OrdersController> _logger;
		private readonly IMapper _mapper;

		public OrdersController(ISpaRepository repository, ILogger<OrdersController> logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				var results = _repository.GetOrders(includeItems);
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
				var results = _repository.GetOrdersById(id);
				return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}
	}
}