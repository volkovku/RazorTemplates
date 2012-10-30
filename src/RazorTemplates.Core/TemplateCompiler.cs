using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Razor;
using Microsoft.CSharp;

namespace RazorTemplates.Core
{
    internal static class TemplateCompiler
    {
        private const string TEMPLATES_NAMESPACE = "RazorTemplates";
        private static readonly string[] _defaultNamespaces = new[]
        {
            "System",
            "System.Collections.Generic",
            "System.Linq"
        };

        private static int _templateNumber;

        public static bool Debug { get; set; }

        public static Type Compile(Type templateType, string templateBody, IEnumerable<string> namespaces)
        {
            string className;
            var compileUnit = GetCodeCompileUnit(templateType, namespaces, templateBody, out className);

            var codeProvider = new CSharpCodeProvider();
            var sourceCode = string.Empty;
            if (Debug)
            {
                var builder = new StringBuilder();
                using (var writer = new StringWriter(builder, CultureInfo.InvariantCulture))
                {
                    codeProvider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());
                    sourceCode = builder.ToString();
                }
            }

            var compileResult = codeProvider.CompileAssemblyFromDom(CreateComplilerParameters(), compileUnit);
            if (compileResult.Errors != null && compileResult.Errors.Count > 0)
                throw new TemplateCompilationException(compileResult.Errors, sourceCode, templateBody);

            var fullClassName = TEMPLATES_NAMESPACE + "." + className;
            return compileResult.CompiledAssembly.GetType(fullClassName);

        }

        private static CompilerParameters CreateComplilerParameters()
        {
            var parameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                CompilerOptions = "/target:library /optimize",
            };

            var tempFolder = Environment.GetEnvironmentVariable("TEMP");
            if (!string.IsNullOrEmpty(tempFolder))
            {
                tempFolder = Path.Combine(tempFolder, Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

                parameters.TempFiles = new TempFileCollection(tempFolder);
            }

            parameters.ReferencedAssemblies.AddRange(GetLoadedAssemblies());

            return parameters;
        }

        private static string[] GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .GroupBy(a => a.FullName).Select(grp => grp.First())
                .Select(a => a.Location)
                .ToArray();
        }

        private static CodeCompileUnit GetCodeCompileUnit(Type templateType, IEnumerable<string> namespaces, string templateBody, out string className)
        {
            var engine = CreateRazorEngine(templateType, namespaces, out className);

            GeneratorResults results;
            using (var textReader = new StringReader(templateBody))
                results = engine.GenerateCode(textReader);

            return results.GeneratedCode;
        }

        private static RazorTemplateEngine CreateRazorEngine(Type templateType, IEnumerable<string> namespaces, out string className)
        {
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());

            className = "Template_" + GetNextTemplateNumber();

            host.DefaultBaseClass = templateType.FullName;
            host.DefaultNamespace = TEMPLATES_NAMESPACE;
            host.DefaultClassName = className;

            foreach (var ns in _defaultNamespaces.Union(namespaces).Distinct())
                host.NamespaceImports.Add(ns);

            return new RazorTemplateEngine(host);
        }

        private static string GetNextTemplateNumber()
        {
            var number = Interlocked.Increment(ref _templateNumber);
            return number.ToString(CultureInfo.InvariantCulture);
        }
    }
}