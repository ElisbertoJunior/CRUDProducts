using MySql.Data.MySqlClient;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Department> GetAll()
        {
            var departments = new List<Department>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Department";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetInt32("Id"),
                            Code = reader.GetString("Code"),
                            Description = reader.GetString("Description")
                        });
                    }
                }
            }

            return departments;
        }
    }
}
