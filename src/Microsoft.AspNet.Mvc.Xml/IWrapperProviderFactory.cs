﻿using System;
using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc.Xml
{
    /// <summary>
    /// Defines an interface to create wrapper providers.
    /// </summary>
    public interface IWrapperProviderFactory
    {
        /// <summary>
        /// Gets the <see cref="IWrapperProvider"/> for the provided context.
        /// </summary>
        /// <param name="context">The <see cref="WrapperProviderContext"/></param>
        /// <returns>A wrapping provider if the factory decides to wrap the type, else null.</returns>
        IWrapperProvider GetProvider(WrapperProviderContext context);
    }
}
