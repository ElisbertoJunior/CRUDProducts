using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Models.DTOs;
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
        public IActionResult Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Product is null");
            }

            var product = new Product
            {
                Code = productDTO.Code,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Status = productDTO.Status,
                DepartmentId = productDTO.DepartmentId
            };

            _productService.Add(product);
            return Created("", productDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductUpdateDTO productDTO)
        {

            // Recupera o produto existente com base no ID
            var existingProduct = _productService.GetById(id);
            if (existingProduct == null)
                return NotFound();

            // Mapeando o DTO para a entidade Product
            existingProduct.Code = productDTO.Code;
            existingProduct.Description = productDTO.Description;
            existingProduct.Price = productDTO.Price;
            existingProduct.Status = productDTO.Status;
            existingProduct.DepartmentId = productDTO.DepartmentId;

            // Atualiza o produto
            _productService.Update(existingProduct);
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
