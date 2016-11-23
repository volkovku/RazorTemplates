using System;
using Rhythm.Text;
using Rhythm.Text.Config;

namespace Rhythm.Samples
{
    class Program
    {
        static void Main()
        {
            SimpleTemplate();
            TemplateWithCustomNamespaces();

            Console.ReadLine();
        }

        public static void SimpleTemplate()
		{
            var template = CompiledApi.Compile("Hello @Model.Name!");
            Console.WriteLine(template.Render(new { Name = "world" }));
        }

        public static void TemplateWithCustomNamespaces()
        {
            var template = CompiledApi
                .WithBaseType<TemplateBase>()
                .AddNamespace("Rhythm.Samples")
                .Compile(@"There is @Model.Apples @Plural.Form(Model.Apples, new [] { ""apple"", ""apples"" }) in the box.");

            Console.WriteLine(template.Render(new { Apples = 1 }));
            Console.WriteLine(template.Render(new { Apples = 2 }));
        }
    }

    public static class Plural
    {
        public static string Form(int value, string[] forms)
        {
            var form = value == 1 ? 0 : 1;
            return forms[form];
        }
    }
}
