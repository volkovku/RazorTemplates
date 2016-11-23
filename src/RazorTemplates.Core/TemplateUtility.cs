using System;
using System.Linq;

namespace Rhythm.Text.Templating
{
    /// <summary>
    /// Represents an entry point for create templates.
    /// </summary>
    public static class TemplateUtility
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
        /// Gets or sets the language that will be used to compile templates.
        /// </summary>
        public static TemplateCompilationLanguage Language
        {
            get { return TemplateCompiler.Language; }
            set { TemplateCompiler.Language = value; }
        }

        /// <summary>
        /// Returns template created from specified source.
        /// </summary>
        public static ITemplateRender<dynamic> Compile(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException(
                    "Template source can't be null or empty string.",
                    "source");

            var compilationResult = TemplateCompiler.Compile(
                typeof(TemplateBase),
                source,
                Enumerable.Empty<string>() /* assembly file names */,
                Enumerable.Empty<string>() /* namespaces */,
                null /* compilation directory */);

            return new TemplateRender<TemplateBase, object>(compilationResult.Type, compilationResult.SourceCode, null);
        }

        /// <summary>
        /// Returns strong typed template created from specified source.
        /// </summary>
        public static ITemplateRender<TModel> Compile<TModel>(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException(
                    "Template source can't be null or empty string.",
                    "source");

            var compilationResult = TemplateCompiler.Compile(
                typeof (TemplateBase<TModel>),
                source,
                Enumerable.Empty<string>() /* assembly file names */,
                Enumerable.Empty<string>() /* namespaces */,
                null /* compilation directory */);

            return new TemplateRender<TemplateBase<TModel>, TModel>(compilationResult.Type, compilationResult.SourceCode, null);
        }

        /// <summary>
        /// Creates template with specified base type.
        /// </summary>
        public static TemplateDescription<T> WithBaseType<T>(Action<T> inializer = null) where T : TemplateBase
        {
            return new TemplateDescription<T>(inializer);
        }
    }
}
