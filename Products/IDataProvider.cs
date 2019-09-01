using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products
{
    public interface IDataProvider
    {
        Task<int> CreateAsync(Product product);
        Task<IEnumerable<Product>> ReadAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ValidateAsync(Product product);
    }
}
