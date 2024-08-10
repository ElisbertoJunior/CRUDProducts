using MySql.Data.MySqlClient;


namespace ProductAPI.Database
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(string connectionString, ILogger<DatabaseInitializer> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    //Cria banco de dados caso não exista
                    var criateDatabaseQuery = "CREATE DATABASE IF NOT EXISTS ECommerceDB;";
                    using (var command = new MySqlCommand(criateDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.ChangeDatabase("ECommerceDB");

                    // Cria a tabela Departament caso não exista
                    var creatDepartmentTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Department (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Code VARCHAR(50) NOT NULL,
                            Description TEXT NOT NULL                               
                        );    
                    ";
                    using (var command = new MySqlCommand(creatDepartmentTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insere dados iniciais na tabela Department
                    var insertDepartmentsQuery = @"
                        INSERT INTO Department (Code, Description)
                        SELECT '010', 'BEBIDAS' WHERE NOT EXISTS (SELECT 1 FROM Department WHERE Code = '010');
                        
                        INSERT INTO Department (Code, Description)
                        SELECT '020', 'CONGELADOS' WHERE NOT EXISTS (SELECT 1 FROM Department WHERE Code = '020');
                        
                        INSERT INTO Department (Code, Description)
                        SELECT '030', 'LATICINIOS' WHERE NOT EXISTS (SELECT 1 FROM Department WHERE Code = '030');
                        
                        INSERT INTO Department (Code, Description)
                        SELECT '040', 'VEGETAIS' WHERE NOT EXISTS (SELECT 1 FROM Department WHERE Code = '040');
                    ";
                    using (var command = new MySqlCommand(insertDepartmentsQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }


                    // Cria a tabela Product com chave estrangeira para o Department
                    var createProductTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Product (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Code VARCHAR(50) NOT NULL,
                            Description TEXT NOT NULL,
                            DepartmentId INT NOT NULL,
                            Price DECIMAL (18, 2) NOT NULL,
                            Status BOOLEAN NOT NULL DEFAULT true,
                            IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,
                            FOREIGN KEY (DepartmentId) REFERENCES Department(Id) ON DELETE CASCADE
                        );
                    ";
                    using (var command = new MySqlCommand(createProductTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Inserir produtos iniciais na tabela Product
                    var insertProductsQuery = @"
                    INSERT INTO Product (Code, Description, DepartmentId, Price, Status, IsDeleted)
                    SELECT '001', 'Coca-Cola', 1, 5.00, true, false WHERE NOT EXISTS (SELECT 1 FROM Product WHERE Code = '001');

                    INSERT INTO Product (Code, Description, DepartmentId, Price, Status, IsDeleted)
                    SELECT '002', 'Sorvete', 2, 10.00, true, false WHERE NOT EXISTS (SELECT 1 FROM Product WHERE Code = '002');

                    INSERT INTO Product (Code, Description, DepartmentId, Price, Status, IsDeleted)
                    SELECT '003', 'Queijo', 3, 15.00, true, false WHERE NOT EXISTS (SELECT 1 FROM Product WHERE Code = '003');

                    INSERT INTO Product (Code, Description, DepartmentId, Price, Status, IsDeleted)
                    SELECT '004', 'Alface', 4, 2.00, true, false WHERE NOT EXISTS (SELECT 1 FROM Product WHERE Code = '004');
";
                    using (var command = new MySqlCommand(insertProductsQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    _logger.LogInformation("Banco de dados e tabelas criadas com sucesso.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao inicializar o banco de dados.");
            }
        }

    }
}
