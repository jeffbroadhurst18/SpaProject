using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaProject.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SpaProject.Data.Items;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SpaProject.ViewModels;

namespace SpaProject.Controllers
{
	//[Produces("application/json")]
	//[Route("api/Account")]
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly IMapper _mapper;
		private readonly SignInManager<StoreUser> _signInManager;
		private readonly UserManager<StoreUser> _userManager;
		private readonly IConfiguration _config;

		public AccountController(ISpaRepository repository, ILogger<AccountController> logger,
			IMapper mapper, SignInManager<StoreUser> signInManager,
			UserManager<StoreUser> userManager,
			IConfiguration config)
		{
			_logger = logger;
			_mapper = mapper;
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
		}

		[HttpPost]
		public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				StoreUser user = await _userManager.FindByNameAsync(model.Username);
				if (user != null)
				{
					var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

					if (result.Succeeded)
					{
						// Create the token
						var claims = new[]
						{
							new Claim(JwtRegisteredClaimNames.Sub,user.Email),
							new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
							new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
						};

						var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
						var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
						var token = new JwtSecurityToken(
							_config["Tokens:Issuer"],
							_config["Tokens:Audience"], claims, expires: DateTime.Now.AddMinutes(20),
							signingCredentials: creds
							);

						var results = new
						{
							token = new JwtSecurityTokenHandler().WriteToken(token),
							SecurityTokenNoExpirationException = token.ValidTo
						};

						return Created("", results);
					}
				}
			}
			return BadRequest();
		}
	}
}