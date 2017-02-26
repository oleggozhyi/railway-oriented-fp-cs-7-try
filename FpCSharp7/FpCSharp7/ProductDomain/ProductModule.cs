using FpCSharp7.Http.Dto;
using FpCSharp7.Infrastructure;
using FpCSharp7Failure.Infrastructure;

namespace FpCSharp7.ProductDomain
{
    public static class ProductModule
    {

        public static IResult<AddProductRequest, IFailure> Validate(this AddProductRequest addProductRequest) =>
             addProductRequest.Name.IsNullOrEmpty() ?
                Failure.ValidationFailureResult<AddProductRequest>("Name can't be empty")
           : addProductRequest.ToSuccess<AddProductRequest, IFailure>();

        public static Product MapToNewProduct(AddProductRequest addProductRequest)
            => new Product(addProductRequest.Name);

        public static IFailure MapConflictResponse(IFailure failure) =>
            failure is ConflictFailure<Product> conflict ?
                Failure.Conflict(conflict.ReloadedModel.MapToProductResponse())
          : failure;

        public static IResult<ProductResponse, IFailure> MapResponseOrConflict(IResult<Product, IFailure> result)
              => result.MapSuccess(MapToProductResponse).MapFailure(MapConflictResponse);

        public static ProductResponse MapToProductResponse(this Product p) => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Version = p.Version
        };
    }
}
