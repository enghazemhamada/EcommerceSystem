using EcommerceSystem.Models;

namespace EcommerceSystem.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext context;

		public CategoryRepository(ApplicationDbContext context)
        {
			this.context = context;
		}

        public void Add(Category category)
		{
			context.Add(category);
		}

		public List<Category> GetAll()
		{
			return context.Categories.ToList();
		}

		public void Save()
		{
			context.SaveChanges();
		}
	}
}
