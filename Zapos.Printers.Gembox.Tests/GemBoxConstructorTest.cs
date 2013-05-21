using System;
using System.IO;
using System.Linq;

using NUnit.Framework;
using Zapos.Common;
using Zapos.Constructors.Razor.Constructors;
using Zapos.Printers.Gembox.Tests.TestModels;

namespace Zapos.Printers.Gembox.Tests
{
    [TestFixture]
    public class GemBoxConstructorTest
    {
        [Test]
        public void ConstructorTest()
        {
            var rnd = new Random();

            var filePath = Path.Combine("Content", "SimpleReport.cshtml");
            var model = new TestReportModel
            {
                Items = Enumerable.Range(0, 50).Select(id => new TestReportItemModel
                {
                    Id = id,
                    Name = Guid.NewGuid().ToString(),
                    Value = rnd.Next(1000000, 10000000) / 1000.0
                })
            };

            var report = new Report<RazorGridConstructor, PdfPrinter>();
            var stream = report.Create(filePath, model);

            Assert.NotNull(stream);
        }
    }
}
