using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceSystem.Models
{
	public class OrderItem
	{
		public int Id { get; set; }

		[ForeignKey("Order")]
		public int OrderId { get; set; }

		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }

		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}
