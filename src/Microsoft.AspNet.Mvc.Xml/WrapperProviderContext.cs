using System;

namespace Microsoft.AspNet.Mvc.Xml
{
    public class WrapperProviderContext
    {
        public WrapperProviderContext([NotNull] Type declaredType, bool isSerialization)
        {
            DeclaredType = declaredType;
            IsSerialization = isSerialization;
        }

        /// <summary>
        /// The declared type which could be wrapped/un-wrapped by a different type 
        /// during serialization or de-serializatoin.
        /// </summary>
        public Type DeclaredType { get; }

        /// <summary>
        /// <see langword="true"/> if a wrapper provider is invoked during serialization,
        /// <see langword="false"/> otherwise.
        /// </summary>
        public bool IsSerialization { get; }
    }
}