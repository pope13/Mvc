using System;
using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc.Xml
{
    public class EnumerableWrapperProvider : IWrapperProvider
    {
        private readonly WrapperProviderContext _context;
        private readonly Type _sourceIEnumerableGenericType;
        private readonly IEnumerable<IWrapperProviderFactory> _wrapperProviderFactories;

        public EnumerableWrapperProvider(
            Type sourceIEnumerableGenericType,
            IEnumerable<IWrapperProviderFactory> wrapperProviderFactories,
            WrapperProviderContext context)
        {
            _sourceIEnumerableGenericType = sourceIEnumerableGenericType;
            _wrapperProviderFactories = wrapperProviderFactories;
            _context = context;
        }

        public Type GetWrappingType([NotNull] Type declaredType)
        {
            IWrapperProvider elementWrapperProvider = null;
            return GetWrappingEnumerableType(out elementWrapperProvider);
        }

        public object Wrap(object original)
        {
            if (original != null)
            {
                IWrapperProvider elementWrapperProvider = null;
                var wrappingEnumerableType = GetWrappingEnumerableType(out elementWrapperProvider);

                var wrappingEnumerableTypeConstructor = wrappingEnumerableType.GetConstructor(new[]
                    {
                        _sourceIEnumerableGenericType,
                        typeof(IWrapperProvider)
                    });

                return wrappingEnumerableTypeConstructor.Invoke(new object[]
                    {
                        original,
                        elementWrapperProvider
                    });
            }

            return null;
        }

        private Type GetWrappingEnumerableType(out IWrapperProvider elementWrapperProvider)
        {
            var declaredElementType = _sourceIEnumerableGenericType.GetGenericArguments()[0];

            // Since the T itself could be wrapped, get the wrapping type for it
            var wrapperProviderContext = new WrapperProviderContext(declaredElementType, _context.IsSerialization);

            var wrappedOrDeclaredElementType = declaredElementType;

            elementWrapperProvider = null;
            foreach (var wrapperProviderFactory in _wrapperProviderFactories)
            {
                elementWrapperProvider = wrapperProviderFactory.GetProvider(wrapperProviderContext);
                if (elementWrapperProvider != null)
                {
                    break;
                }
            }

            if (elementWrapperProvider != null)
            {
                var wrappingType = elementWrapperProvider.GetWrappingType(wrapperProviderContext.DeclaredType);
                if (wrappingType != null)
                {
                    wrappedOrDeclaredElementType = wrappingType;
                }
            }

            return typeof(DelegatingEnumerable<,>).MakeGenericType(wrappedOrDeclaredElementType, declaredElementType);
        }
    }
}