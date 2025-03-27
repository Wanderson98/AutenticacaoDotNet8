using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryProduct _repositoryProduct;

        public ProductsController(IRepositoryProduct repositoryProduct)
        {
            _repositoryProduct = repositoryProduct;
        }


        [HttpGet("/api/GetAllProducts")]
        [Produces("application/json")]
        public async Task<object> GetAllProducts()
        {
            try
            {
                var result = await _repositoryProduct.ListAll();

                if (result?.Count == 0) return NoContent();
                
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to list products");
            }
        }

        [HttpPost("/api/CreateProduct")]
        [Produces("application/json")]
        public async Task<object> CreateProduct(ProductModel product)
        {
            try
            {
                await _repositoryProduct.Add(product);

            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to create product");
            }

            return Task.FromResult("Ok");

        }
        [HttpPut("/api/UpdateProduct")]
        [Produces("application/json")]
        public async Task<object> UpdateProduct(ProductModel product)
        {
            try
            {
                await _repositoryProduct.Update(product);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to update product");
            }

            return Task.FromResult("Ok");

        }

        [HttpGet("/api/GetProductById")]
        [Produces("application/json")]
        public async Task<object> GetProductById(int id)
        {
            try
            {
                var result = await _repositoryProduct.GetProductById(id);

                if (result is null) return NoContent();

                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to search products");
            }
        }

        [HttpDelete("/api/DeleteProduct")]
        [Produces("application/json")]
        public async Task<object> DeleteProduct(int id)
        {
            try
            {
                var product = await _repositoryProduct.GetProductById(id);
                await _repositoryProduct.Delete(product);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error to delete product");
            }
            return true;
        }
    }
}
