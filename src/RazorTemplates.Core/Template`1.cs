using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace RazorTemplates.Core
{
    internal class Template<T> : ITemplate where T : TemplateBase
    {
        private readonly Type _templateType;
        private readonly Action<T> _initializer;

        internal Template(Type templateType, Action<T> initializer)
        {
            _templateType = templateType;
            _initializer = initializer;
        }

        /// <summary>
        /// Renders templates with data from specified model.
        /// </summary>
        /// <param name="model">A model data.</param>
        /// <returns>A rendered content.</returns>
        public string Render(object model = null)
        {
            var instance = (T)Activator.CreateInstance(_templateType);

            if (_initializer != null)
                _initializer(instance);

            return instance.Render(CreateExpandoObject(model));
        }

        private static ExpandoObject CreateExpandoObject(object anonymousObject)
        {
            var anonymousDictionary = new Dictionary<string, object>();
            if (anonymousObject != null)
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(anonymousObject))
                    anonymousDictionary.Add(property.Name, property.GetValue(anonymousObject));

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary) expando.Add(item);

            return (ExpandoObject)expando;
        }
    }
}