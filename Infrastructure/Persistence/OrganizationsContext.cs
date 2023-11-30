using Infraestructura.Models.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Persistence
{
    public class OrganizationsContext : DbContext
    {
        public OrganizationsContext(DbContextOptions<OrganizationsContext> options) : base(options)
        {

        }

        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
