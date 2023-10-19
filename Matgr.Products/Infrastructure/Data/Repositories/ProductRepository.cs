using Common.Infrastructure.Repositories;
using Matgr.Products.Core.Entities;
using Matgr.Products.Core.Repositories;

namespace Matgr.Products.Infrastructure.Data.Repositories
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(ProductsDbContext dbContext):base(dbContext) { }

    }
}
