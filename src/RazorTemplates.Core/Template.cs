using System;
using System.Linq;

namespace RazorTemplates.Core
{
    /// <summary>
    /// Represents an entry point for create templates.
    /// </summary>
    public static class Template
    {
        /// <summary>
        /// Gets or sets flag which determines is templates debugging info will
        /// included in output results.
        /// </summary>
        public static bool Debug
        {
            get { return TemplateCompiler.Debug; }
            set { TemplateCompiler.Debug = value; }
        }

        /// <summary>
        /// Returns template created from specified source.
        /// </summary>
        public static ITemplate Compile(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException(
                    "Template source can't be null or empty string.",
                    "source");

            var compilationResult = TemplateCompiler.Compile(
                typeof(TemplateBase),
                source,
                Enumerable.Empty<string>() /* namespaces */,
                null /* compilation directory */);

            return new Template<TemplateBase>(compilationResult.Type, compilationResult.SourceCode, null);
        }

        /// <summary>
        /// Creates template with specified base type.
        /// </summary>
        public static TemplateDescription<T> WithBaseType<T>(Action<T> inializer = null) where T : TemplateBase
        {
            return new TemplateDescription<T>(inializer);
        }

        public static TemplateDescription<TemplateBase<TModel>, TModel> WithModel<TModel>(Action<TemplateBase<TModel>> inializer = null)
        {
            return new TemplateDescription<TemplateBase<TModel>, TModel>(inializer);

        }
    }
}
