using System;
using System.Collections.Generic;

namespace RazorTemplates.Core
{
    /// <summary>
    /// Represents a description of new Razor template type.
    /// </summary>
    public class TemplateDescription<T> where T : TemplateBase
    {
        private readonly HashSet<string> _namespaces = new HashSet<string>();
        private readonly Action<T> _templateInitializer;

        /// <summary>
        /// Initializes a new instance of TemplateDescription class.
        /// </summary>
        internal TemplateDescription(Action<T> templateInitializer)
        {
            _templateInitializer = templateInitializer;
        }

        /// <summary>
        /// Gets a directory which Razor compiler should use to store temp files
        /// </summary>
        internal string CompilationDirectory { get; set; }

        /// <summary>
        /// Specifies that Razor template should uses specified namespace.
        /// </summary>
        public TemplateDescription<T> AddNamespace(string @namespace)
        {
            _namespaces.Add(@namespace);
            return this;
        }

        /// <summary>
        /// Specifies that Razor compiler should use specified directory to store temp files.
        /// </summary>
        public TemplateDescription<T> CompileTo(string compilationDirectory)
        {
            if (string.IsNullOrWhiteSpace(compilationDirectory))
                throw new ArgumentException(
                    "Compilation directory can't be null or whitespace.",
                    "compilationDirectory");

            CompilationDirectory = compilationDirectory;
            return this;
        }

        /// <summary>
        /// Creates template from specified source.
        /// </summary>
        public ITemplate<dynamic> Compile(string source)
        {
            var compilationResult = InternalCompile(source);
            return new Template<T, dynamic>(compilationResult.Type, compilationResult.SourceCode, _templateInitializer);
        }

        /// <summary>
        /// Creates template from specified source.
        /// </summary>
        public ITemplate<TModel> Compile<TModel>(string source)
        {
            var compilationResult = InternalCompile(source);
            return new Template<T, TModel>(compilationResult.Type, compilationResult.SourceCode, _templateInitializer);
        }

        internal TemplateCompilationResult InternalCompile(string source)
        {
            var compilationResult = TemplateCompiler.Compile(typeof(T), source, _namespaces, CompilationDirectory);
            return compilationResult;
        }
    }
}