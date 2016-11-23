using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhythm.Text
{
    /// <summary>
    /// Represents an exception which raises when template contains compilation errors.
    /// </summary>
    public class TemplateCompilationException : Exception
    {
        private readonly CompilerErrorCollection _errors;
        private readonly string _sourceCode;
        private readonly string _template;

        /// <summary>
        /// Initializes a new instance of TemplateCompilationException class.
        /// </summary>
        /// <param name="errors">A collection of occured errors.</param>
        /// <param name="sourceCode">A generated source code (available if debug mode is on).</param>
        /// <param name="template">A template code.</param>
        public TemplateCompilationException(CompilerErrorCollection errors, string sourceCode, string template)
        {
            _errors = errors;
            _sourceCode = sourceCode;
            _template = template;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get { return GetErrorMessage(); }
        }

        /// <summary>
        /// Gets a collection of occured errors.
        /// </summary>
        public IEnumerable<CompilerError> Errors
        {
            get { return _errors.Cast<CompilerError>(); }
        }

        /// <summary>
        /// Gets a generated source code.
        /// </summary>
        public string SourceCode
        {
            get { return _sourceCode; }
        }

        /// <summary>
        /// Gets a template code.
        /// </summary>
        public string Template
        {
            get { return _template; }
        }

        private string GetErrorMessage()
        {
            var result = new StringBuilder();
            result.AppendLine("An errors was occured on template compilation:");
            result.AppendLine();

            foreach (var error in Errors)
            {
                result.AppendLine(error.ToString());
                result.AppendLine();
            }

            result.AppendLine("Template source code:");
            result.AppendLine(Template);
            result.AppendLine();

            result.AppendLine("Generated source code:");
            if (string.IsNullOrEmpty(SourceCode))
            {
                result.AppendLine("Generated source code is not available.");
                result.AppendLine("For enable source code turn on Template.Debug option.");
            }
            else
            {
                result.AppendLine(SourceCode);
            }

            return result.ToString();
        }
    }
}