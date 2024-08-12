
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using ProductAPI.Models;
using ProductAPI.Models.DTOs;
using ProductAPI.Services.Interfaces;


namespace ProductTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;

        private readonly ProductsController _controller;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkResult_WithListOfProducts()
        {
            
            var products = new List<Product>
            {
                new Product { Id = 1, Code = "001", Description = "Coca-Cola", Price = 5, Status = true },
                new Product { Id = 2, Code = "002", Description = "Sorvete", Price = 10, Status = true }
            };

            _mockProductService.Setup(service => service.GetAll()).Returns(products);

            
            var result = _controller.Get();

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count);

        }

        [Fact]
        public void GetById_ExistingProduct_ReturnsOkResult()
        {
            
            var productId = 1;
            var product = new Product { Id = productId, Code = "001", Description = "Coca-Cola", Price = 5, Status = true };

            _mockProductService.Setup(service => service.GetById(productId)).Returns(product);

            
            var result =  _controller.GetById(productId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnProduct.Id);
        }

        [Fact]
        public void GetById_NonExistingProduct_ReturnsNotFound()
        {
            
            int nonExistingProductId = 999;
            _mockProductService.Setup(service => service.GetById(nonExistingProductId)).Returns((Product)null);

           
            var result = _controller.GetById(nonExistingProductId);

            
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            // Verifica a mensagem de erro retornada
            Assert.Equal($"Produto com Id {nonExistingProductId} não foi encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void CreateProduct_ReturnsCreated_WhenProductIsValid()
        {
            // Arrange
            var newProduct = new ProductDTO
            {
                Code = "003",
                Description = "Novo Produto",
                Price = 15.00m,
                Status = true,
                DepartmentId = 1
            };

            _mockProductService.Setup(service => service.Add(It.IsAny<Product>())).Verifiable();

          
            var result = _controller.Post(newProduct);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);

            // Verifica se o metodo Add foi chamado
            _mockProductService.Verify(service => service.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void UpdateProduct_ReturnsNoContent_WhenProductExists()
        {
            
            var productId = 1; 
            var productDTO = new ProductUpdateDTO { Code = "001", Description = "Coca-Cola", Price = 5, Status = true };
            var existingProduct = new Product { Id = productId, Code = "001", Description = "Coca-Cola", Price = 5, Status = true, DepartmentId = 1 };

            _mockProductService.Setup(service => service.GetById(productId)).Returns(existingProduct);

            var result = _controller.Update(productId, productDTO);

           
            Assert.IsType<NoContentResult>(result);
        }





        [Fact]
        public void DeleteProduct_ReturnsNoContent_WhenProductExists()
        {
            // Arrange
            var existingProductId = 1;

            
            _mockProductService.Setup(service => service.GetById(existingProductId)).Returns(new Product { Id = existingProductId, IsDeleted = false });

            // Act
            var result = _controller.Delete(existingProductId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

            // Verifica se o metodo de exclusao foi chamado no serviço
            _mockProductService.Verify(service => service.DeleteById(existingProductId), Times.Once);
        }

        


    }
}