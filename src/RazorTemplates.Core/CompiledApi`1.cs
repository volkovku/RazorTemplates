using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using Rhythm.Text.Core;

namespace Rhythm.Text
{
    internal class CompiledApi<T, TModel> : ICompiledApi<TModel> where T : TemplateBase
    {
        private readonly Type _templateType;
        private readonly Action<T> _initializer;
        private readonly string _sourceCode;

        internal CompiledApi(Type templateType, string sourceCode, Action<T> initializer)
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
			var template = (ITemplateInternal)CreateTemplateInstance();


			if (!ReferenceEquals(null, model) && model.GetType().Name.StartsWith("<>f__AnonymousType"))
			{
				template.SetModel(CreateExpandoObject(model));
			template.Execute();
				return template.Result;
			}else
			{
				template.SetModel(model);

				template.Execute();
				return template.Result;

			}
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