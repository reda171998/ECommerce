using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.Api.Repositories.Interfaces
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContextcs _context;

        public ProductRepository(ICatalogContextcs catalogContext)
        {
            _context = catalogContext;
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p=>true).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(p=>p.Id==id).FirstOrDefaultAsync() ;


        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filter).ToListAsync();
        }


        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Update(Product product)
        {

            var updateResult = await _context.Products.ReplaceOneAsync(filter:g=>g.Id==product.Id ,replacement :product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount>0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);


            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }



    }
}
