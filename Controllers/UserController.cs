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
		private ILogger<UserController> _logger;
		private IMapper _mapper;
		private UserManager<StoreUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(ISpaRepository repository, ILogger<UserController> logger, IMapper mapper,
			UserManager<StoreUser> userManager,RoleManager<IdentityRole> roleManager)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
			_userManager = userManager;
			this._roleManager = roleManager;
		}
		
		[HttpGet("getrole/{user}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> GetRole(string user)
		{
			StoreUser storeUser = await _userManager.FindByNameAsync(user);
			var roles = await _userManager.GetRolesAsync(storeUser);
			return Ok(roles.First());
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UserViewModel uvm)
		{
			try
			{
				StoreUser storeUser = await _userManager.FindByNameAsync(uvm.UserName);
				if (storeUser != null) { throw new Exception("Username already exists"); }

				storeUser = await _userManager.FindByEmailAsync(uvm.EmailAddress);

				StoreUser lastUser = _repository.GetLastUser();

				if (storeUser != null) { throw new Exception("Username already exists"); }

				storeUser = new StoreUser()
				{
					FirstName = uvm.FirstName,
					LastName = uvm.LastName,
					UserName = uvm.UserName,
					Email = uvm.EmailAddress,
					PersonalIdNumber =  (int.Parse(lastUser.PersonalIdNumber.ToString()) + 1).ToString()
				};

				//Default the password to this value.
				var result = await _userManager.CreateAsync(storeUser, "P@ssw0rd!");

				await _userManager.AddToRoleAsync(storeUser, "User");
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in UserController: {ex}");
				return BadRequest(ex.ToString());
			}
		}
	}
}