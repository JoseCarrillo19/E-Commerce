using E_Commerce.Domain.IRepositories;
using E_Commerce.Persistence.Data;

namespace E_Commerce.Persistence.Repositories
{
    public class OracleRepositoryFactory : IRepositoryFactory
    {
        private readonly ApplicationDbContext _context;

        public OracleRepositoryFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository CreateProductRepository()
        {
            return new ProductRepository(_context); // Usa el repositorio normal o una variante si es necesario
        }
    }
}
