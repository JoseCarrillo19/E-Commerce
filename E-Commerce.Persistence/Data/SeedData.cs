using E_Commerce.Domain.Entities;

namespace E_Commerce.Persistence.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Products.Any()) return;

            context.Products.AddRange(
                new Product { Name = "Producto A", Description = "Descripción del producto A", Price = 10.99M },
                new Product { Name = "Producto B", Description = "Descripción del producto B", Price = 20.99M },
                new Product { Name = "Producto C", Description = "Descripción del producto C", Price = 30.99M }
            );

            context.SaveChanges();
        }
    }
}
