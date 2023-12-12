using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Catalog.Api.Settings;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContextcs
    {

        public CatalogContext(ICatalogDatabaseSettings settings) {

            var clients = new MongoClient(settings.ConnectionString);
            var database = clients.GetDatabase(settings.DatabaseName);
            Products = database.GetCollection<Product>(settings.CollectionName);

            CatalogContextSeed.SeedData(Products);


        } 
        public IMongoCollection<Product> Products { get; }
    }
}
