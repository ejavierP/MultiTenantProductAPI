using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Models.Products
{
    public class Products
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }


    }
}
