using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class ProductTypeRepository: IProductTypeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductType> CreateAsync(ProductType productType)
        {
            await dbContext.ProductTypes.AddAsync(productType);
            await dbContext.SaveChangesAsync();

            return productType;
        }

        public async Task<ProductType?> DeleteAsync(int productTypeID)
        {
            var existingProductType = await dbContext.ProductTypes.Include(pt => pt.Category).FirstOrDefaultAsync(p => p.ID == productTypeID);

            if (existingProductType != null)
            {
                dbContext.ProductTypes.Remove(existingProductType);
                await dbContext.SaveChangesAsync();
                return existingProductType;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ProductType>> GetAllAsync()
        {
            return await dbContext.ProductTypes
                .Include(pt => pt.Category)
                .ToListAsync();
        }

        public async Task<ProductType?> GetByID(int productTypeID)
        {
            return await dbContext.ProductTypes.Include(pt => pt.Category).FirstOrDefaultAsync(p => p.ID == productTypeID);
        }

        public async Task<ProductType?> UpdateAsync(ProductType productType)
        {
            var existingProductType = await dbContext.ProductTypes.Include(pt => pt.Category).FirstOrDefaultAsync(p => p.ID == productType.ID);

            if(existingProductType != null)
            {
                dbContext.Entry(existingProductType).CurrentValues.SetValues(productType);
                await dbContext.SaveChangesAsync();
                return existingProductType;
            }
            else
            {
                return null;
            }
        }
    }
}
