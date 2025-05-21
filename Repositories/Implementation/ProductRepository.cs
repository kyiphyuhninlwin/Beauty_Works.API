using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        // create and assign field
        private readonly ApplicationDbContext dbContext;

        //ctor to connect with dbcontext
        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int productID)
        {
            var existingProduct = await dbContext.Products.Include(sc => sc.Subcategory).Include(s=> s.Status)
                .Include(b=> b.Brand).FirstOrDefaultAsync(p => p.ID == productID);

            if(existingProduct != null)
            {
                dbContext.Products.Remove(existingProduct);
                await dbContext.SaveChangesAsync();
                return existingProduct;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int productID)
        {
            return await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).FirstOrDefaultAsync(p => p.ID == productID);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).FirstOrDefaultAsync(p => p.ID == product.ID);

            if (existingProduct != null)
            {
                dbContext.Entry(existingProduct).CurrentValues.SetValues(product);
                await dbContext.SaveChangesAsync();
                return existingProduct;
            }
            else
            {
                return null;
            }
        }
    }
}
