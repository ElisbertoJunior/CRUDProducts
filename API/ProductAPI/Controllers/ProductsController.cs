using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
                return NotFound($"Produto com Id {id} não foi encontrado.");

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            _productService.Update(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("id")]
        public IActionResult Update(int id, [FromBody] Product product) 
        {
            if (id != product.Id)
                return BadRequest();

            var existingProduct = _productService.GetById(id);
            if (existingProduct == null)
                return NotFound();

            
            _productService.DeleteById(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var product = _productService.GetById(id);

            if(product == null)
                return NotFound();

            _productService.DeleteById(id);
            return NoContent();
        }
    }
}
