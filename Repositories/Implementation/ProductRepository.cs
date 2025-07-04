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
            var product = await dbContext.Products.Include(sc => sc.Subcategory).Include(s=> s.Status)
                .Include(b=> b.Brand).Include(v => v.Variants).FirstOrDefaultAsync(p => p.ID == productID);

            if(product != null)
            {
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
                return product;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbContext.Products.OrderBy(p => p.OrderID)
                .Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int? subcategoryID, int? brandID, int? statusID, 
            string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100)
        {
            // query
            var products = dbContext.Products.Include(p => p.Subcategory).Include(p => p.Brand).Include(p => p.Status).AsQueryable();

            if (subcategoryID.HasValue)
            {
                products = products.Where(p => p.SubcategoryID == subcategoryID.Value);
            }

            if (brandID.HasValue)
            {
                products = products.Where(p => p.BrandID == brandID.Value);
            }

            if (statusID.HasValue)
            {
                products = products.Where(p => p.StatusID == statusID.Value);
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

                switch (sortBy.ToLower())
                {
                    case "name":
                        products = isAsc ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
                        break;
                    case "price":
                        products = isAsc ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                        break;
                    //case "orderid":
                    //    products = isAsc ? products.OrderBy(p => p.OrderID) : products.OrderByDescending(p => p.OrderID);
                    //    break;
                }
            }

            // Pagination
            // Page Number 1, Page size 5 => skip 0, take 5
            // Page Number 2, Page size 5 => skip 5, take 5 [6,7,8,9,10]
            // Page Number 3, Page size 5 => skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            products = products.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int productID)
        {
            return await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants).FirstOrDefaultAsync(p => p.ID == productID);
        }

        public async Task<Product?> GetByOrderIdAsync(int orderID)
        {
            return await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants).FirstOrDefaultAsync(p => p.OrderID == orderID);
        }

        public async Task<int> GetProductsTotal()
        {
            return await dbContext.Products.CountAsync();
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants).FirstOrDefaultAsync(p => p.ID == product.ID);

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
