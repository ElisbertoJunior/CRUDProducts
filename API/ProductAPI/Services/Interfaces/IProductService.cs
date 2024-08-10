using ProductAPI.Models;

namespace ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();

        Product GetById(int id);

        void Add(Product product);

        void Update(Product product);

        void DeleteById(int id);
    }
}
