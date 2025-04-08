using EcommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSystem.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext context;

		public OrderRepository(ApplicationDbContext context)
        {
			this.context = context;
		}

        public void Add(Order order)
		{
			context.Add(order);
		}

		public List<Order> GetAll()
		{
			return context.Orders.ToList();
		}

		public List<Order> GetOrdersByUserId(string userId)
		{
			return context.Orders.Where(o => o.UserId == userId).ToList();
		}

		public Order GetOrderWithItemsById(int orderId)
		{
			return context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == orderId);
		}

		public Order GetOrderById(int id)
		{
			return context.Orders.FirstOrDefault(o => o.Id == id);
		}

		public void Update(Order order)
		{
			context.Update(order);
		}

		public void Save()
		{
			context.SaveChanges();
		}
	}
}
