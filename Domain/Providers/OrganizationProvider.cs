using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Providers
{
    public class OrganizationProvider : IOrganizationProvider
    {

        // Para aplicar migraciones debe setear el connection string como un valor por defecto :( 
        public string OrganizationConnectionString { get; set; }
        public OrganizationProvider()
        {
            
        }

        public string getConnectionString()
        {
            return this.OrganizationConnectionString;
        }

        public void setConnection(string connectionString)
        {
            this.OrganizationConnectionString = connectionString;
        }
    }
}
