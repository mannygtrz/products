using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace Products
{
    public class MockDataProvider : IDataProvider
    {
        private string _productsJsonFile;

        public MockDataProvider()
        {
            _productsJsonFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "products.json");
        }

        // TODO: add validation and error handling
        public async Task<int> CreateAsync(Product product)
        {
            var products_json = File.ReadAllText(_productsJsonFile);
            var products = JsonConvert.DeserializeObject<List<Product>>(products_json);
            if (products.Count() == 0)
            {
                product.ID = 1;
                products_json = JsonConvert.SerializeObject(new Product[] { product }, Formatting.Indented);
                File.WriteAllText(_productsJsonFile, products_json);
                return await Task.FromResult(1);
            }

            
            product.ID = products.Max(m => m.ID) + 1;

            products.Add(product);

            File.WriteAllText(_productsJsonFile, JsonConvert.SerializeObject(products, Formatting.Indented));

            return await Task.FromResult(product.ID);
        }

        // TODO: add validation and error handling
        public async Task DeleteAsync(int id)
        {
            var products_json = File.ReadAllText(_productsJsonFile);

            var products = JsonConvert.DeserializeObject<List<Product>>(products_json);

            var index = products.FindIndex(p => p.ID == id);
            products.RemoveAt(index);

            File.WriteAllText(_productsJsonFile, JsonConvert.SerializeObject(products, Formatting.Indented));

            await Task.CompletedTask;
        }

        // TODO: add validation and error handling
        public async Task<IEnumerable<Product>> ReadAsync()
        {
            var products_json = File.ReadAllText(_productsJsonFile);
            var products = JsonConvert.DeserializeObject<Product[]>(products_json);
            return await Task.FromResult(products);
        }

        // TODO: add validation and error handling
        public async Task UpdateAsync(Product product)
        {
            var products_json = File.ReadAllText(_productsJsonFile);

            var products = JsonConvert.DeserializeObject<List<Product>>(products_json);

            var index = products.FindIndex(p => p.ID == product.ID);
            products[index] = product;

            File.WriteAllText(_productsJsonFile, JsonConvert.SerializeObject(products, Formatting.Indented));

            await Task.CompletedTask;
        }

        // TODO: add validation and error handling
        public async Task<bool> ValidateAsync(Product product)
        {
            var products_json = File.ReadAllText(_productsJsonFile);

            var products = JsonConvert.DeserializeObject<List<Product>>(products_json);

            var result = products
                .FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower() && p.ID != product.ID);

            if (products.Count() != 0 && result != null)
                return await Task.FromResult(false);

            return await Task.FromResult(true);
        }
    }
}
