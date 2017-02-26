using System;

namespace FpCSharp7.DataAccess
{
    public class ConflictException : Exception
    {
        private object _reloadedModel;
        public ConflictException(object reloadedModel) => _reloadedModel = reloadedModel;
        public T GetReloadedModel<T>() => (T)_reloadedModel;
    }
}
