using Newtonsoft.Json.Linq;
using Products;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProductsTest
{
    public class MockDataProviderTests
    {
        [Fact]
        public async Task ReadAsync_Test()
        {
            // arrange
            var data_provider = new MockDataProvider();

            // act
            var products = await data_provider.ReadAsync();

            // assert 
            Assert.NotNull(products);
            Assert.Single(products);
        }

        [Fact]
        public async Task CreateAsync_Test()
        {
            // arrange
            var data_provider = new MockDataProvider();
            var product = new Product
            {
                Name = "Other product",
                Description = "This is a cool product as well",
                Quantity = 100
            };

            // act
            var new_id = await data_provider.CreateAsync(product);

            // assert 
            Assert.NotEqual(0, new_id);
        }

        [Fact]
        public async Task UpdateAsync_Test()
        {
            // arrange
            var data_provider = new MockDataProvider();
            var product = new Product
            {
                ID = 1,
                Name = "Other product",
                Description = "This is a cool product as well",
                Quantity = 100
            };

            // act
            var task = data_provider.UpdateAsync(product);
            await task;

            // assert 
            Assert.True(task.Status == TaskStatus.RanToCompletion);
        }

        [Fact]
        public async Task DeleteAsync_Test()
        {
            // arrange
            var data_provider = new MockDataProvider();

            // act
            var task = data_provider.DeleteAsync(1);
            await task;

            // assert 
            Assert.True(task.Status == TaskStatus.RanToCompletion);
        }

        [Fact]
        public async Task ValidateAsync_Test()
        {
            // arrange
            var data_provider = new MockDataProvider();

            var product = new Product
            {
                ID = 2,
                Name = "Product A",
                Description = "This is a cool product as well",
                Quantity = 100
            };

            // act
            var is_valid = await data_provider.ValidateAsync(product);

            // assert 
            Assert.False(is_valid);
        }
    }
}
