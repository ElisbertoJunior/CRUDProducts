
using Microsoft.OpenApi.Models;
using ProductAPI.Database;
using ProductAPI.Repositories;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;
using Serilog;
using ProductAPI.Auth;


namespace ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            // Configurando Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/myapp-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ProductPolice",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Beaerer scheme (Example: '12345abcdef')",
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products.api", Version = "v1" });
            });

            // Inje��o de depend�ncia para os reposit�rios
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            // Inje��o de depend�ncia para os servi�os
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

           
            builder.Services.AddAuthentication("ApiKeyScheme")
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", option => {});

            // Registra a string de conexao
            /* 
            Tive que usar duas strings de conexões diferentes, pois um, pois minha intenção 
            era que assim que a aplicação iniciar ela crie dinamicamente o banco e as tabelas caso não
            exista porem com apenas uma string de conexão até funcionava, mas em algum momento depois de 2 ou 4 requisições
            por algum motivo a aplicação rachava por não encontrar o banco, então consegui contornar esse problema 
            criando uma string de conexão para iniciar a aplicação e outra para as requisições.  
            */
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return configuration.GetConnectionString("DefaultConnection");
            });


            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return configuration.GetConnectionString("ConnectionRequests");
            });

            
            builder.Services.AddScoped<IProductRepository>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("ConnectionRequests");
                return new ProductRepository(connectionString);
            }); 

            builder.Services.AddScoped<IDepartmentRepository>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("ConnectionRequests");
                return new DepartmentRepository(connectionString);
            });


            /// Adiciona o DatabaseInitializer
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var logger = sp.GetRequiredService<ILogger<DatabaseInitializer>>();
                return new DatabaseInitializer(connectionString, logger);
            });

            


            var app = builder.Build();

           
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                Log.Information("Tratamento de solicitaçao: " + context.Request.Method + " " + context.Request.Path);
                await next.Invoke();
                Log.Information("Solicitação de tratamento concluida.");

            });

            // Mensagem de apresenta��o ap�s in�ciar a aplica��o
            Log.Information("****************************************");
            Log.Information("*    Aplica��o iniciada com sucesso!   *");
            Log.Information("****************************************");

            try
            {
                var dbInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
                dbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao inicializar o banco de dados.");
            }


            app.UseCors("ProductPolice");

            app.MapControllers();

            app.Run();
        }
    }

   
}
