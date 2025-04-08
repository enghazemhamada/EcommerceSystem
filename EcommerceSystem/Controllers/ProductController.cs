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
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository productRepository;

		public ProductController(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		[HttpGet]
		[Authorize]
		public ActionResult<GeneralResponse> GetAllProducts()
		{
			List<Product> products = productRepository.GetAll();
			List<ShowProductDto> productsDto = new List<ShowProductDto>();

			foreach(Product product in products)
			{
				ShowProductDto productDto = new ShowProductDto();
				productDto.Id = product.Id;
				productDto.Name = product.Name;
				productDto.Price = product.Price;
				productDto.CategoryId = product.CategoryId;
				productDto.Description = product.Description;

				productsDto.Add(productDto);
			}

			GeneralResponse generalResponse = new GeneralResponse();
			generalResponse.IsSuccess = true;
			generalResponse.Data = productsDto;
			return generalResponse;
		}

		[HttpGet("{id}")]
		[Authorize]
		public ActionResult<GeneralResponse> GetProductById(int id)
		{
			Product product = productRepository.GetById(id);
			GeneralResponse generalResponse = new GeneralResponse();
			if(product != null)
			{
				ShowProductDto productDto = new ShowProductDto();
				productDto.Id = product.Id;
				productDto.Name = product.Name;
				productDto.Price = product.Price;
				productDto.CategoryId = product.CategoryId;
				productDto.Description = product.Description;

				generalResponse.IsSuccess = true;
				generalResponse.Data = productDto;
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = "Id invalid";
			return generalResponse;
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> AddProduct(ProductDto productDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				Product product = new Product();
				product.Name = productDto.Name;
				product.Price = productDto.Price;
				product.CategoryId = productDto.CategoryId;
				product.Description = productDto.Description;
				try
				{
					productRepository.Add(product);
					productRepository.Save();

					generalResponse.IsSuccess = true;
					generalResponse.Data = "Created done";
					return generalResponse;
				}
				catch (Exception ex)
				{
					generalResponse.IsSuccess = false;
					generalResponse.Data = ex.Message;
					return generalResponse;
				}
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> UpdateProduct(int id, ProductDto productDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				Product product = productRepository.GetById(id);
				if(product != null)
				{
					product.Name = productDto.Name;
					product.Price = productDto.Price;
					product.CategoryId = productDto.CategoryId;
					product.Description = productDto.Description;

					try
					{
						productRepository.Update(product);
						productRepository.Save();

						generalResponse.IsSuccess = true;
						generalResponse.Data = "Updated done";
						return generalResponse;
					}
					catch(Exception ex)
					{
						generalResponse.IsSuccess = false;
						generalResponse.Data = ex.Message;
						return generalResponse;
					}
				}
				ModelState.AddModelError("id", "Id invalid");
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> DeleteProduct(int id)
		{
			Product product = productRepository.GetById(id);
			GeneralResponse generalResponse = new GeneralResponse();
			if(product != null)
			{
				productRepository.Delete(product);
				productRepository.Save();

				generalResponse.IsSuccess = true;
				generalResponse.Data = "Deleted done";
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = "Id invalid";
			return generalResponse;
		}
	}
}
