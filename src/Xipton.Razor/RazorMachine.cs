﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Razor;
using Rhythm.Text.Config;
using Rhythm.Text.Core;
using Rhythm.Text.Core.ContentProvider;
using Rhythm.Text.Extension;

namespace Rhythm.Text
{
    /// <summary>
    /// The main razor template executer. Use a singleton instance because each instance creates its own type cache (at the template factory).
    /// </summary>
    public class RazorMachine : IDisposable
    {

        protected internal const string ContentGeneratedUrlPrefix = "~/_MemoryContent/_{0}";

        private readonly ConcurrentDictionary<string, string>
            _templateContentToGeneratedVirtualPathMap = new ConcurrentDictionary<string, string>();

        #region Constructors
        public RazorMachine(){
            Context = new RazorContext(new RazorConfig().Initializer.TryInitializeFromConfig().AsReadOnly());
        }
        public RazorMachine(RazorConfig config){
            Context = new RazorContext(config ?? new RazorConfig().Initializer.TryInitializeFromConfig().AsReadOnly());
        }
        public RazorMachine(string xmlContentOrFileName){
            if (xmlContentOrFileName == null)
                // default initialization (from app.config or else default settings)
                Context = new RazorContext(new RazorConfig().Initializer.TryInitializeFromConfig().AsReadOnly());
            else if (xmlContentOrFileName.IsFileName())
                // initialization by xml configuration file
                Context = new RazorContext(new RazorConfig().Initializer.InitializeByXmlFileName(xmlContentOrFileName).AsReadOnly());
            else if (xmlContentOrFileName.IsXmlContent())
                // initialization by literal xml configuration string
                Context = new RazorContext(new RazorConfig().Initializer.InitializeByXmlContent(xmlContentOrFileName).AsReadOnly());
            else
                throw new ArgumentException("Argument must be either an existing file name or literal xml content", "xmlContentOrFileName");
        }

        public RazorMachine(
            Type baseType = null,
            string rootOperatorPath = null,
            RazorCodeLanguage language = null,
            string defaultExtension = null,
            string autoIncludeNameWithoutExtension = null,
            string sharedLocation = null,
            bool? includeGeneratedSourceCode = null,
            bool? htmlEncode = null,
            IEnumerable<string> references = null,
            IEnumerable<string> namespaces = null,
            IEnumerable<Func<IContentProvider>> contentProviders = null,
            bool replaceReferences = false,
            bool replaceNamespaces = false,
            bool replaceContentProviders = false
            )
        {
            Context = new RazorContext(new RazorConfig()
                .Initializer
                .TryInitializeFromConfig()
                .InitializeByValues(
                    baseType, 
                    rootOperatorPath, 
                    language, 
                    defaultExtension, 
                    autoIncludeNameWithoutExtension, 
                    sharedLocation, 
                    includeGeneratedSourceCode, 
                    htmlEncode, 
                    references, 
                    namespaces, 
                    contentProviders, 
                    replaceReferences, 
                    replaceNamespaces,
                    replaceContentProviders
                )
                .AsReadOnly()
            );
        }

        #endregion

        /// <summary>
        /// Executes the template by its virtual path and returns the resulting executed template instance.
        /// </summary>
        /// <param name="templateVirtualPath">The requested virtual path for the template.</param>
        /// <param name="model">The optional model.</param>
        /// <param name="viewbag">The optional viewbag.</param>
        /// <param name="skipLayout">if set true then any layout setting is ignored</param>
        /// <param name="throwExceptionOnVirtualPathNotFound">
        ///   Optional. If set to <c>true</c> an exception is thrown if the requested path could not be resolved. 
        ///   If set to false then null is returned if the requested path could not be resolved.
        /// </param>
        /// <returns>An executed template instance. The corresponding rendered result can be found at <see cref="ITemplate.Result"/></returns>
        public virtual ITemplate ExecuteUrl(string templateVirtualPath, object model = null, object viewbag = null, bool skipLayout = false, bool throwExceptionOnVirtualPathNotFound = true)
        {
            if (templateVirtualPath == null) throw new ArgumentNullException("templateVirtualPath");

            var instance = Context
                .TemplateFactory
                .CreateTemplateInstance(templateVirtualPath, throwExceptionOnVirtualPathNotFound);

            if (instance == null) return null;

            var template = instance
                .CastTo<ITemplateInternal>()
                .SetModel(model)
                .SetViewBag(viewbag)
                .Execute();

            if (!skipLayout)
                template.TryApplyLayout();

            return template;
        }

