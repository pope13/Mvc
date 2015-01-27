﻿using System;
using Microsoft.AspNet.Mvc.Xml;
using XmlFormattersWebSite.Models;

namespace XmlFormattersWebSite
{
    public class PersonWrapperProvider : IWrapperProvider
    {
        public object Wrap(object obj)
        {
            var person = obj as Person;

            if (person == null)
            {
                return obj;
            }

            return new PersonWrapper(person);
        }

        public Type GetWrappingType(Type declaredType)
        {
            if (declaredType == typeof(Person))
            {
                return typeof(PersonWrapper);
            }

            return null;
        }
    }
}