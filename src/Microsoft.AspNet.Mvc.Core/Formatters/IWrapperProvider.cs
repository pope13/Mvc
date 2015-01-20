using System;

namespace Microsoft.AspNet.Mvc
{
    public interface IWrapperProvider
    {
        bool TryGetWrappingType(Type originalType, out Type wrappingType);

        object Wrap(object obj);

        object Unwrap(object obj);
    }
}