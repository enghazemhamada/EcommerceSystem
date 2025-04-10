﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceSystem.DTOs
{
	public class ShowOrderItemDto
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
	}
}
