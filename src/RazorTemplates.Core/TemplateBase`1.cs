using System.Text;

namespace RazorTemplates.Core
{
    /// <summary>
    /// Represents a base class for generated templates.
    /// </summary>
    public abstract class TemplateBase<TModel> : TemplateBase
    {
        private readonly StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// Gets or sets dynamic model which data should be rendered.
        /// </summary>
        public new virtual TModel Model
        {
            get { return (TModel)base.Model; }
            set { base.Model = value; }
        }

        /// <summary>
        /// Renders specified model.
        /// </summary>
        public virtual string Render(TModel model)
        {
            Model = model;
            Execute();
            return _buffer.ToString();
        }
    }
}