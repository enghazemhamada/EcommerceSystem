﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceSystem.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		[ForeignKey("Category")]
		public int CategoryId { get; set; }

		public Category Category { get; set; }
		public List<OrderItem> OrderItems { get; set; }
	}
}
