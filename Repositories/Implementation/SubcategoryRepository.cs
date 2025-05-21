using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SubcategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Subcategory> CreateAsync(Subcategory subcategory)
        {
            await dbContext.Subcategories.AddAsync(subcategory);
            await dbContext.SaveChangesAsync();

            return subcategory;
        }

        public async Task<Subcategory?> DeleteAsync(int subcategoryID)
        {
            var existingSubcategory = await dbContext.Subcategories.Include(s => s.ProductType).FirstOrDefaultAsync(s => s.ID == subcategoryID);

            if(existingSubcategory != null)
            {
                dbContext.Subcategories.Remove(existingSubcategory);
                await dbContext.SaveChangesAsync();
                return existingSubcategory;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            return await dbContext.Subcategories.Include(s => s.ProductType).ToListAsync();
        }

        public async Task<Subcategory?> GetByID(int subcategoryID)
        {
            return await dbContext.Subcategories.Include(s => s.ProductType).FirstOrDefaultAsync(s => s.ID == subcategoryID);
        }

        public async Task<Subcategory?> UpdateAsync(Subcategory subcategory)
        {
            var existingSubcategory = await dbContext.Subcategories.Include(s => s.ProductType).FirstOrDefaultAsync(s => s.ID == subcategory.ID);

            if(existingSubcategory == null)
            {
               return null;
            }
            else
            {
                dbContext.Entry(existingSubcategory).CurrentValues.SetValues(subcategory);
                await dbContext.SaveChangesAsync();
                return existingSubcategory;
            }}
        
    }
}
