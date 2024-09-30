using E_Commerce.Domain.IRepositories;
using E_Commerce.Persistence.Data;
using E_Commerce.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Application.IoC;
using E_Commerce.Persistence.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configurar el DbContext para MySQL u Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var databaseType = builder.Configuration["DatabaseType"];
    Console.WriteLine($"DatabaseType seleccionado: {databaseType}");

    if (databaseType == "MySQL")
    {
        options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"),
                         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection")));
    }
    else if (databaseType == "Oracle")
    {
        options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"));
    }
    else if (databaseType == "InMemory")
    {
        options.UseInMemoryDatabase("TestDb");
    }
    else
    {
        throw new Exception($"Tipo de base de datos no soportado: {databaseType}");
    }
});

// Registrar fábricas de repositorios usando el patrón Abstract Factory
builder.Services.AddScoped<IRepositoryFactory>(serviceProvider =>
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var databaseType = builder.Configuration["DatabaseType"];

    if (databaseType == "MySQL")
    {
        return new MySqlRepositoryFactory(context);
    }
    else if (databaseType == "Oracle")
    {
        return new OracleRepositoryFactory(context);
    }
    else if (databaseType == "InMemory")
    {
        return new MySqlRepositoryFactory(context); 
    }

    throw new Exception($"Tipo de base de datos no soportado: {databaseType}");
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddServices();
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

// Inicializar los datos de prueba
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
