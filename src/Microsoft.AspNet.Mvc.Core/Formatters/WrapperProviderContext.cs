using System;

namespace Microsoft.AspNet.Mvc
{
    public class WrapperContext
    {
        public Type OriginalType { get; set; }

        public Type WrappingType { get; set; }

        public IWrapperProvider WrapperProvider { get; set; }
    }
}