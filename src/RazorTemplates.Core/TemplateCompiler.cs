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
using Microsoft.VisualBasic;

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

        private static volatile bool _runtimeBinderLoaded;
        private static int _templateNumber;

        public static bool Debug { get; set; }
        public static TemplateCompilationLanguage Language { get; set; }

        internal static TemplateCompilationResult Compile(Type templateType, string templateBody, IEnumerable<string> namespaces, string tempDirectory)
        {
            LoadRuntimeBinder();

            string className;
            var compileUnit = GetCodeCompileUnit(templateType, namespaces, templateBody, out className);

            string sourceCode;
            CodeDomProvider codeProvider;
            switch (Language)
            {
             case TemplateCompilationLanguage.CSharp:
                    codeProvider = new CSharpCodeProvider();
                    break;
                case TemplateCompilationLanguage.VisualBasic:
                    codeProvider = new VBCodeProvider();
                    break;
                default:
                    throw new NotSupportedException("Language not supported.");
            }
            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder, CultureInfo.InvariantCulture))
            {
                codeProvider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());
                sourceCode = builder.ToString();
            }

            var parameters = CreateComplilerParameters(tempDirectory);
            var compileResult = codeProvider.CompileAssemblyFromDom(parameters, compileUnit);
            if (compileResult.Errors != null && compileResult.Errors.Count > 0)
                throw new TemplateCompilationException(compileResult.Errors, sourceCode, templateBody);

            var fullClassName = TEMPLATES_NAMESPACE + "." + className;

            return new TemplateCompilationResult
            {
                Type = compileResult.CompiledAssembly.GetType(fullClassName),
                SourceCode = sourceCode
            };
        }

        private static CompilerParameters CreateComplilerParameters(string tempDirectory)
        {
            var parameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                CompilerOptions = "/target:library /optimize",
            };

            if (Language == TemplateCompilationLanguage.VisualBasic)
            {
                parameters.CompilerOptions += " /optioninfer /optioncompare:text /optionstrict /optionexplicit";
            }

            tempDirectory = string.IsNullOrWhiteSpace(tempDirectory)
                ? GetTempDirectoryFromEnvironment()
                : tempDirectory;

            if (!string.IsNullOrWhiteSpace(tempDirectory))
            {
                tempDirectory = Path.Combine(tempDirectory, Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

                parameters.TempFiles = new TempFileCollection(tempDirectory, false);
            }

            parameters.ReferencedAssemblies.AddRange(GetLoadedAssemblies());

            return parameters;
        }

        private static string GetTempDirectoryFromEnvironment()
        {
            var tempDirectory = Environment.GetEnvironmentVariable("TEMP");
            return !string.IsNullOrEmpty(tempDirectory) ? tempDirectory : Path.GetTempPath();
        }

        private static string[] GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .GroupBy(a => a.FullName)
                .Select(grp => grp.First())
                .Select(a => a.Location)
                .Where(a => !String.IsNullOrWhiteSpace(a))
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
            RazorEngineHost host;
            switch (Language)
            {
                case TemplateCompilationLanguage.CSharp:
                    host = new RazorEngineHost(new CSharpRazorCodeLanguage());
                    break;
                case TemplateCompilationLanguage.VisualBasic:
                    host = new RazorEngineHost(new VBRazorCodeLanguage());
                    break;
                default:
                    throw new NotSupportedException("Language not supported.");
            }

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

        private static void LoadRuntimeBinder()
        {
            if (_runtimeBinderLoaded) return;

            var binderType = typeof(Microsoft.CSharp.RuntimeBinder.Binder);
            var binderAssemblyName = binderType.Assembly.FullName;
            if (string.IsNullOrEmpty(binderAssemblyName)) 
                throw new InvalidOperationException(
                    "Could not load 'Microsoft.CSharp.RuntimeBinder.Binder' assembly.");

            _runtimeBinderLoaded = true;
        }
    }
}