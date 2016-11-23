using System;
using Rhythm.Text.Templating;
using Rhythm.Text.Templating.Config;

namespace Rhythm.Samples
{
    class Program
    {
        static void Main()
        {
			SimpleTemplate();
            TemplateWithCustomNamespaces();
			HelperTemplate();
            Console.ReadLine();
        }



		public static void HelperTemplate()
		{
			
			var template = TemplateUtility.Compile(@"
@helper h1(string str){
	<span attr='ahahha@(System.DateTime.Now) @(str)'>helper out @(str) </span>
}			

Hello @Model.Name!
hello helper @h1(""ssssssss"")
");
			Console.WriteLine(template.Render(new { Name = "world" }));
		}




        public static void SimpleTemplate()
		{
            var template = TemplateUtility.Compile("Hello @Model.Name!");
            Console.WriteLine(template.Render(new { Name = "world" }));
        }

        public static void TemplateWithCustomNamespaces()
        {
            var template = TemplateUtility
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
