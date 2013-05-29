using System;
using System.Collections.Generic;
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

            Func<string, string> resolvePath = s => s;

            var constructorConfig = new Dictionary<string, object>
                {
                    {"RESOLVE_PATH_ACTION", resolvePath}
                };

            var printerConfig = new Dictionary<string, object>
                {
                    {"LICENSE_KEY", "FREE-LICENSE-KEY"}
                };

            var report = new Report<RazorGridConstructor, PdfPrinter>(constructorConfig, printerConfig);
            var stream = report.Create(filePath, model);

            Assert.NotNull(stream);
        }
    }
}
