﻿#region  Microsoft Public License
/* This code is part of Xipton.Razor v2.5
 * (c) Jaap Lamfers, 2012 - jaap.lamfers@xipton.net
 * Licensed under the Microsoft Public License (MS-PL) http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
 */
#endregion

using System;
using System.CodeDom;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using Rhythm.Text.Templating.Extension;

namespace Rhythm.Text.Templating.Core.Generator
{
    /// <summary>
    /// SpanCodeGenerator for the model directive
    /// </summary>
    public class SetModelCodeGenerator : SpanCodeGenerator
    {
        public SetModelCodeGenerator(string modelType){
            ModelType = modelType;
        }

        public override void GenerateCode(Span target, CodeGeneratorContext context)
        {
            context.GeneratedClass.BaseTypes.Clear();
            context.GeneratedClass.BaseTypes.Add(new CodeTypeReference(ResolveType(context)));

            #region Work Around
            if (!(context.Host.CodeLanguage is VBRazorCodeLanguage)) 
                context.GeneratedClass.LinePragma = context.GenerateLinePragma(target, CalculatePadding(target, 0));
            //else
                // exclude VBRazorCodeLanguage
                // with VB I found a problem with the #End ExternalSource directive rendered at the GeneratedClass's end while it should not be rendered
                // this only effects the compile error report

            #endregion
        }

        protected virtual string ResolveType(CodeGeneratorContext context)
        {
            var modelType = ModelType.Trim();
            if (context.Host.CodeLanguage is VBRazorCodeLanguage)
                return "{0}(Of {1})".FormatWith(context.Host.DefaultBaseClass, modelType);
            if (context.Host.CodeLanguage is CSharpRazorCodeLanguage)
                return "{0}<{1}>".FormatWith(context.Host.DefaultBaseClass, modelType);
            throw new TemplateException("Code language {0} is not supported.".FormatWith(context.Host.CodeLanguage));
        }

        public string ModelType { get; private set; }

        public override string ToString()
        {
            return "Model:" + ModelType;
        }
        public override bool Equals(object obj)
        {
            var other = obj as SetModelCodeGenerator;
            return other != null && string.Equals(ModelType, other.ModelType, StringComparison.Ordinal);
        }
        public override int GetHashCode()
        {
            return ModelType.GetHashCode();
        }
    }
}