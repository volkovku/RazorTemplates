using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorTemplates.Core;

namespace RazorTemplates.Tests.Bugs
{
    [TestClass]
    public class UsingLinqInTemlate
    {
        [TestMethod]
        public void ItShouldAllowToUseLinqInTemplates()
        {
            var template = Template
                .WithBaseType<TemplateBase>()
                .AddNamespace("System.Linq")
                .Compile<List<string>>("Hello @(Model.First())");

            var result = template.Render(new List<string> { "Hello", "World" });
            Assert.AreEqual("Hello Hello", result);
        }
    }
}