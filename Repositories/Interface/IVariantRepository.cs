using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IVariantRepository
    {
        Task<Variant> CreateAsync(Variant variant);
        Task<IEnumerable<Variant>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 10);
        Task<Variant?> GetByID(int variantID);
        Task<Variant?> UpdateAsync(Variant variant);
        Task<Variant?> DeleteAsync(int variantID);
    }
}
