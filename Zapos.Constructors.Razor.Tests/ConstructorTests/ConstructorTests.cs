using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zapos.Constructors.Razor.Constructors;
using Zapos.Constructors.Razor.Tests.TestModels;

namespace Zapos.Constructors.Razor.Tests.ConstructorTests
{
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
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

            Assert.IsNotNull(table.Head);
            Assert.IsNotNull(table.Body);

            Assert.IsTrue(table.Head.Rows.Any());
            Assert.IsTrue(table.Body.Rows.Any());

            Assert.IsTrue(table.Head.Rows.First().Cells.Any());
            Assert.IsTrue(table.Body.Rows.First().Cells.Any());

            Assert.IsNotNull(table.Head.Rows.First().Cells.First().Style);
            Assert.IsNotNull(table.Body.Rows.First().Cells.First().Style);
        }
    }
}