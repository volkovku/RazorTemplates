
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Generator;

namespace RazorTemplates.Core
{
	/// <summary>
	/// The engine host is the part where the code rendering takes place.
	/// </summary>
	public class CustomEngineHost : RazorEngineHost
	{

		GeneratedClassContext _generatedClassContext;
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomEngineHost"/> class.
		/// </summary>
		/// <param name="config">The config holds all settings that are needed to initialzie the host.</param>
		public CustomEngineHost(RazorCodeLanguage codeLanguage)
			: base(codeLanguage)
		{
			//if (config == null) throw new ArgumentNullException("config");

			/*
using System;

public GeneratedClassContext (string executeMethodName, string writeMethodName, string writeLiteralMethodName, string writeToMethodName, string writeLiteralToMethodName, string templateTypeName, string defineSectionMethodName);

			*/
			// defining sections inside the template
			_generatedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral", "WriteTo", "WriteLiteralTo", typeof(HelperResult).FullName, "DefineSection")
			{
				ResolveUrlMethodName = "ResolveUrl",
			};
			//GeneratedClassContext.TemplateTypeName = typeof(HelperResult).FullName;
		}

		public override GeneratedClassContext GeneratedClassContext
		{
		get { return _generatedClassContext; }
			set { _generatedClassContext = value; }
		}

	}
}