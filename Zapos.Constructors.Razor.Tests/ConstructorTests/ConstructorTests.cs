using System;
using System.Collections.Generic;
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
        [DeploymentItem(@".\Content", "Content")]
        public void ConstructorTableTest()
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

            var constructor = new RazorGridConstructor();
            var table = constructor.CreateTable(@"Content\SimpleReport.cshtml", model);

            Assert.IsNotNull(table.Head);
            Assert.IsNotNull(table.Body);

            Assert.IsTrue(table.Head.Rows.Any());
            Assert.IsTrue(table.Body.Rows.Any());

            Assert.IsTrue(table.Head.Rows.First().Cells.Any());
            Assert.IsTrue(table.Body.Rows.First().Cells.Any());

            Assert.IsNotNull(table.Head.Rows.First().Cells.First().Style);
            Assert.IsNotNull(table.Body.Rows.First().Cells.First().Style);
        }
        [TestMethod]
        [DeploymentItem(@".\Content", "Content")]
        [DeploymentItem(@".\Content\Images", @"Content\Images")]
        public void ConstructorImagesTest()
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

            Func<string, string> pathConverter = s => s.Replace("/", "\\");

            var constructor = new RazorGridConstructor();
            constructor.Init(new Dictionary<string, object> { { "RESOLVE_PATH_ACTION", pathConverter } });
            var table = constructor.CreateTable(@"Content\SimpleReport.cshtml", model);


            Assert.IsNotNull(table.Images);
            Assert.IsTrue(table.Images.Count() == 3);

            Assert.IsTrue(table.Images.All(image => image.ImagePath.EndsWith(".png")));
            Assert.IsTrue(table.Images.All(image => image.Width == 160 && image.Height == 70));
            Assert.IsTrue(table.Images.All(image => image.Left >= 1 && image.Top >= 1));
        }
    }
}