        /// <summary>
        /// Renders the specified template content's result with the passed model instance and returns the template's rendered result.
        /// A corresponding virtual path is generated internally for being able to keep the compiled template type cached.
        /// </summary>
        /// <param name="templateContent">Content of the template.</param>
        /// <param name="model">The model</param>
        /// <param name="viewbag">The optional viewbag</param>
        /// <param name="skipLayout">if set to <c>true</c> then any layout setting (probably at _ViewStart) is ignored.</param>
        /// <returns>
        /// The rendered string
        /// </returns>
        public virtual ITemplate ExecuteContent(string templateContent, object model = null, object viewbag = null, bool skipLayout = false) {
            if (templateContent == null) return null;
            var generatedVirtualPath = _templateContentToGeneratedVirtualPathMap.GetOrAdd(templateContent, k =>
                {
                    var path = ContentGeneratedUrlPrefix.FormatWith(Guid.NewGuid().ToString("N")); 
                    RegisterTemplate(path, templateContent);
                    return path;
                }
            );
            return ExecuteUrl(generatedVirtualPath, model, viewbag, skipLayout);
        }

        /// <summary>
        /// Hybrid convenience executer. It decides whether to execute content or an url.
        /// </summary>
        /// <param name="templateVirtualPathOrContent">Content of the template URL or.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewbag">The viewbag.</param>
        /// <param name="skipLayout">if set to <c>true</c> [skip layout].</param>
        /// <param name="throwExceptionOnVirtualPathNotFound">if set to <c>true</c> [throw exception on virtual path not found].</param>
        /// <returns></returns>
        public virtual ITemplate Execute(string templateVirtualPathOrContent, object model = null, object viewbag = null, bool skipLayout = false, bool throwExceptionOnVirtualPathNotFound = true){
            if (templateVirtualPathOrContent == null) throw new ArgumentNullException("templateVirtualPathOrContent");
            return templateVirtualPathOrContent.IsUrl()
                       ? ExecuteUrl(templateVirtualPathOrContent, model, viewbag, skipLayout, throwExceptionOnVirtualPathNotFound)
                       : ExecuteContent(templateVirtualPathOrContent, model, viewbag, skipLayout);
        }

        public RazorContext Context { get; private set; }

        public RazorMachine RegisterTemplate(string virtualPath, string content)
        {
            MemoryContentProvider.RegisterTemplate(NormalizeVirtualPath(virtualPath), content);
            return this;
        }
        public RazorMachine RemoveTemplate(string virtualPath) {
            MemoryContentProvider.RegisterTemplate(NormalizeVirtualPath(virtualPath),null);
            return this;
        }
        public IDictionary<string, string> GetRegisteredInMemoryTemplates() {
            return MemoryContentProvider.GetRegisteredTemplates();
        }

        public RazorMachine ClearTypeCache(){
            Context.TemplateFactory.ClearTypeCache();
            return this;
        }

        protected MemoryContentProvider MemoryContentProvider {
            get {
                var provider = Context.TemplateFactory.ContentManager.TryGetContentProvider<MemoryContentProvider>();
                if (provider == null)
                    Context.TemplateFactory.ContentManager.AddContentProvider(provider = new MemoryContentProvider());
                return provider;
            }
        }

        protected string NormalizeVirtualPath(string virtualPath) {
            if (virtualPath.NullOrEmpty()) throw new ArgumentNullException("virtualPath");
            virtualPath = new VirtualPathBuilder(Context.Config.RootOperator.Path)
                .CombineWith(virtualPath)
                .WithRootOperator()
                .AddOrKeepExtension(Context.Config.Templates.DefaultExtension);
            return virtualPath;
        }


        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            var context = Context;
            Context = null;
            if (context != null)
                context.Dispose();
        }

        #endregion

    }
}
