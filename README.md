RazorTemplates
==============

Open source templating engine based on Microsoft's Razor parsing engine. Thread safe. Allows to run Razor templates outside of ASP.NET projects.

## Install ##

To install RazorTemplates, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console):
```
PM> Install-Package RazorTemplates
```

## Get Started ##

Using the library is as simple as two lines of code:

```csharp
using RazorTemplates.Core;

var template = Template.Compile("Hello @Model.Name!");
Console.WriteLine(template.Render(new { Name = "World" }));
```

## Extending with custom methods ##

You can extend templates by including required namespaces:

```csharp
using System;
using RazorTemplates.Core;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var template = Template
                .WithBaseType<TemplateBase>()
                .AddNamespace("TestApplication")
                .Compile(@"@Model.Apples @Plural.Form(Model.Apples, new [] { ""apple"", ""apples"" }) in the box.");

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
```
