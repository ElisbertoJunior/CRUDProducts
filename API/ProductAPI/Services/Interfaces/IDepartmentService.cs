using ProductAPI.Models;

namespace ProductAPI.Services.Interfaces
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAll();
    }
}
