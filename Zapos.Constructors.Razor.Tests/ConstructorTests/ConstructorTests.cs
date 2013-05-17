using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Zapos.Constructors.Razor.Constructors;
using Zapos.Constructors.Razor.Tests.TestModels;

namespace Zapos.Constructors.Razor.Tests.ConstructorTests
{
    [TestFixture]
    public class ConstructorTests
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

            var constructor = new RazorGridConstructor();
            var table = constructor.CreateTable(filePath, model);

            Assert.NotNull(table.Head);
            Assert.NotNull(table.Body);

            Assert.IsNotEmpty(table.Head.Rows);
            Assert.IsNotEmpty(table.Body.Rows);

            Assert.IsNotEmpty(table.Head.Rows.First().Cells);
            Assert.IsNotEmpty(table.Body.Rows.First().Cells);

            Assert.IsNotNull(table.Head.Rows.First().Cells.First().Style);
            Assert.IsNotNull(table.Body.Rows.First().Cells.First().Style);
        }
    }
}