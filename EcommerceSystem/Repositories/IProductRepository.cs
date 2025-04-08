using EcommerceSystem.Models;

namespace EcommerceSystem.Repositories
{
	public interface IProductRepository
	{
		List<Product> GetAll();
		Product GetById(int id);
		void Add(Product obj);
		void Update(Product obj);
		void Delete(Product obj);
		void Save();
	}
}
