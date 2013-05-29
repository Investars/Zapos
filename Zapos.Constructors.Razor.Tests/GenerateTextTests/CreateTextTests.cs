using System;
using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zapos.Constructors.Razor.Generators;
using Zapos.Constructors.Razor.Tests.TestModels;

namespace Zapos.Constructors.Razor.Tests.GenerateTextTests
{
    [TestClass]
    public class CreateTextTests
    {
        [TestMethod]
        [DeploymentItem(@".\Content", "Content")]
        public void GenerateTextTest()
        {
            var rnd = new Random();

            var model = new TestReportModel
                {
                    Items = Enumerable.Range(0, 50).Select(id => new TestReportItemModel
                        {
                            Id = id,
                            Name = Guid.NewGuid().ToString(),
                            Value = rnd.Next(1000000, 10000000) / 1000.0
                        })
                };

            var generator = new RazorTextGenerator();

            var content = generator.Generate(@"Content\SimpleReport.cshtml", model);

            var document = XDocument.Parse("<body>" + content + "</body>");

            // ReSharper disable PossibleNullReferenceException
            var style = document.Root.Element("style");
            var table = document.Root.Element("table");
            // ReSharper restore PossibleNullReferenceException

            Assert.IsFalse(string.IsNullOrEmpty(content), "Content is null or empty");
            Assert.IsNotNull(style, "Cant find 'style' tag");
            Assert.IsNotNull(table, "Cant find 'table' tag");
        }
    }
}