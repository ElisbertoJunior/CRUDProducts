
using Microsoft.AspNetCore.Mvc;
using Moq;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using ProductAPI.Controllers;
using ProductAPI.Models;
using ProductAPI.Models.DTOs;
using ProductAPI.Services.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

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
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Code = "001", Description = "Coca-Cola", Price = 5, Status = true },
                new Product { Id = 2, Code = "002", Description = "Sorvete", Price = 10, Status = true }
            };

            _mockProductService.Setup(service => service.GetAll()).Returns(products);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count);

        }

        [Fact]
        public void GetById_ExistingProduct_ReturnsOkResult()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Code = "001", Description = "Coca-Cola", Price = 5, Status = true };

            _mockProductService.Setup(service => service.GetById(productId)).Returns(product);

            // Act
            var result =  _controller.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnProduct.Id);
        }

        [Fact]
        public void GetById_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            int nonExistingProductId = 999; // ID que não existe
            _mockProductService.Setup(service => service.GetById(nonExistingProductId)).Returns((Product)null);

            // Act
            var result =  _controller.GetById(nonExistingProductId);

            // Assert
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

            // Configura o mock para o método Add
            _mockProductService.Setup(service => service.Add(It.IsAny<Product>())).Verifiable();

            // Act
            var result = _controller.Post(newProduct);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode); // Verifica o status 201 Created

            // Verifica se o método Add foi chamado
            _mockProductService.Verify(service => service.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void UpdateProduct_ReturnsNoContent_WhenProductExists()
        {
            // Arrange
            var productId = 1; // ID do produto que será passado pela rota
            var productDTO = new ProductUpdateDTO { Code = "001", Description = "Coca-Cola", Price = 5, Status = true };
            var existingProduct = new Product { Id = productId, Code = "001", Description = "Coca-Cola", Price = 5, Status = true, DepartmentId = 1 };

            _mockProductService.Setup(service => service.GetById(productId)).Returns(existingProduct);

            // Act
            var result = _controller.Update(productId, productDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }





        [Fact]
        public void DeleteProduct_ReturnsNoContent_WhenProductExists()
        {
            // Arrange
            var existingProductId = 1;
            var existingProduct = new Product { Id = existingProductId, IsDeleted = false };

            // Configura o mock para retornar o produto existente
            _mockProductService.Setup(service => service.GetById(existingProductId)).Returns(existingProduct);

            // Act
            var result = _controller.Delete(existingProductId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

            // Verifica se o método de exclusão foi chamado no serviço
            _mockProductService.Verify(service => service.DeleteById(existingProductId), Times.Once);

            // Verifica se o produto foi marcado como excluído
            Assert.True(existingProduct.IsDeleted, "O produto não foi marcado como excluído.");
        }





    }
}