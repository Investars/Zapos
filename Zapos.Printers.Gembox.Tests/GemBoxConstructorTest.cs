using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zapos.Common.DocumentModel;

namespace Zapos.Printers.Gembox.Tests
{
    [TestClass]
    public class GemBoxConstructorTest
    {
        [TestMethod]
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

                Assert.IsTrue(bytes.Length > 1024);
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
