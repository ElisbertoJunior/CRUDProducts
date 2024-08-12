using ProductAPI.Models;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();


    }
}
