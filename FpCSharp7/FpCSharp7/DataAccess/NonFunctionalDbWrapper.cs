using FpCSharp7.ProductDomain;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FpCSharp7.DataAccess
{
    public static class NonFunctionalDbWrapper
    {
        private static int _nextId;
        private static IDictionary<int, Product> _storage = new ConcurrentDictionary<int, Product>();
        public static async Task SaveAsync(Product product)
        {
            await Task.Delay(200); // emulate db call latency

            var productWithSameName = _storage.Values.FirstOrDefault(p => p.Name == product.Name);
            if (productWithSameName != null)
                throw new ConflictException(productWithSameName);
            product.Id = Interlocked.Increment(ref _nextId);
            _storage.Add(product.Id, product);
        }

        public static async Task<Product> GetProduct(int productId)
        {
            await Task.Delay(200); // emulate db call latency
            _storage.TryGetValue(productId, out Product product);
            return product;
        }
    }
}
