using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IBrandRepository
    {
        Task<Brand> CreateAsync(Brand brand);
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByID(int brandID);
        Task<Brand?> UpdateAsync(Brand brand);
        Task<Brand?> DeleteAsync(int brandID);
    }
}
