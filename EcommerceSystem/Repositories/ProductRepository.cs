using EcommerceSystem.Models;

namespace EcommerceSystem.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext context;

		public ProductRepository(ApplicationDbContext context)
        {
			this.context = context;
		}

        public void Add(Product obj)
		{
			context.Add(obj);
		}

		public void Delete(Product obj)
		{
			context.Remove(obj);
		}

		public List<Product> GetAll()
		{
			return context.Products.ToList();
		}

		public Product GetById(int id)
		{
			return context.Products.FirstOrDefault(p => p.Id == id);
		}

		public void Save()
		{
			context.SaveChanges();
		}

		public void Update(Product obj)
		{
			context.Update(obj);
		}
	}
}
