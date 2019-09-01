using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Unity;

namespace Products.Controllers
{
    
    public class ProductsController : BaseController
    {
        public ProductsController(IUnityContainer container) : base(container)
        {
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                return await DataProvider.ReadAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Error executing ${nameof(DataProvider.CreateAsync)}: ${e.GetBaseException()}");
            }
            
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<JsonResult> CreateProductAsync([FromBody] Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"{nameof(product)} was null");

            try
            {
                var id = await DataProvider.CreateAsync(product);
                return Json(id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error executing ${nameof(DataProvider.CreateAsync)}: ${e.GetBaseException()}");
            }

        }

        [HttpPut]
        [Route("api/[controller]")]
        public async Task UpdateProductAsync([FromBody] Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"{nameof(product)} was null");

            try
            {
                await DataProvider.UpdateAsync(product);
            }
            catch (Exception e)
            {
                throw new Exception($"Error executing ${nameof(DataProvider.UpdateAsync)}: ${e.GetBaseException()}");
            }
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public async Task DeleteProductAsync([FromRoute] int id)
        {
            if (id == default)
                throw new ArgumentNullException($"invalid {nameof(id)} provided");

            try
            {
                await DataProvider.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error executing ${nameof(DataProvider.DeleteAsync)}: ${e.GetBaseException()}");
            }
            
        }

        [HttpPost]
        [Route("api/[controller]/validate")]
        public async Task<JsonResult> ValidateProductAsync([FromBody] Product product)
        {
            if (product == null)
                throw new ArgumentNullException($"{nameof(product)} was null");

            try
            {
                var is_valid = await DataProvider.ValidateAsync(product);
                return Json(is_valid);
            }
            catch (Exception e)
            {
                throw new Exception($"Error executing ${nameof(DataProvider.ValidateAsync)}: ${e.GetBaseException()}");
            }
        }
    }
}
