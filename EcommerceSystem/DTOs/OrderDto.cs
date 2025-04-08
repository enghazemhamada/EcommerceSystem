namespace EcommerceSystem.DTOs
{
	public class OrderDto
	{
		public string UserId { get; set; }
		public List<OrderItemDto> OrderItemsDto { get; set; }
	}
}
