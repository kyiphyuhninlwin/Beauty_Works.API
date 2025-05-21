using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Brand> CreateAsync(Brand brand)
        {
            await dbContext.Brands.AddAsync(brand);
            await dbContext.SaveChangesAsync();

            return brand;
        }

        public async Task<Brand?> DeleteAsync(int brandID)
        {
            var existingBrand = await dbContext.Brands.FirstOrDefaultAsync(b => b.ID == brandID);

            if (existingBrand != null)
            {
                dbContext.Brands.Remove(existingBrand);
                await dbContext.SaveChangesAsync();
                return existingBrand;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await dbContext.Brands.ToListAsync();
        }

        public async Task<Brand?> GetByID(int brandID)
        {
            return await dbContext.Brands.FirstOrDefaultAsync(b => b.ID == brandID);
        }

        public async Task<Brand?> UpdateAsync(Brand brand)
        {
            var existingBrand = await dbContext.Brands.FirstOrDefaultAsync(b => b.ID == brand.ID);

            if (existingBrand != null)
            {
                dbContext.Entry(existingBrand).CurrentValues.SetValues(brand);
                await dbContext.SaveChangesAsync();
                return existingBrand;
            }
            else
            {
                return null;
            }
        }
    }
}
