namespace E_Commerce.Domain.IRepositories
{
    public interface IRepositoryFactory
    {
        IProductRepository CreateProductRepository();
    }
}
