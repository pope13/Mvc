// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Net.Http.Headers;
using UrlHelperWebSite.Controllers;

namespace UrlHelperWebSite
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var configuration = app.GetTestConfiguration();

            // Set up application services
            app.UseServices(services =>
            {
                services.Configure<AppOptions>(optionsSetup =>
                {
                    optionsSetup.ServeCDNContent = true;
                    optionsSetup.CDNServerBaseUrl = "http://cdn.contoso.com";
                    optionsSetup.GenerateLowercaseUrls = true;
                });

                // Add MVC services to the services container
                services.AddMvc(configuration);

                services.Configure<MvcOptions>(options =>
                {
                    options.InputFormatters.Clear();
                    options.OutputFormatters.Clear();

                    var xmlSerializerInputFormatter = new XmlSerializerInputFormatter();
                    xmlSerializerInputFormatter.WrapperProviders.Add(new PersonWrapperProvider());
                    xmlSerializerInputFormatter.SupportedMediaTypes.Clear();
                    xmlSerializerInputFormatter.SupportedMediaTypes.Add(
                        new MediaTypeHeaderValue("application/xml-xmlser"));
                    xmlSerializerInputFormatter.SupportedMediaTypes.Add(
                        new MediaTypeHeaderValue("text/xml-xmlser"));

                    var xmlSerializerOutputFormatter = new XmlSerializerOutputFormatter();
                    xmlSerializerOutputFormatter.WrapperProviders.Add(new PersonWrapperProvider());
                    xmlSerializerOutputFormatter.SupportedMediaTypes.Clear();
                    xmlSerializerOutputFormatter.SupportedMediaTypes.Add(
                        new MediaTypeHeaderValue("application/xml-xmlser"));
                    xmlSerializerOutputFormatter.SupportedMediaTypes.Add(
                        new MediaTypeHeaderValue("text/xml-xmlser"));

                    var dcsInputFormatter = new XmlDataContractSerializerInputFormatter();
                    dcsInputFormatter.WrapperProviders.Add(new PersonWrapperProvider());
                    dcsInputFormatter.SupportedMediaTypes.Clear();
                    dcsInputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml-dcs"));
                    dcsInputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml-dcs"));

                    var dcsOutputFormatter = new XmlDataContractSerializerOutputFormatter();
                    dcsOutputFormatter.WrapperProviders.Add(new PersonWrapperProvider());
                    dcsOutputFormatter.SupportedMediaTypes.Clear();
                    dcsOutputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml-dcs"));
                    dcsOutputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml-dcs"));

                    options.InputFormatters.Add(dcsInputFormatter);
                    options.InputFormatters.Add(xmlSerializerInputFormatter);
                    options.OutputFormatters.Add(dcsOutputFormatter);
                    options.OutputFormatters.Add(xmlSerializerOutputFormatter);
                });

                services.AddScoped<IUrlHelper, CustomUrlHelper>();
            });

            // Add MVC to the request pipeline
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
