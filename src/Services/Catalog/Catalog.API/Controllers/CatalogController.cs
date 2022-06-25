using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly Microsoft.Extensions.Logging.ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository repository, Microsoft.Extensions.Logging.ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProductsAsync();
            return Ok(products);
        }
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _repository.GetProductAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product with id {@id} not found", id);
                return NotFound();
            }
            return Ok(product);
        }
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductsByCategoryAsync(category);
            return Ok(products);
        }
        [Route("[action]/{price}", Name = "GetProductByPrice")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByPrice(decimal price)
        {
            var products = await _repository.GetProductsByPriceAsync(price);
            return Ok(products);
        }
        [Route("[action]/{min}/{max}", Name = "GetProductByPriceRange")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByPriceRange(decimal min, decimal max)
        {
            var products = await _repository.GetProductsByPriceRangeAsync(min, max);
            return Ok(products);
        }
        [Route("[action]/{name}", Name = "GetProductsByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            var products = await _repository.GetProductsByNameAsync(name);
            return Ok(products);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }
        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> UpdateProduct(string id, [FromBody] Product product)
        {
            var productToUpdate = await _repository.GetProductAsync(id);
            if (productToUpdate == null)
            {
                _logger.LogError($"Product with id {@id} not found", id);
                return NotFound();
            }
            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.Category = product.Category;
            await _repository.UpdateProduct(productToUpdate);
            return Ok(productToUpdate);
        }
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var product = await _repository.GetProductAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product with id {id} not found", id);
                return NotFound();
            }
            await _repository.DeleteProduct(id);
            return Ok(product);
        }
    }
}
