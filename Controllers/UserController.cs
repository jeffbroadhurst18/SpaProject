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
using Microsoft.EntityFrameworkCore;
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
			UserManager<StoreUser> userManager, RoleManager<IdentityRole> roleManager)
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

		[HttpGet("getuser/{user}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> GetUser(string user)
		{
			StoreUser storeUser = await _userManager.FindByNameAsync(user);
			return Ok(storeUser);
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
					PersonalIdNumber = (int.Parse(lastUser.PersonalIdNumber.ToString()) + 1).ToString()
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

		[HttpGet("getusers")]
		public async Task<IActionResult> GetUsers(string user)
		{
			List<string> userNames = new List<string>();
			var users = await _userManager.Users.ToListAsync();
			foreach (var u in users)
			{
				userNames.Add(u.UserName);
			}
			if (userNames.Count() > 0)
			{
				return Ok(userNames.OrderBy(u => u));
			}
			return BadRequest();
		}

		[HttpGet("getaddress/{user}")]
		public IActionResult GetAddress(string user)
		{
			try
			{
				return Ok(_mapper.Map<Address, AddressViewModel>(_repository.GetAddress(user)));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.ToString());
			}
		}

		[HttpPost("address")]
		public async Task<IActionResult> Address([FromBody] AddressViewModel avm)
		{
			try
			{
				var newAddress = _mapper.Map<AddressViewModel, Address>(avm);
				var currentUser = await _userManager.FindByNameAsync(avm.username);
				var updatedAddress = _repository.AddAddress(newAddress);
				_repository.AddAddressToUser(currentUser, updatedAddress);
				return Ok(_mapper.Map<Address, AddressViewModel>(updatedAddress));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.ToString());
			}

		}

		[HttpPut("address/{username}")]
		public async Task<IActionResult> Address(string username, [FromBody] AddressViewModel avm)
		{
			try
			{
				var newAddress = _mapper.Map<AddressViewModel, Address>(avm);
				var currentUser = await _userManager.FindByNameAsync(username);
				var updatedAddress = _repository.UpdateAddress(newAddress);
				if (updatedAddress == null) return NotFound($"CAn't find this address");
				_repository.AddAddressToUser(currentUser, updatedAddress);
				return Ok(_mapper.Map<Address, AddressViewModel>(updatedAddress));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.ToString());
			}

		}
	}
}