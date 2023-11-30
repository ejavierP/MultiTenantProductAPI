using API.Middlewares;
using Aplicacion.Interfaces;
using Aplicacion.Services;
using Dominio.Configurations;
using Dominio.Interfaces;
using Dominio.Interfaces.Common;
using Dominio.Providers;
using Infraestructura.Persistence;
using Infraestructura.Repositories.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionStringOptions = new ConnectionStringOptions
{
    OrganizacionDB = configuration.GetValue<string>("ConnectionStrings:OrganizacionDB")
};

builder.Services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));

builder.Services.Configure<TenantOptions>(configuration.GetSection("Tenants"));

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

builder.Services.AddDbContext<ProductsContext>((s, o) =>
{
    var organizationService = s.GetService<IOrganizationProvider>();
   
    var connectionString = organizationService?.getConnectionString() ?? "";
    // multi-tenant databases
    o.UseSqlServer(connectionString);
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(typeof(IRepositoryOrganizations<>), typeof(RepositoryGenericOrganization<>));
builder.Services.AddScoped(typeof(IRepositoryProducts<>), typeof(RepositoryGenericProducts<>));
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IOrganizationProvider, OrganizationProvider>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddTransient<IProductService, ProductService>();

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

app.UseWhen(context => context.Request.Path.StartsWithSegments("/Products"), appBuilder =>
{
    appBuilder.UseMiddleware<OrganizationConnectionMiddleware>();
});

app.MapControllers();

app.Run();
