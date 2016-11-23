namespace Rhythm.Text.Templating
{
    /// <summary>
    /// Describes an interface of template.
    /// </summary>
    public interface ITemplateRender<in TModel>
    {
        /// <summary>
        /// Returns generated source code for this template.
        /// </summary>
        string SourceCode { get; }

        /// <summary>
        /// Renders templates with data from specified model.
        /// </summary>
        /// <param name="model">A model data.</param>
        /// <returns>A rendered content.</returns>
        string Render(TModel model);
    }
}