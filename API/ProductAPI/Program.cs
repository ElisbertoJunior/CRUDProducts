
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ProductAPI.Database;
using ProductAPI.Repositories;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;
using Serilog;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registra a string de conexão
            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return configuration.GetConnectionString("DefaultConnection");
            });

            // Configurando Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/myapp-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ProductPolice",
                    builder =>
                    {
                        builder.AllowAnyHeader()
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

            // Injeção de dependência para os repositórios
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            // Injeção de dependência para os serviços
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            

            builder.Services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return configuration.GetConnectionString("DefaultConnection");
            });

            builder.Services.AddScoped<IProductRepository>(sp =>
            {
                var connectionString = sp.GetRequiredService<string>();
                return new ProductRepository(connectionString);
            });

            builder.Services.AddScoped<IDepartmentRepository>(sp =>
            {
                var connectionString = sp.GetRequiredService<string>();
                return new DepartmentRepository(connectionString);
            });



            /// Adiciona o DatabaseInitializer
            builder.Services.AddSingleton(sp =>
            {
                var connectionString = sp.GetRequiredService<string>();
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
                Log.Information("Handling request: " + context.Request.Method + " " + context.Request.Path);
                await next.Invoke();
                Log.Information("Finished handling request.");

            });

            // Presentation message after application is started
            Log.Information("****************************************");
            Log.Information("*    Aplicação iniciada com sucesso!   *");
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

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
                                           ILoggerFactory logger,
                                           UrlEncoder encoder,
                                           ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("ApiKey"))
            {
                return AuthenticateResult.Fail("API key is missing");
            }

            var apiKey = Request.Headers["ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return AuthenticateResult.Fail("API key is missing");
            }

            if (!IsApiKeyValid(apiKey))
            {
                return AuthenticateResult.Fail("Invalid API key");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "ApiKeyUser")
                // Aqui você pode adicionar outras claims conforme necessário
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private bool IsApiKeyValid(string apiKey)
        {
            if (apiKey != "Besouro")
            {
                return false;
            }

            return true;
        }
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKeyScheme";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
