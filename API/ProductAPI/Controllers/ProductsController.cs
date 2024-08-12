using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Models.DTOs;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

            if (products == null || !products.Any())
                return NotFound("Nenhum produto encontrado.");
            

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
            try 
            {
                if (productDTO == null)
                {
                    return BadRequest("Produto esta nulo");
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
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            } 
            catch (ArgumentException e) 
            {
                return BadRequest(e.Message);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar o produto.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductUpdateDTO productDTO)
        {

            try 
            {
                var existingProduct = _productService.GetById(id);
                if (existingProduct == null)
                        return NotFound();

                
                existingProduct.Code = productDTO.Code;
                existingProduct.Description = productDTO.Description;
                existingProduct.Price = productDTO.Price;
                existingProduct.Status = productDTO.Status;
                existingProduct.DepartmentId = productDTO.DepartmentId;

                
                _productService.Update(existingProduct);
                return NoContent();

            } 
            catch (ArgumentException e) 
            {
                return BadRequest(e.Message);
            } catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar produto.");
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var product = _productService.GetById(id);

            if(product == null)
                return NotFound($"Produto com Id {id} não foi encontrado.");

            _productService.DeleteById(id);
            return NoContent();
        }
    }
}
