using GudelIdService.Domain.Services;
using System;

namespace GudelIdService.Implementation.Services
{
    public class ConverterServiceFactory<T> : IConverterServiceFactory<T>
    {
        public IConverterService<T> GetConverter()
        {
            return new ConverterService<T>();
        }
    }


    public class ConverterService<T> : IConverterService<T>
    {
        private T _defaultValue;
        private Func<T> _converter;

        public IConverterService<T> WithDefaultValue(T value)
        {
            _defaultValue = value;
            return this;
        }

        public IConverterService<T> WithConvertAction(Func<T> converter)
        {
            _converter = converter;
            return this;
        }

        public T Execute()
        {
            if (_converter is null) throw new ArgumentNullException();
            if (_defaultValue is null) throw new ArgumentNullException();

            try
            {
                return _converter.Invoke();
            }
            catch (Exception)
            {
                return _defaultValue;
            }
            finally
            {
                _converter = null;
                _defaultValue = default;
            }
        }
    }
}