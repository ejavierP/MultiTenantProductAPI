using Aplicacion.Interfaces;
using Dominio.DTOS;
using Dominio.DTOS.Organization;
using Dominio.DTOS.Products;
using Infraestructura.Models.Products;
using Infraestructura.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) 
        {
            _productService = productService;
        }
        [HttpGet("{slugTenant}")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{slugTenant}/{productId}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            try
            {
                var products = await _productService.GetById(productId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{slugTenant}")]
        public async Task<IActionResult> CreateProduct(CreateOrUpdateProductDTO productRequest)
        {
            try
            {
                await _productService.Add(new Products
                {
                    Description = productRequest.Description,
                    Name = productRequest.Name,
                    Price = productRequest.Price
                });
                return Ok(productRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{slugTenant}")]

        public async Task<IActionResult> UpdateProduct(CreateOrUpdateProductDTO productRequest)
        {
            try
            {
                await _productService.Update(new Products
                {
                    Description = productRequest.Description,
                    Name = productRequest.Name,
                    Price = productRequest.Price
                });
                return Ok(productRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{slugTenant}/{productId}")]

        public async Task<IActionResult> Remove(Guid productId)
        {
            try
            {
                await _productService.Remove(productId);
                return Ok("Product removed succefully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
