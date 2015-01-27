using System;

namespace Microsoft.AspNet.Mvc.Xml
{
    /// <summary>
    /// Defines an interface for wrapping objects for serialization or deserialization into xml.
    /// </summary>
    public interface IWrapperProvider
    {
        /// <summary>
        /// Gets the type which wraps the given type.
        /// </summary>
        /// <param name="declaredType">The declared type which needs to be wrapped.</param>
        /// <returns>The wrapping type if the provider decides to wrap the given type, else null.</returns>
        Type GetWrappingType(Type declaredType);

        /// <summary>
        /// Wraps the given object to the wrapping type provided by <see cref="GetWrappingType(Type)"/>.
        /// </summary>
        /// <param name="original">The original non-wrapped object</param>
        /// <returns></returns>
        object Wrap(object original);
    }
}