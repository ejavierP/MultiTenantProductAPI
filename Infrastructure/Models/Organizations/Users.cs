using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Models.Organizations
{
    [Table("Users")]
    public class Users: AuditInfo
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("OrganizationId")]
        public Organizations Organizations { get; set; }
        public Guid OrganizationId { get; set; }

    }
}
