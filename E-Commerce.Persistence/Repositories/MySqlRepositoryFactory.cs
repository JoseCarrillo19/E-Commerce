using E_Commerce.Domain.IRepositories;
using E_Commerce.Persistence.Data;

namespace E_Commerce.Persistence.Repositories
{
    public class MySqlRepositoryFactory : IRepositoryFactory
    {
        private readonly ApplicationDbContext _context;

        public MySqlRepositoryFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository CreateProductRepository()
        {
            return new ProductRepository(_context); 
        }
    }
}
