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
            var tableModel = new Table();

            var printerConfig = new Dictionary<string, object>
                {
                    { "LICENSE_KEY", "FREE-LIMITED-KEY" }
                };

            var printer = new PdfPrinter();
            printer.Init(printerConfig);

            try
            {
                using (var fileStream = new FileStream("test.pdf", FileMode.CreateNew))
                {
                    printer.Print(fileStream, tableModel);
                }

                var bytes = File.ReadAllBytes("test.pdf");

                Assert.Greater(bytes.Length, 1024);
            }
            finally
            {
                if (File.Exists("test.pdf"))
                {
                    File.Delete("test.pdf");
                }
            }
        }
    }
}
