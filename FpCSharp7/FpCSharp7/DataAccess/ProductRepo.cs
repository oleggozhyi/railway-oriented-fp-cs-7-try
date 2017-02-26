using FpCSharp7.ProductDomain;
using FpCSharp7Failure.Infrastructure;
using System;

namespace FpCSharp7.DataAccess
{
    public static class ProductRepo
    {
        public static IResult<Product, IFailure> SaveProductAsync(Product product)
        {
            try
            {
                NonFunctionalDbWrapper.SaveAsync(product).Wait();
                return product.ToSuccess<Product, IFailure>();
            }
            catch (AggregateException ex) when (ex.InnerException is ConflictException conflictEx)
            {
                return Failure.ConflictFailureResult(conflictEx.GetReloadedModel<Product>());
            }
        }

    }
}
