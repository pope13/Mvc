// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Core.Collections;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc.Testing
{
	public class ControllerTestHelperContext
	{

		// Not sure if this RouteData property will be usefull..
		public RouteData RouteData
		{
			get;
			set;
		}

		// Not sure if this HttpContext property will be usefull..
		public HttpContext HttpContext
		{
			get;
			set;
		}

		public IServiceCollection ServiceCollection
		{
			get;
			set;
		}

		public IServiceProvider ServiceProvider
		{
			get;
			set;
		}
		public string RequestFragment
		{
			get;
			set;
		}

		public string RequestHost
		{
			get;
			set;
		}

		public string RequestScheme
		{
			get;
			set;
		}

		public bool RequestAborted
		{
			get;
			set;
		}

		public FormCollection RequestFormCollection
		{
			get;
			set;
		}

		public Func<string, string, object, string> OnUrlAction
		{
			get;
			set;
		}
	}
}