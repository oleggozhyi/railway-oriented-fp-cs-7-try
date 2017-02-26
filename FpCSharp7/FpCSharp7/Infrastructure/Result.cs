namespace FpCSharp7Failure.Infrastructure
{
    public interface IResult<TValue, TError>
    {
    }
    public class Success<TValue, TError> : IResult<TValue, TError>
    {
        public Success(TValue value) => Value = value;
        public TValue Value { get; }
    }
    public class Failure<TValue, TError> : IResult<TValue, TError>
    {
        public Failure(TError error) => Error = error;
        public TError Error { get; }
    }
}
