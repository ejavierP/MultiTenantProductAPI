using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Configurations
{
    public  class JWTOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
        public int ExpiryHours { get; set; }
    }
}
