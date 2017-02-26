using FpCSharp7.Infrastructure;
using FpCSharp7Failure.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FpCSharp7.Http
{
    public class ProductController
    {
        
        public async Task<(int statusCode, string content)> AddNewProduct()
        { }
    }

    public class AddProductRequest
    {
        public string Name { get; set; }
    }

    public class Product
    {
        public Product(string name) => Name = name;
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public int Version { get; set; } = 0;
    }

    public class ProductResponse
    {
        public static ProductResponse FromProduct(Product p) => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Version = p.Version
        };
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
    public static class ProductModule
    {
        public static IResult<AddProductRequest, IFailure> Validate(this AddProductRequest addProductRequest) =>
             addProductRequest.Name.IsNullOrEmpty() ?
                Failure.ValidationFailureResult<AddProductRequest>("Name can't be empty")
           : addProductRequest.ToSuccess<AddProductRequest, IFailure>();

        public static Product MapToNewProduct(AddProductRequest addProductRequest)
            => new Product(addProductRequest.Name);

        public static IResult<Product, IFailure> SaveProductAsync(this Product product)
        {
            try
            {
                NonFunctionalDbWrapper.SaveAsync(product).Wait();
                return product.ToSuccess<Product, IFailure>();
            }
            catch (ConflictException ex)
            {
                return Failure.ConflictFailureResult(ex.GetReloadedModel<Product>());
            }
        }
    }
    public class ConflictException : Exception
    {
        private object _reloadedModel;
        public ConflictException(object reloadedModel) => _reloadedModel = reloadedModel;
        public T GetReloadedModel<T>() => (T)_reloadedModel;
    }

    public static class NonFunctionalDbWrapper
    {
        private static int _nextId;
        private static IDictionary<int, Product> _storage = new ConcurrentDictionary<int, Product>();
        public static async Task SaveAsync(Product product)
        {
            await Task.Delay(200);

            var productWithSameName = _storage.Values.FirstOrDefault(p => p.Name == product.Name);
            if (productWithSameName != null)
                throw new ConflictException(productWithSameName);
            product.Id = Interlocked.Increment(ref _nextId);
            _storage.Add(product.Id, product);
        }
    }

    public static class ProductComposition
    {
        public static IResult<ProductResponse, IFailure> AddProductComposition(AddProductRequest addProductRequest) 
            => from validatedRequest in addProductRequest.Validate()
               from product in ProductModule.MapToNewProduct(validatedRequest).ToSuccess<Product, IFailure>()
               from savedProduct in ProductModule.SaveProductAsync(product)
               from response in ProductResponse.FromProduct(savedProduct).ToSuccess<ProductResponse, IFailure>()
               select response;
    }
}
