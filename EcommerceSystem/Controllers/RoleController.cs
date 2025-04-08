using EcommerceSystem.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Admin")]
	public class RoleController : ControllerBase
	{
		private readonly RoleManager<IdentityRole> roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager)
        {
			this.roleManager = roleManager;
		}

		[HttpGet]
		public ActionResult<GeneralResponse> GetAllRoles()
		{
			GeneralResponse generalResponse = new GeneralResponse();
			generalResponse.IsSuccess = true;
			generalResponse.Data = roleManager.Roles.Select(r => new {r.Id, r.Name}).ToList();
			return generalResponse;
		}

        [HttpPost]
		public async Task<ActionResult<GeneralResponse>> AddRole(RoleDto roleDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				IdentityRole role = new IdentityRole();
				role.Name = roleDto.Name;

				IdentityResult result = await roleManager.CreateAsync(role);
				if(result.Succeeded)
				{
					generalResponse.IsSuccess = true;
					generalResponse.Data = "Role created";
					return generalResponse;
				}
				foreach(var item in result.Errors)
				{
					ModelState.AddModelError("Name", item.Description);
				}
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<GeneralResponse>> UpdateRole(string id, RoleDto roleDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				IdentityRole role = await roleManager.FindByIdAsync(id);
				if(role != null)
				{
					role.Name = roleDto.Name;
					await roleManager.UpdateAsync(role);

					generalResponse.IsSuccess = true;
					generalResponse.Data = "Updated done";
					return generalResponse;
				}
				ModelState.AddModelError("id", "Id invalid");
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<GeneralResponse>> DeleteRole(string id)
		{
			IdentityRole role = await roleManager.FindByIdAsync(id);
			GeneralResponse generalResponse = new GeneralResponse();
			if(role != null)
			{
				await roleManager.DeleteAsync(role);

				generalResponse.IsSuccess = true;
				generalResponse.Data = "Deleted Done";
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = "Id invalid";
			return generalResponse;
		}
	}
}
