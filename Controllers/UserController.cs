using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaProject.Data;
using SpaProject.Data.Items;
using SpaProject.ViewModels;

namespace SpaProject.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
		private ISpaRepository _repository;
		private ILogger<OrdersController> _logger;
		private IMapper _mapper;
		private UserManager<StoreUser> _userManager;

		public UserController(ISpaRepository repository, ILogger<OrdersController> logger, IMapper mapper,
			UserManager<StoreUser> userManager)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
			_userManager = userManager;
		}
		
		[HttpGet("getrole/{user}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> GetRole(string user)
		{
			StoreUser storeUser = await _userManager.FindByNameAsync(user);
			var roles = await _userManager.GetRolesAsync(storeUser);
			return Ok(roles.First());
		}

		[HttpPost()]
		public async Task<IActionResult> Add([FromBody] UserViewModel uvm)
		{
			StoreUser storeUser = await _userManager.FindByNameAsync(uvm.UserName);
			if (storeUser != null) { throw new Exception("Username already exists"); }

			storeUser = await _userManager.FindByEmailAsync(uvm.EmailAddress);
			if (storeUser != null) { throw new Exception("Username already exists"); }

			storeUser = new StoreUser()
			{
				UserName = uvm.UserName,
				Email = uvm.EmailAddress,
			};

			var result = await _userManager.CreateAsync(storeUser, "P@ssw0rd!");
		}
	}
}