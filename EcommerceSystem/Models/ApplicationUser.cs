﻿using Microsoft.AspNetCore.Identity;

namespace EcommerceSystem.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		public string Address { get; set; }
		
		public List<Order> Orders { get; set; }
	}
}
