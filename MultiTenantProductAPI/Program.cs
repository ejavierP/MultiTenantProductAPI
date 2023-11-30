using Aplicacion.Interfaces;
using Aplicacion.Services;
using Dominio.Configurations;
using Dominio.Interfaces.Common;
using Infraestructura.Persistence;
using Infraestructura.Repositories.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT",
        Type = SecuritySchemeType.Http,
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }
            },
            new List<string>()
        }
    });

});



var configuration = builder.Configuration;

configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionStringOptions = new ConnectionStringOptions
{
    OrganizacionDB = configuration.GetValue<string>("ConnectionStrings:OrganizacionDB")
};

builder.Services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));

var test = new JWTOptions
{
    Audience = configuration.GetValue<string>("JWTOptions:Audience"),
    Issuer = configuration.GetValue<string>("JWTOptions:Issuer"),
    Secret = configuration.GetValue<string>("JWTOptions:Secret")
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.RequireHttpsMetadata = false;


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

builder.Services.AddAuthorization();

builder.Services.AddDbContext<OrganizationsContext>(options => options.UseSqlServer(connectionStringOptions.OrganizacionDB, b => b.MigrationsAssembly("Infraestructura")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MULTI-TENANT-PRODUCT-API");
});


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
