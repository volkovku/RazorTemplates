RazorTemplates
==============

Open source templating engine based on Microsoft's Razor parsing engine. Thread safe. Allows run Razor templates outside ASP.Net MVC Projects. 

Get Started
===========

This is very easy to use RazorTemplates:

'''csharp
using System;
using RazorTemplates.Core;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var template = Template.Compile("Hello @Model.Name!");
            Console.WriteLine(template.Render(new { Name = "World" }));
        }
    }
}
'''


