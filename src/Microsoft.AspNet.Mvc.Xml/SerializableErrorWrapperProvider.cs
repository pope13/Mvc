using System;
using Microsoft.AspNet.Mvc;

namespace Microsoft.AspNet.Mvc.Xml
{
    public class SerializableErrorWrapperProvider : IWrapperProvider
    {
        private readonly WrapperProviderContext _context;

        public SerializableErrorWrapperProvider([NotNull] WrapperProviderContext context)
        {
            _context = context;
        }

        public Type GetWrappingType([NotNull] Type declaredType)
        {
            if (declaredType == typeof(SerializableError))
            {
                return typeof(SerializableErrorWrapper);
            }

            return null;
        }

        public object Wrap(object original)
        {
            var error = original as SerializableError;
            if (error == null)
            {
                return original;
            }

            return new SerializableErrorWrapper(error);
        }
    }
}