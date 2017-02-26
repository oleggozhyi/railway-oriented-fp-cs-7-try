using FpCSharp7Failure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpCSharp7
{
    public interface IFailure
    {
    }

    public static class Failure
    {
        public static IResult<T, IFailure> ValidationFailureResult<T>(string message)
            => Validation(message).ToFailureResult<T, IFailure>();
        public static IResult<T, IFailure> DataNotFoundFailureResult<T>(string data, string predicate)
            => DataNotFound(data, predicate).ToFailureResult<T, IFailure>();
        public static IResult<TModel, IFailure> ConflictFailureResult<TModel>(TModel reloadedModel) 
            => Conflict<TModel>(reloadedModel).ToFailureResult<TModel, IFailure>();

        public static IFailure Validation(string message) => new ValidationFailure(message);
        public static IFailure DataNotFound(string data, string predicate) => new DataNotFoundFailure(data, predicate);
        public static IFailure Conflict<TModel>(TModel reloadedModel) => new ConflictFailure<TModel>(reloadedModel);
    }

    public class ValidationFailure : IFailure
    {
        public ValidationFailure(string message) => ValidationMessage = message;
        public string ValidationMessage { get; }
    }
    public class DataNotFoundFailure : IFailure
    {
        public DataNotFoundFailure(string data, string predicate)
        {
            Predicate = predicate;
            Data = data;
        }
        public string Predicate { get; }
        public string Data { get; }
    }
    public class ConflictFailure<TModel> : IFailure
    {
        public ConflictFailure(TModel reloadedModel) => ReloadedModel = reloadedModel;
        public TModel ReloadedModel { get; }
    }
}
