using System;
using Microsoft.AspNet.Mvc;

namespace Microsoft.AspNet.Mvc.Xml
{
    public class SerializableErrorWrapperProviderFactory : IWrapperProviderFactory
    {
        public IWrapperProvider GetProvider([NotNull] WrapperProviderContext context)
        {
            if (context.DeclaredType == typeof(SerializableError))
            {
                return new SerializableErrorWrapperProvider(context);
            }

            return null;
        }
    }
}