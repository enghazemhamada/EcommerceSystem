using EcommerceSystem.DTOs;
using EcommerceSystem.Models;
using EcommerceSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryRepository categoryRepository;

		public CategoryController(ICategoryRepository categoryRepository)
        {
			this.categoryRepository = categoryRepository;
		}

		[HttpGet]
		[Authorize]
		public ActionResult<GeneralResponse> GetAllCategories()
		{
			List<Category> categories = categoryRepository.GetAll();
			List<ShowCategoryDto> categoriesDto = new List<ShowCategoryDto>();

			foreach(Category category in categories)
			{
				ShowCategoryDto categoryDto = new ShowCategoryDto();
				categoryDto.Id = category.Id;
				categoryDto.Name = category.Name;

				categoriesDto.Add(categoryDto);
			}

			GeneralResponse generalResponse = new GeneralResponse();
			generalResponse.IsSuccess = true;
			generalResponse.Data = categoriesDto;
			return generalResponse;
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> AddCategory(CategoryDto categoryDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				Category category = new Category();
				category.Name = categoryDto.Name;

				categoryRepository.Add(category);
				categoryRepository.Save();

				generalResponse.IsSuccess = true;
				generalResponse.Data = "Created done";
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}
	}
}
