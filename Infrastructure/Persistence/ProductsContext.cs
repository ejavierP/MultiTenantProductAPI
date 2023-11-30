using Dominio.Interfaces;
using Dominio.Configurations;
using Dominio.Interfaces;
using Infraestructura.Models.Organizations;
using Infraestructura.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Dominio.Providers;

namespace Infraestructura.Persistence
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
           
        }

       

        public DbSet<Products> Products { get; set; }
    }
}
