using Infraestructura.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IProductService
    {
        public Task Add(Products entity);
        public Task<IEnumerable<Products>> GetAll();
        public Task<Products> GetById(object id);
        public Task Remove(object id);
        public Task Update(Products products);


    }
}
