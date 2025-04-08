using EcommerceSystem.Models;

namespace EcommerceSystem.Repositories
{
	public interface IOrderRepository
	{
		void Add(Order order);
		List<Order> GetAll();
		List<Order> GetOrdersByUserId(string userId);
		Order GetOrderWithItemsById(int orderId);
		void Update(Order order);
		Order GetOrderById(int id);
		void Save();
	}
}
