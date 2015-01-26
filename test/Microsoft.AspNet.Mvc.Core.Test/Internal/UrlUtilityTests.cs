// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Mvc.Core.Internal;
using Xunit;

namespace Microsoft.AspNet.Mvc.Core.Test.Internal
{
    public class UrlUtilityTests
    {
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void IsLocalUrl_ReturnsFalseOnEmpty(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("/foo.html")]
		[InlineData("/www.example.com")]
		[InlineData("/")]
		public void IsLocalUrl_AcceptsRootedUrls(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData("~/")]
		[InlineData("~/foo.html")]
		public void IsLocalUrl_AcceptsApplicationRelativeUrls(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData("foo.html")]
		[InlineData("../foo.html")]
		[InlineData("fold/foo.html")]
		public void IsLocalUrl_RejectsRelativeUrls(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("http:/foo.html")]
		[InlineData("hTtP:foo.html")]
		[InlineData("http:/www.example.com")]
		[InlineData("HtTpS:/www.example.com")]
		public void IsLocalUrl_RejectValidButUnsafeRelativeUrls(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("http://www.mysite.com/appDir/foo.html")]
		[InlineData("http://WWW.MYSITE.COM")]
		public void IsLocalUrl_RejectsUrlsOnTheSameHost(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("http://localhost/foobar.html")]
		[InlineData("http://127.0.0.1/foobar.html")]
		public void IsLocalUrl_RejectsUrlsOnLocalHost(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("https://www.mysite.com/")]
		public void IsLocalUrl_RejectsUrlsOnTheSameHostButDifferentScheme(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("http://www.example.com")]
		[InlineData("https://www.example.com")]
		[InlineData("hTtP://www.example.com")]
		[InlineData("HtTpS://www.example.com")]
		public void IsLocalUrl_RejectsUrlsOnDifferentHost(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("http://///www.example.com/foo.html")]
		[InlineData("https://///www.example.com/foo.html")]
		[InlineData("HtTpS://///www.example.com/foo.html")]
		[InlineData("http:///www.example.com/foo.html")]
		[InlineData("http:////www.example.com/foo.html")]
		public void IsLocalUrl_RejectsUrlsWithTooManySchemeSeparatorCharacters(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("//www.example.com")]
		[InlineData("//www.example.com?")]
		[InlineData("//www.example.com:80")]
		[InlineData("//www.example.com/foobar.html")]
		[InlineData("///www.example.com")]
		[InlineData("//////www.example.com")]
		public void IsLocalUrl_RejectsUrlsWithMissingSchemeName(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("http:\\\\www.example.com")]
		[InlineData("http:\\\\www.example.com\\")]
		[InlineData("/\\")]
		[InlineData("/\\foo")]
		public void IsLocalUrl_RejectsInvalidUrls(string url)
		{
			// Arrange && Act
			var result = UrlUtility.IsLocalUrl(url);

			// Assert
			Assert.False(result);
		}
	}
}