using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace RazorTemplates.Core
{
    internal class Template<T, TModel> : ITemplate<TModel> where T : TemplateBase
    {
        private readonly Type _templateType;
        private readonly Action<T> _initializer;
        private readonly string _sourceCode;

        internal Template(Type templateType, string sourceCode, Action<T> initializer)
        {
            _templateType = templateType;
            _sourceCode = sourceCode;
            _initializer = initializer;
        }

        public string SourceCode
        {
            get { return _sourceCode; }
        }

        public string Render(TModel model)
        {
            var template = CreateTemplateInstance();

            if (!ReferenceEquals(null, model)
                && IsAnonymousType(model))
                return template.Render(CreateExpandoObject(model));

            return template.Render(model);
        }

        /// <summary>
        /// Is the type of the model an Anonymous type?
        /// </summary>
        /// <param name="model">Model to be checked</param>
        /// <returns>True if the model is a C# or VB.NET anonymous type</returns>
        /// <remarks>See also http://msdn.microsoft.com/en-us/library/cc468406(v=vs.90).aspx </remarks>
        private bool IsAnonymousType(object model)
        {
            const string csharpAnonPrefix = "<>f__AnonymousType";
            const string vbAnonPrefix = "VB$Anonymous";

            var typeName = model.GetType().Name;

            return (typeName.StartsWith(csharpAnonPrefix) || typeName.StartsWith(vbAnonPrefix));
        }

        protected T CreateTemplateInstance()
        {
            var instance = (T)Activator.CreateInstance(_templateType);

            if (_initializer != null)
                _initializer(instance);
            return instance;
        }

        private static ExpandoObject CreateExpandoObject(object anonymousObject)
        {
            var expandoObject = anonymousObject as ExpandoObject;
            if (expandoObject != null) return expandoObject;

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