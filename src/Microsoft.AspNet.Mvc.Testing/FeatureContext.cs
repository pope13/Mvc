// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Core;
using Microsoft.AspNet.Http.Interfaces;

namespace Microsoft.AspNet.Mvc.Testing
{
	internal class FeatureContext :
		IHttpRequestLifetimeFeature,
		IHttpRequestFeature,
		IFormFeature
	// More features will be added here as it goes.
	{

		private CancellationToken _cancelationToken = CancellationToken.None;

		private Dictionary<string, string[]> _headers;

		internal FeatureContext()
		{
			_headers = new Dictionary<string, string[]>();
		}

		CancellationToken IHttpRequestLifetimeFeature.RequestAborted
		{
			get
			{
				return _cancelationToken;
			}
		}
		void IHttpRequestLifetimeFeature.Abort()

		{
			_cancelationToken = new CancellationToken(true);
		}

		IFormCollection IFormFeature.ReadForm()
		{
			throw new NotImplementedException();
		}

		Task<IFormCollection> IFormFeature.ReadFormAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		Stream IHttpRequestFeature.Body
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		IDictionary<string, string[]> IHttpRequestFeature.Headers
		{
			get
			{
				return _headers;
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		string IHttpRequestFeature.Method
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		string IHttpRequestFeature.Path
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		string IHttpRequestFeature.PathBase
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		string IHttpRequestFeature.Protocol
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		string IHttpRequestFeature.QueryString
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		string IHttpRequestFeature.Scheme
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		bool IFormFeature.HasFormContentType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		IFormCollection IFormFeature.Form
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}
	}
}