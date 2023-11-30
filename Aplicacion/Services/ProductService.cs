using Aplicacion.Interfaces;
using Dominio.Interfaces.Common;
using Infraestructura.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryProducts<Products> _productsRepository;
        public ProductService(IRepositoryProducts<Products> productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public async Task Add(Products entity)
        {
           await _productsRepository.Add(entity);
        }


        public async Task<IEnumerable<Products>>GetAll()
        {
            return await _productsRepository.GetAll();
        }

        public async Task<Products> GetById(object id)
        {
            return await _productsRepository.GetById(id);
        }

        public async Task Remove(Products products)
        {

            var productRemove = await this.GetById(products.Id);
            await _productsRepository.Remove(productRemove);
        }

        public async Task Update(Products products)
        {
            var productUpdated = await this.GetById(products.Id);
            await _productsRepository.Update(productUpdated);
        }
    }
}
