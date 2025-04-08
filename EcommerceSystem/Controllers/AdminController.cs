using EcommerceSystem.DTOs;
using EcommerceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class AdminController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;

		public AdminController(UserManager<ApplicationUser> userManager)
        {
			this.userManager = userManager;
		}

        [HttpPost("Register")]
		public async Task<ActionResult<GeneralResponse>> Register(RegisterDto userFromRequest)
		{
			GeneralResponse response = new GeneralResponse();
			if (ModelState.IsValid)
			{
				ApplicationUser user = new ApplicationUser();
				user.FullName = userFromRequest.FullName;
				user.Email = userFromRequest.Email;
				user.UserName = userFromRequest.Email;
				user.Address = userFromRequest.Address;
				user.PhoneNumber = userFromRequest.PhoneNumber;

				IdentityResult result = await userManager.CreateAsync(user, userFromRequest.Password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Admin");

					response.IsSuccess = true;
					response.Data = "Account Created";
					return response;
				}
				foreach (var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			response.IsSuccess = false;
			response.Data = ModelState;
			return response;
		}
	}
}
