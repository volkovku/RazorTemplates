using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhythm.Text;

namespace Rhythm.Tests.Bugs
{
    [TestClass]
    public class AddingExtraPeriodToSomeUrls
    {
        [TestMethod]
        public void ItShouldNotAddExtraPeriodToUrls()
        {
            var images = new[] { "jazz", "coffee", "box", "cup" };

            const string templateSource = "<img src=\"http://sub.domain.com/@(Model).png\">";
            var template = Template.Compile(templateSource);

            foreach (var image in images)
            {
                var expected = string.Format("<img src=\"http://sub.domain.com/{0}.png\">", image);
                var actual = template.Render(image);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}