using E_Commerce.Domain.IRepositories;
using E_Commerce.Persistence.Data;
using E_Commerce.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar el DbContext para MySQL u Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var databaseType = builder.Configuration["DatabaseType"];
    if (databaseType == "MySQL")
    {
        options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection")));
    }
    else if (databaseType == "Oracle")
    {
        options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"));
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

    throw new Exception("Tipo de base de datos no soportado");
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
