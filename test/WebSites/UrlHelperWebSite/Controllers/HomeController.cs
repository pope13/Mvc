// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc;

namespace UrlHelperWebSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string UrlContent()
        {
            return Url.Content("~/Bootstrap.min.css");
        }

        public string LinkByUrlAction()
        {
            return Url.Action("UrlContent", "Home", null);
        }

        public string LinkByUrlRouteUrl()
        {
            return Url.RouteUrl("SimplePocoApi", new { id = 10 });
        }

        [HttpGet]
        public Person Details()
        {
            return new Person() { Id = 10, Name = "Mike" };
        }

        [HttpPost]
        public IActionResult Create([FromBody] Person person)
        {
            if(!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            return Content(person != null ? person.ToString() : "Null parameter");
        }
    }


    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Id, Name);
        }
    }


    public class PersonWrapperProvider : IWrapperProvider
    {
        public bool TryGetWrappingType(Type originalType, out Type wrappingType)
        {
            bool hasWrappingType = false;
            wrappingType = null;

            if(originalType == typeof(Person))
            {
                wrappingType = typeof(PersonWrapper);
                hasWrappingType = true;
            }

            return hasWrappingType;
        }

        public object Unwrap(object obj)
        {
            var personWrapper = obj as PersonWrapper;

            if(personWrapper == null)
            {
                return obj;
            }

            return new Person() { Id = personWrapper.Id, Name = personWrapper.Name };
        }

        public object Wrap(object obj)
        {
            var person = obj as Person;

            if (person == null)
            {
                return obj;
            }

            return new PersonWrapper(person);
        }
    }
    
    public class PersonWrapper
    {
        public PersonWrapper() { }

        public PersonWrapper(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Age = 35;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Id, Name, Age);
        }
    }

}