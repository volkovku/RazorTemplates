namespace RazorTemplates.Core
{
    /// <summary>
    /// Describes an interface of template.
    /// </summary>
    public interface ITemplate
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
        string Render(object model = null);
    }

    public interface ITemplate<in TModel> : ITemplate
    {
        string Render(TModel model);
    }
}