using FpCSharp7.Http.Dto;
using FpCSharp7Failure.Infrastructure;
using System;

namespace FpCSharp7.Http
{
    public class ProductController
    {
        private Func<AddProductRequest, IResult<ProductResponse, IFailure>> _addProductImpl;
        private Func<GetProductRequest, IResult<ProductResponse, IFailure>> _getProductImpl;
        public ProductController(
            Func<AddProductRequest, IResult<ProductResponse, IFailure>> addProductImpl,
            Func<GetProductRequest, IResult<ProductResponse, IFailure>> getProductImpl)
        {
            _getProductImpl = getProductImpl;
            _addProductImpl = addProductImpl;
        }

        public (int statusCode, string content) AddNewProduct(AddProductRequest addProductRequest)
            => _addProductImpl(addProductRequest).ToHttpResponse();

        public (int statusCode, string content) GetProduct(GetProductRequest getProductRequest)
            => _getProductImpl(getProductRequest).ToHttpResponse();
    }
}
