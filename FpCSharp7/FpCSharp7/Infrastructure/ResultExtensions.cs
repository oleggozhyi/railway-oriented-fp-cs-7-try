using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpCSharp7Failure.Infrastructure
{
    public static class Result
    {
        private static ArgumentOutOfRangeException OutOfRange() => new ArgumentOutOfRangeException();

        public static IResult<TValue, TError> ToSuccess<TValue, TError>(this TValue value) 
            => new Success<TValue, TError>(value);

        public static IResult<TValue, TError> ToFailureResult<TValue, TError>(this TError error) 
            => new Failure<TValue, TError>(error);

        public static IResult<TB, TError> Bind<TA, TB, TError>(this IResult<TA, TError> result,
                 Func<TA, IResult<TB, TError>> function) =>
            result is Success<TA, TError> succes ? function(succes.Value)
          : result is Failure<TA, TError> failure ? failure.Error.ToFailureResult<TB, TError>()
          : throw OutOfRange();

        public static IResult<TA, TE2> MapFailure<TA, TE1, TE2>(this IResult<TA, TE1> result,
                Func<TE1, TE2> function) =>
            result is Success<TA, TE1> succes ? succes.Value.ToSuccess<TA, TE2>()
          : result is Failure<TA, TE1> failure ? function(failure.Error).ToFailureResult<TA, TE2>()
          : throw OutOfRange();

    }

    public static class ResultLinqExtensions
    {
        public static IResult<TB, TError> SelectMany<TA, TB, TError>(this IResult<TA, TError> result,
            Func<TA, IResult<TB, TError>> function) => result.Bind(function);

        public static IResult<TC, TError> SelectMany<TA, TB, TC, TError>(this IResult<TA, TError> result,
                    Func<TA, IResult<TB, TError>> function, Func<TA, TB, TC> composer) =>
            result.Bind(v1 => function(v1).Select(v2 => composer(v1, v2)));

        public static IResult<TB, TError> Select<TA, TB, TError>(this IResult<TA, TError> result,
            Func<TA, TB> function) => result.Bind(x => function(x).ToSuccess<TB, TError>());
    }
}
