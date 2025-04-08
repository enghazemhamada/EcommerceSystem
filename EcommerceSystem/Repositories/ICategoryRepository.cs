using EcommerceSystem.Models;

namespace EcommerceSystem.Repositories
{
	public interface ICategoryRepository
	{
		List<Category> GetAll();
		void Add(Category category);
		void Save();
	}
}
