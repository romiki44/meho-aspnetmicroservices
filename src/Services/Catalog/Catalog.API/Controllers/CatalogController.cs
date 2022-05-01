using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepo;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository productRepo, ILogger<CatalogController> logger)
        {
            this.productRepo = productRepo;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productRepo.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name ="GetProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult>GetProductById(string id)
        {
            var product = await productRepo.GetProductById(id);
            if(product == null)
                return NotFound();

            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await productRepo.GetProductsByCategory(category);
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await productRepo.GetProductsByName(name);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productRepo.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var result=await productRepo.UpdateProduct(product);
            if (result)
                return Ok();

            return NotFound();
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var result=await productRepo.DeleteProduct(id);
            if (result)
                return Ok();

            return NotFound();
        }
    }
}
