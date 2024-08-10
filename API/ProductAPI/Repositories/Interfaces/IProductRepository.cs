using ProductAPI.Models;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();

        Product GetById(int id);

        void Add(Product product);

        void Update(Product product);

        void DeleteById(int id);
    }
}
