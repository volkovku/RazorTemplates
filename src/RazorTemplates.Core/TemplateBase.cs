using System.Text;

namespace RazorTemplates.Core
{
    /// <summary>
    /// Represents a base class for generated templates.
    /// </summary>
    public abstract class TemplateBase
    {
        private readonly StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// Gets or sets dynamic model which data should be rendered.
        /// </summary>
        public dynamic Model { get; set; }

        #region Execute

        /// <summary>
        /// Renders specified model.
        /// </summary>
        public virtual string Render(object model)
        {
            Model = model;

            Execute();

            return _buffer.ToString();
        }

        /// <summary>
        /// A method which implemets by Razor engine.
        /// Produces sequance like:
        ///     WriteLiteral("Hello ");
        ///     Write(Model.Name);
        ///     WriteLiteral("!");
        /// </summary>
        public abstract void Execute();

        #endregion

        #region Write

        protected void Write(string value)
        {
            _buffer.Append(value);
        }

        protected void Write(object value)
        {
            _buffer.Append(value);
        }

        #endregion

        #region WriteLiteral

        protected void WriteLiteral(string value)
        {
            _buffer.Append(value);
        }

        #endregion
    }
}