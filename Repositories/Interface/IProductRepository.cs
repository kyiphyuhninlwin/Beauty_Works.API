using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int productID);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(int productID);
    }
}
