using System;
using System.Collections.Generic;

namespace RazorTemplates.Core
{
    public sealed class TemplateDescription<T> where T : TemplateBase
    {
        private readonly HashSet<string> _namespaces = new HashSet<string>();
        private readonly Action<T> _templateInitializer;

        public TemplateDescription(Action<T> templateInitializer)
        {
            _templateInitializer = templateInitializer;
        }

        public TemplateDescription<T> AddNamespace(string @namespace)
        {
            _namespaces.Add(@namespace);
            return this;
        }

        public ITemplate Compile(string source)
        {
            var templateType = TemplateCompiler.Compile(typeof(T), source, _namespaces);
            return new Template<T>(templateType, _templateInitializer);
        }
    }
}