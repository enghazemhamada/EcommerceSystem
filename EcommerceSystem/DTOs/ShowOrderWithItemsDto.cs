using EcommerceSystem.Models;

namespace EcommerceSystem.DTOs
{
	public class ShowOrderWithItemsDto
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public OrderStatus Status { get; set; }
		public List<ShowOrderItemDto> OrderItems { get; set; }
	}
}
