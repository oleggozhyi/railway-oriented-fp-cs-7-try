using FpCSharp7Failure.Infrastructure;
using Newtonsoft.Json;

namespace FpCSharp7.Http
{
    public static class HttpResponseHelper
    {
        public static string Serialize<T>(this T o) => JsonConvert.SerializeObject(o);
        public static (int statusCode, string content) ToHttpResponse<TResult>(this IResult<TResult, IFailure> result)
            => result is Success<TResult, IFailure> success
                    ? (200, success.Serialize())
             : result is Failure<TResult, IFailure> failure
                  ? failure.Error is ConflictFailure<TResult> conflict
                            ? (409, conflict.ReloadedModel.Serialize())
                  : failure.Error is DataNotFoundFailure dataNotFound
                            ? (404, $"{dataNotFound.Data} not found by {dataNotFound.Predicate}")
                  : failure.Error is ValidationFailure validation
                            ? (400, validation.ValidationMessage)
                  : (500, "Unknown failure : " + failure.Serialize())
            : throw Result.OutOfRange();
    }
}
