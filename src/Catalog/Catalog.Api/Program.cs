using Catalog.Api.Data;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Repositories.Interfaces;
using Catalog.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<CatalogDatabaseSettings>(builder.Configuration.GetSection(nameof(CatalogDatabaseSettings)));
builder.Services.AddSingleton<ICatalogDatabaseSettings>(sp=>sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);
builder.Services.AddTransient<ICatalogContextcs, CatalogContext>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddSwaggerGen();


/*builder.Services.AddSingleton<ICatalogDatabaseSettings, CatalogDatabaseSettings>();
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
