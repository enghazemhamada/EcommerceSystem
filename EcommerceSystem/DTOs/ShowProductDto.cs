using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceSystem.DTOs
{
	public class ShowProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
	}
}
