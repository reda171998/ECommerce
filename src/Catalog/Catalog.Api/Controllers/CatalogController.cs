using Catalog.Api.Entities;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace Catalog.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _Logger;


        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository;
            _Logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();

            return Ok(products);
        }

        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var product = await _repository.GetProductById(id);

            if (product == null)
            {
                _Logger.LogError($"Product with {id} is not found");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("[action]/{categoryName}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName)
        {
            var product = await _repository.GetProductByCategory(categoryName);

            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            await _repository.Create(product);



            return CreatedAtRoute("GetProduct", new { id = product.Id },product);
        }
        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult>UpdateProduct([FromBody] Product product)
        {

            return Ok(await _repository.Update(product));
        }



    }



}
