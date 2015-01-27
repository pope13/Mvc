// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc.Xml
{
    public class DelegatingEnumerator<TWrappedOrDeclared, TDeclared> : IEnumerator<TWrappedOrDeclared>
    {
        private readonly IEnumerator<TDeclared> _inner;
        private readonly IWrapperProvider _wrapperProvider;

        public DelegatingEnumerator([NotNull] IEnumerator<TDeclared> inner, IWrapperProvider wrapperProvider)
        {
            _inner = inner;
            _wrapperProvider = wrapperProvider;
        }

        public TWrappedOrDeclared Current
        {
            get
            {
                object obj = _inner.Current;
                if (_wrapperProvider == null)
                {
                    // if there is no wrapper, then this cast should not fail
                    return (TWrappedOrDeclared)obj;
                }

                return (TWrappedOrDeclared)_wrapperProvider.Wrap(obj);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                var obj = _inner.Current;
                if (_wrapperProvider == null)
                {
                    return obj;
                }

               return _wrapperProvider.Wrap(obj); 
            }
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }
    }
}