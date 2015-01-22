// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.FeatureModel;
using Microsoft.AspNet.Http.Core;
using Microsoft.AspNet.Http.Interfaces;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Fallback;

namespace Microsoft.AspNet.Mvc.Testing
{
	public static class ControllerTestHelper
	{
		public static Controller Initialize(Controller controller, ControllerTestHelperContext context)
		{
			return Initialize(controller, new ActionDescriptor(), context);
			// Question: Can we build (fully or partially) ActionDescriptor() from tyep information??
		}

		private static DefaultHttpContext CreateHttpContext(IServiceProvider serviceProvider)
		{
			var featureCollection = new FeatureCollection();
			var featureContext = new FeatureContext();

			featureCollection.Add(typeof(IHttpRequestLifetimeFeature), new FeatureContext());
			featureCollection.Add(typeof(IHttpRequestFeature), new FeatureContext());

			var httpContext = new DefaultHttpContext(featureCollection)
			{
				RequestServices = serviceProvider
			};

			return httpContext;
		}

		private static Controller Initialize(Controller controller, ActionDescriptor actionDescriptor,
			ControllerTestHelperContext helperContext)
		{
			helperContext = helperContext ?? new ControllerTestHelperContext();

			helperContext.RouteData = helperContext.RouteData ?? new RouteData();
			helperContext.ServiceCollection = helperContext.ServiceCollection ?? new ServiceCollection();
			helperContext.ServiceProvider = helperContext.ServiceProvider ??
				helperContext.ServiceCollection.BuildServiceProvider();
			helperContext.HttpContext = CreateHttpContext(helperContext.ServiceProvider);

			controller.ActionContext =
				new ActionContext(helperContext.HttpContext, helperContext.RouteData, actionDescriptor);
			controller.Url = new MockUrlHelper();

			helperContext.HttpContext.Request.Form = helperContext.RequestFormCollection;

			if (helperContext.RequestAborted)
			{
				helperContext.HttpContext.Abort();
			}

			((MockUrlHelper)controller.Url).OnAction
				= (action, controllerName, values, protocol, host, fragment)
				=>
				{
					helperContext.RequestScheme = protocol;
					helperContext.RequestHost = host;
					helperContext.RequestFragment = fragment;

					return helperContext.OnUrlAction(action, controllerName, values);
				};

			return controller;
		}

		private class MockUrlHelper : IUrlHelper
		{
			public Func<string, string, object, string, string, string, string> OnAction
			{
				get;
				set;
			}

			public string Action(string action, string controller, object values, string protocol, string host, string fragment)
			{
				return OnAction(action, controller, values, protocol, host, fragment);
			}

			public string Content(string contentPath)
			{
				throw new NotImplementedException();
			}

			public string RouteUrl(string routeName, object values, string protocol, string host, string fragment)
			{
				throw new NotImplementedException();
			}
		}
	}
}