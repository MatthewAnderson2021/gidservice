using System;

namespace GudelIdService.Domain.Services
{
    public interface IConverterService<T>
    {
        IConverterService<T> WithDefaultValue(T value);

        IConverterService<T> WithConvertAction(Func<T> converter);

        T Execute();
    }

    public interface IConverterServiceFactory<T>
    {
        IConverterService<T> GetConverter();

    }
}