using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Models.Organizations
{
    [Table("Organizations")]
    public class Organizations
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SlugTenant { get; set; }
    }
}
