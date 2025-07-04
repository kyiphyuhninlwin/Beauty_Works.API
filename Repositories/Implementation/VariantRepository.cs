using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class VariantRepository: IVariantRepository
    {
        private readonly ApplicationDbContext dbContext;

        public VariantRepository(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<Variant> CreateAsync(Variant variant)
        {
            await dbContext.Variants.AddAsync(variant);
            await dbContext.SaveChangesAsync();

            return variant;
        }

        public async Task<Variant?> DeleteAsync(int variantID)
        {
            var variant = await dbContext.Variants.Include(p => p.Products).FirstOrDefaultAsync(v => v.ID == variantID);

            if (variant != null) 
            {
                dbContext.Variants.Remove(variant);
                await dbContext.SaveChangesAsync();
                return variant;
            }
            else 
            {
                return null;
            }
        }

        public async Task<IEnumerable<Variant>> GetAllAsync()
        {
            return await dbContext.Variants.Include(p => p.Products).ToListAsync();
        }

        public async Task<Variant?> GetByID(int variantID)
        {
            return await dbContext.Variants.Include(p => p.Products)
                .FirstOrDefaultAsync(v => v.ID == variantID);
        }

        public async Task<Variant?> UpdateAsync(Variant variant)
        {
            var existingVariant = await dbContext.Variants.Include(p => p.Products)
                .FirstOrDefaultAsync(v => v.ID == variant.ID);

            if(existingVariant != null)
            {
                dbContext.Entry(existingVariant).CurrentValues.SetValues(variant);
                await dbContext.SaveChangesAsync();
                return existingVariant;
            }
            else
            {
                return null;
            }
        }
    }
}
