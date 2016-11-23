using System;

namespace Rhythm.Text
{
    /// <summary>
    /// Represents result of template compilation.
    /// </summary>
    internal struct TemplateCompilationResult
    {
        /// <summary>
        /// A type of compiled temlate (inherit from TemplateBase).
        /// </summary>
        public Type Type;

        /// <summary>
        /// A source code of compiled template.
        /// </summary>
        public string SourceCode;
    }
}