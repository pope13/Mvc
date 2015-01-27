using System;
using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc.Xml
{
    /// <summary>
    /// Defines an interface for getting list of wrapper provider factories.
    /// </summary>
    public interface IWrapperProviderFactoryProvider
    {
        IList<IWrapperProviderFactory> WrapperProviderFactories { get; }
    }
}