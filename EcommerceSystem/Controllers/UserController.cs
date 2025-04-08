using EcommerceSystem.DTOs;
using EcommerceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IConfiguration configuration;

		public UserController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
			this.userManager = userManager;
			this.configuration = configuration;
		}

        [HttpPost("Register")]
		public async Task<ActionResult<GeneralResponse>> Register(RegisterDto userFromRequest)
		{
			GeneralResponse response = new GeneralResponse();
			if(ModelState.IsValid)
			{
				ApplicationUser user = new ApplicationUser();
				user.FullName = userFromRequest.FullName;
				user.Email = userFromRequest.Email;
				user.UserName = userFromRequest.Email;
				user.Address = userFromRequest.Address;
				user.PhoneNumber = userFromRequest.PhoneNumber;

				IdentityResult result = await userManager.CreateAsync(user, userFromRequest.Password);
				if(result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "User");

					response.IsSuccess = true;
					response.Data = "Account Created";
					return response;
				}
				foreach(var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			response.IsSuccess = false;
			response.Data = ModelState;
			return response;
		}

		[HttpPost("Login")]
		public async Task<ActionResult<GeneralResponse>> Login(LoginDto userFromRequest)
		{
			GeneralResponse response = new GeneralResponse();
			if(ModelState.IsValid)
			{
				ApplicationUser userFromDB = await userManager.FindByEmailAsync(userFromRequest.Email);
				if(userFromDB != null)
				{
					bool found = await userManager.CheckPasswordAsync(userFromDB, userFromRequest.Password);
					if(found)
					{
						List<Claim> claims = new List<Claim>();
						claims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDB.Id));
						claims.Add(new Claim(ClaimTypes.Name, userFromDB.FullName));
						claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
						var userRoles = await userManager.GetRolesAsync(userFromDB);
						foreach (var item in userRoles)
						{
							claims.Add(new Claim(ClaimTypes.Role, item));
						}

						SymmetricSecurityKey signInKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(configuration["JWT:SecritKey"])
						);
						SigningCredentials signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

						JwtSecurityToken myToken = new JwtSecurityToken(
							issuer: configuration["JWT:IssuerIP"],
							expires: DateTime.Now.AddHours(24),
							claims: claims,
							signingCredentials: signingCredentials
						);

						response.IsSuccess = true;
						response.Data = new
						{
							token = new JwtSecurityTokenHandler().WriteToken(myToken),
							expirations = DateTime.Now.AddHours(24)
						};
						return response;
					}
				}
				ModelState.AddModelError("Email", "Email or Password invalid");
			}
			response.IsSuccess = false;
			response.Data = ModelState;
			return response;
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<ActionResult<GeneralResponse>> GetUserById(string id)
		{
			ApplicationUser userFromDB = await userManager.FindByIdAsync(id);
			GeneralResponse response = new GeneralResponse();
			if(userFromDB != null)
			{
				ShowUserDto userDto = new ShowUserDto();
				userDto.Id = userFromDB.Id;
				userDto.FullName = userFromDB.FullName;
				userDto.Email = userFromDB.Email;
				userDto.Address = userFromDB.Address;
				userDto.PhoneNumber = userFromDB.PhoneNumber;

				response.IsSuccess = true;
				response.Data = userDto;
				return response;
			}
			response.IsSuccess = false;
			response.Data = "User not found";
			return response;
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<ActionResult<GeneralResponse>> UpdateUser(string id, UpdateUserDto userFromRequest)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				ApplicationUser userFromDB = await userManager.FindByIdAsync(id);
				if(userFromDB != null)
				{
					bool isAdmin = await userManager.IsInRoleAsync(userFromDB, "Admin");
					if(isAdmin == false)
					{
						userFromDB.FullName = userFromRequest.FullName;
						userFromDB.Address = userFromRequest.Address;
						userFromDB.PhoneNumber = userFromRequest.PhoneNumber;

						await userManager.UpdateAsync(userFromDB);

						generalResponse.IsSuccess = true;
						generalResponse.Data = "Updated done";
						return generalResponse;
					}
				}
				ModelState.AddModelError("id", "Id invalid");
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult<GeneralResponse>> DeleteUser(string id)
		{
			ApplicationUser user = await userManager.FindByIdAsync(id);
			GeneralResponse generalResponse = new GeneralResponse();
			if(user != null)
			{
				bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
				if(isAdmin == false)
				{
					await userManager.DeleteAsync(user);

					generalResponse.IsSuccess = true;
					generalResponse.Data = "Deleted done";
					return generalResponse;
				}
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = "Id invalid";
			return generalResponse;
		}
	}
}
