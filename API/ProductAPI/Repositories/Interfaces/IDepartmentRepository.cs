using ProductAPI.Models;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();

       /* Department GetById(int id);

        void Add(Department department);

        void Update(Department department);

        void Delete(int id);*/
    }
}
