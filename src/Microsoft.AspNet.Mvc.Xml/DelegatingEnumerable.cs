// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNet.Mvc.Xml
{
    /// <summary>
    /// Helper class to serialize <see cref="IEnumerable{T}"/> types by delegating them through a concrete 
    /// implementation.
    /// </summary>
    /// <typeparam name="TWrappedOrDeclared">The wrapping or original type of the <see cref="IEnumerable{T}"/> 
    /// to proxy.</typeparam>
    /// <typeparam name="TDeclared">The type parameter of the original <see cref="IEnumerable{T}"/> 
    /// to proxy.</typeparam>
    public sealed class DelegatingEnumerable<TWrappedOrDeclared, TDeclared> : IEnumerable<TWrappedOrDeclared>
    {
        private IEnumerable<TDeclared> _source;
        private readonly IWrapperProvider _wrapperProvider;

        /// <summary>
        /// Initializes a <see cref="DelegatingEnumerable{TWrappedOrDeclared, TDeclared}"/>. 
        /// This constructor is necessary for <see cref="System.Runtime.Serialization.DataContractSerializer"/> 
        /// to serialize.
        /// </summary>
        public DelegatingEnumerable()
        {
            _source = Enumerable.Empty<TDeclared>();
        }

        /// <summary>
        /// Initializes a <see cref="DelegatingEnumerable{TWrappedOrDeclared, TDeclared}"/> with the original
        ///  <see cref="IEnumerable{T}"/> and the wrapper provider for individual elements.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> instance to get the enumerator from.</param>
        /// <param name="wrapperProvider">The wrapper for wrapping individual elements.</param>
        public DelegatingEnumerable(
            [NotNull] IEnumerable<TDeclared> source, 
            IWrapperProvider wrapperProvider)
        {
            _source = source;
            _wrapperProvider = wrapperProvider;
        }

        /// <summary>
        /// Get the enumerator of the associated <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>The enumerator of the <see cref="IEnumerable{T}"/> source.</returns>
        public IEnumerator<TWrappedOrDeclared> GetEnumerator()
        {
            return new DelegatingEnumerator<TWrappedOrDeclared, TDeclared>(_source.GetEnumerator(), _wrapperProvider);
        }

        /// <summary>
        /// This method is not implemented but is required method for serialization to work. Do not use.
        /// </summary>
        /// <param name="item">The item to add. Unused.</param>
        public void Add(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the enumerator of the associated <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>The enumerator of the <see cref="IEnumerable{T}"/> source.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DelegatingEnumerator<TWrappedOrDeclared, TDeclared>(_source.GetEnumerator(), _wrapperProvider);
        }
    }
}