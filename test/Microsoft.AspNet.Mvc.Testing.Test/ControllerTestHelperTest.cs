// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Http.Core.Collections;
using Xunit;

namespace Microsoft.AspNet.Mvc.Testing.Test
{
	public class ControllerTestHelperTest
	{
		[Fact]
		public void ControllerTestability_UsingContextRequestAbortedTest()
		{
			// Arrange
			var controller = new TestabilityController();

			var context = new ControllerTestHelperContext()
			{
				RequestAborted = true
			};

			// Act
			ControllerTestHelper.Initialize(controller, context);
			var viewResult = controller.UsingContextRequestAborted();

			// Assert
			Assert.Null(viewResult);

			// Arrange
			context = new ControllerTestHelperContext()
			{
				RequestAborted = false
			};

			// Act
			ControllerTestHelper.Initialize(controller, context);
			viewResult = controller.UsingContextRequestAborted();

			// Assert
			Assert.Equal("Request", viewResult.ViewName);
		}

		[Fact]
		public void ControllerTestability_UsingModelStateTest()
		{
			// Arrange
			var controller = new TestabilityController();
			var model = new MyModel()
			{
				Property1 = "ValidModel",
				Property2 = "InvalidModel"
			};

			// Act
			ControllerTestHelper.Initialize(controller, null);
			controller.ModelState.AddModelError("", "error_1");
			var result = controller.UsngModelState(model);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal(model.Property2, viewResult.ViewName);
			Assert.NotNull(viewResult.ViewData.Model);
			Assert.Equal(1, controller.ModelState.ErrorCount);
		}

		[Fact]
		public void ControllerTestability_UsingFormCollectionTest()
		{
			// Arrange
			var controller = new TestabilityController();
			var formData = new Dictionary<string, string[]>() { { "formKey1", new string[] { "formKey1Value" } } };
			var formCollection = new FormCollection(formData);

			var context = new ControllerTestHelperContext()
			{
				RequestFormCollection = formCollection
			};

			// Act
			ControllerTestHelper.Initialize(controller, context);
			var result = controller.UsingContextFormCollection("formKey1");

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal(viewResult.ViewName, "formKey1Value");
		}

		[Fact]
		public void ControllerTestability_UsingActionTest()
		{
			// Arrange
			var controller = new TestabilityController();

			var helperContext = new ControllerTestHelperContext()
			{
				OnUrlAction = (action, controllerName, values)
				=>
				{
					Assert.Equal("redirect", action);
					return action;
				}
			};
			ControllerTestHelper.Initialize(controller, helperContext);

			// Act && Assert
			var result = controller.UsingUrlAction("redirect");

			// Arrange
			controller = new TestabilityController();

			helperContext = new ControllerTestHelperContext()
			{
				OnUrlAction = (action, controllerName, values)
				=>
				{
					Assert.Equal("controller1", controllerName);
					Assert.Equal(null, values);
					Assert.Equal("http", helperContext.RequestScheme);
					Assert.Equal("host1", helperContext.RequestHost);
					Assert.Equal("fragment1", helperContext.RequestFragment);
					Assert.Equal("redirect", action);
					return action;
				}
			};

			ControllerTestHelper.Initialize(controller, helperContext);

			// Act && Assert
			result = controller.UsingUrlActionFull("redirect", "controller1", null, "http", "host1", "fragment1");
		}

		private class MyModel
		{
			[Required]
			public string Property1
			{
				get; set;
			}

			[StringLength(3)]
			public string Property2
			{
				get;
				set;
			}
		}

		private class TestabilityController : Controller
		{
			public ViewResult UsingViewBagAction()
			{
				ViewBag.StatusMessage = "Hello World";
				return View();
			}

			public ViewResult UsingContextRequestAborted()
			{
				if (Context.RequestAborted.IsCancellationRequested)
				{
					return null;
				}
				return View("Request");
			}

			public IActionResult UsngModelState(MyModel model)
			{
				if (ModelState.IsValid)
				{
					return View(model.Property1);
				}

				return View(model.Property2, model);
			}

			public IActionResult UsingContextFormCollection(string formKey)
			{
				var formCollection = Context.Request.ReadFormAsync();
				var viewName = formCollection.Result.GetValues(formKey).FirstOrDefault();

				return View(viewName);
			}

			public IActionResult UsingUrlAction(string action)
			{
				return View(Url.Action(action));
			}

			public IActionResult UsingUrlActionFull(string action, string controller, object values, string protocol,
				string host, string fragment)
			{
				return View(Url.Action(action, controller, values, protocol, host, fragment));
			}
		}
	}
}