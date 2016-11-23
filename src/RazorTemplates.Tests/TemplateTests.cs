using System;
using System.Dynamic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhythm.Text;

namespace Rhythm.Tests
{
    [TestClass]
    public class TemplateTests
    {
        [TestMethod]
        public void ItShouldRenderExpandoObjects()
        {
            dynamic expando = new ExpandoObject();
            var template = Template.Compile("There is @Model.Count @Model.Item in the box.");

            expando.Count = 1;
            expando.Item = "apple";

            Assert.AreEqual("There is 1 apple in the box.", template.Render(expando));

            expando.Count = 2;
            expando.Item = "apples";

            Assert.AreEqual("There is 2 apples in the box.", template.Render(expando));
        }

        [TestMethod]
        public void ItShouldSupportAnonymousObjects()
        {
            var obj = new {Count = 1, Item = "apple"};
            var template = Template.Compile("There is @Model.Count @Model.Item in the box.");

            Assert.AreEqual("There is 1 apple in the box.", template.Render(obj));
        }
    
        [TestMethod]
        public void ItShouldUseTheTypedModel()
        {
            var templateStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("RazorTemplates.Tests.Template.cshtml");

            var templateContent = new StreamReader(templateStream).ReadToEnd();
            var template = Template
                .WithBaseType<TemplateBase>()
                .AddAssemblies("Newtonsoft.Json.dll")
                .AddNamespace("Newtonsoft.Json")
                .Compile<TestModel>(templateContent);
            var model = new TestModel { Message = "Hello world" };

            Assert.AreEqual(model.Message + "\r\n{\"Message\":\"Hello world\"}\r\n", template.Render(model));
        }

        [TestMethod]
        public void ItShouldLoadAssemblies()
        {
            var obj = new { Count = 1, Item = "apple" };
            var template = Template
                .WithBaseType<TemplateBase>()
                .AddAssemblies("Newtonsoft.Json.dll")
                .AddNamespace("Newtonsoft.Json")
                .Compile("There is @Model.Count @Model.Item in the box. @JsonConvert.SerializeObject(@Model)");

            Assert.AreEqual("There is 1 apple in the box. {\"Count\":1,\"Item\":\"apple\"}", template.Render(obj));
        }
    }
}