namespace RazorTemplates.Core
{
    /// <summary>
    /// Describes an interface of template.
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Renders templates with data from specified model.
        /// </summary>
        /// <param name="model">A model data.</param>
        /// <returns>A rendered content.</returns>
        string Render(object model = null);
    }
}