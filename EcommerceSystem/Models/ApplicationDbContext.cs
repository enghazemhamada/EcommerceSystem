﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSystem.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
