using System.Dynamic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorTemplates.Core;

namespace RazorTemplates.Tests
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
        public void ItShouldSupportAnnonymousObjects()
        {
            var obj = new {Count = 1, Item = "apple"};
            var template = Template.Compile("There is @Model.Count @Model.Item in the box.");

            Assert.AreEqual("There is 1 apple in the box.", template.Render(obj));
        }

  

        [TestMethod]
        public void ItShouldUseTheTypedModel()
        {
            var templateDescription = Template.WithModel<TestModel>();
            var templateStream = Assembly.GetExecutingAssembly()
                                         .GetManifestResourceStream("RazorTemplates.Tests.Template.cshtml");
            var templateContent = new StreamReader(templateStream).ReadToEnd();
            var template = templateDescription.Compile(templateContent);
            var model = new TestModel
                {
                    Message = "Hello world"
                };

            var render = template.Render(model);

            Assert.AreEqual(render, model.Message);
        }
    }
  
}