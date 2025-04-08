using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceSystem.Models
{
	public class Order
	{
		public int Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public OrderStatus Status { get; set; }

		public ApplicationUser User { get; set; }
		public List<OrderItem> OrderItems { get; set;}
	}
	public enum OrderStatus
	{
		Pending,
		Processing,
		Shipped,
		Delivered,
		Canceled
	}
}
