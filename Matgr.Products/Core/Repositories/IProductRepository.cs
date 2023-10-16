using Common.Core.Repositories;
using Matgr.Products.Core.Entities;

namespace Matgr.Products.Core.Repositories
{
    public interface IProductRepository:IRepository<Product,int>
    {
    }
}
