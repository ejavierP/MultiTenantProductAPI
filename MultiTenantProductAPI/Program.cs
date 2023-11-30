using Dominio.Configurations;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionStringOptions = new ConnectionStringOptions
{
    OrganizacionDB = configuration.GetValue<string>("ConnectionStrings:OrganizacionDB")
};


builder.Services.AddDbContext<OrganizationsContext>(options => options.UseSqlServer(connectionStringOptions.OrganizacionDB, b => b.MigrationsAssembly("Infraestructura")));

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
