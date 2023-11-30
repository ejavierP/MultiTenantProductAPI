using Aplicacion.Interfaces;
using Aplicacion.Services;
using Dominio.Configurations;
using Dominio.Interfaces.Common;
using Infraestructura.Persistence;
using Infraestructura.Repositories.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(options =>
          {
              var jwtOptions = new JWTOptions
              {
                  Audience = configuration.GetValue<string>("JWTOptions:Audience"),
                  Issuer = configuration.GetValue<string>("JWTOptions:Issuer"),
                  Secret = configuration.GetValue<string>("JWTOptions:Secret")
              };

              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = jwtOptions.Issuer,
                  ValidAudience = jwtOptions.Audience,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
              };
          });


builder.Services.AddDbContext<OrganizationsContext>(options => options.UseSqlServer(connectionStringOptions.OrganizacionDB, b => b.MigrationsAssembly("Infraestructura")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenService, TokenService>();

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
