using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public void Add(Product product)
        {
            _repository.Add(product);
        }

        public void DeleteById(int id)
        {
            var product = _repository.GetById(id);
            if (product != null)
            {
                product.IsDeleted = true;
                _repository.DeleteById(id); 
            }

        }

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public Product GetById(int id)
        {

            return _repository.GetById(id);
        }

        public void Update(Product product)
        {
            _repository.Update(product);
        }
    }
}
