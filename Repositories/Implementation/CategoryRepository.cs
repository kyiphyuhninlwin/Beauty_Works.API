using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(int categoryID)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.ID == categoryID);

            if (existingCategory != null)
            {
                dbContext.Categories.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByID(int categoryID)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.ID == categoryID);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.ID == category.ID);

            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            else
            {
                return null;
            }

        }
    }
}
