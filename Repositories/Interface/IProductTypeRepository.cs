using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IProductTypeRepository
    {
        Task<ProductType> CreateAsync(ProductType productType);
        Task<IEnumerable<ProductType>> GetAllAsync();
        Task<ProductType?> GetByID(int productTypeID);
        Task<ProductType?> UpdateAsync(ProductType productType);
        Task<ProductType?> DeleteAsync(int productTypeID);
    }
}
