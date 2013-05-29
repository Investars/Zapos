using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using Zapos.Common.DocumentModel;

namespace Zapos.Printers.Gembox.Tests
{
    [TestFixture]
    public class GemBoxConstructorTest
    {
        [Test]
        public void ConstructorTest()
        {

            var tableModel = new Table
                {

                };

            var printer = new PdfPrinter();

            printer.Init(new Dictionary<string, object> { { "LICENSE_KEY", "FREE-LICENSE-KEY" } });

            using (Stream stream = new FileStream("test.pdf", FileMode.Create))
            {
                printer.Print(stream, tableModel);

                Assert.AreNotEqual(stream.Length, 0);
            }

            File.Delete("test.pdf");
        }
    }
}
