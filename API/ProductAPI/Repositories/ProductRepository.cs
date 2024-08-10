using MySql.Data.MySqlClient;
using ProductAPI.Models;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Realiza um JOIN para buscar os produtos junto com seus departamentos
                var query = @"
                SELECT p.Id AS ProductId, p.Code AS ProductCode, p.Description AS ProductDescription, 
                p.Price, p.Status, p.IsDeleted,
                d.Id AS DepartmentId, d.Code AS DepartmentCode, d.Description AS DepartmentDescription
                FROM Product p
                INNER JOIN Department d ON p.DepartmentId = d.Id
                WHERE p.IsDeleted = FALSE";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            Id = reader.GetInt32("ProductId"),
                            Code = reader.GetString("ProductCode"),
                            Description = reader.GetString("ProductDescription"),
                            Price = reader.GetDecimal("Price"),
                            Status = reader.GetBoolean("Status"),
                            IsDeleted = reader.GetBoolean("IsDeleted"),

                            // Mapeia o departamento relacionado
                            Department = new Department
                            {
                                Id = reader.GetInt32("DepartmentId"),
                                Code = reader.GetString("DepartmentCode"),
                                Description = reader.GetString("DepartmentDescription")
                            }
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }


        public Product GetById(int id)
        {
            Product product = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Product WHERE Id = @Id AND IsDeleted = FALSE";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32("Id"),
                                Code = reader.GetString("Code"),
                                Description = reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                Status = reader.GetBoolean("Status"),
                                DepartmentId = reader.GetInt32("DepartmentId"),
                                IsDeleted = reader.GetBoolean("IsDeleted"),
                            };
                        }
                    }
                }
            }

          

            return product;
        }

        public void Add(Product product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"INSERT INTO Product (Code, Description, Price, Status, IsDeleted, DepartmentId)
                            VALUES (@Code, @Description, @Price, @Status, FALSE, @DepartmentId);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", product.Code);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Status", product.Status);
                    command.Parameters.AddWithValue("@DepartmentId", product.DepartmentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Product product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"UPDATE Product SET Code = @Code, Description = @Description, Price = @Price, Status = @Status, DepartmentId = @DepartmentId
                                WHERE Id = @Id AND IsDeleted = FALSE";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", product.Code);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Status", product.Status);
                    command.Parameters.AddWithValue("@DepartmentId", product.DepartmentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var query = "UPDATE Product SET IsDeleted = TRUE WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
