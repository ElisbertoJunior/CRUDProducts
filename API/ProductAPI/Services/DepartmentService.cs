using ProductAPI.Models;
using ProductAPI.Repositories;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Department> GetAll()
        {
           return _repository.GetAll();
        }
    }
}
