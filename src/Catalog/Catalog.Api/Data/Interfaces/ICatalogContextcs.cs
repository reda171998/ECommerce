using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data.Interfaces
{
    public interface ICatalogContextcs
    {
        IMongoCollection<Product> Products { get; }
    }
}
