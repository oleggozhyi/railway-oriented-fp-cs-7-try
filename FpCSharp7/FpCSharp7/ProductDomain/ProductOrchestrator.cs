using FpCSharp7.Http.Dto;
using FpCSharp7Failure.Infrastructure;
using System;
using System.Linq;
using static FpCSharp7.DataAccess.ProductRepo;
using static FpCSharp7.ProductDomain.ProductModule;

namespace FpCSharp7.ProductDomain
{
    public static class ProductOrchestrator
    {
        public static IResult<ProductResponse, IFailure> AddProductComposition(AddProductRequest addProductRequest)
            => from validatedRequest in addProductRequest.Validate()
               from product in MapToNewProduct(validatedRequest).ToSuccess<Product, IFailure>()
               let savedProduct = SaveProductAsync(product)
               from response in MapResponseOrConflict(savedProduct)
               select response;

        public static IResult<ProductResponse, IFailure> GetProductComposition(GetProductRequest getProductRequest)
            => throw new NotImplementedException();
    }
}
