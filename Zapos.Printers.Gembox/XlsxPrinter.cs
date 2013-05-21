using System.IO;
using GemBox.Spreadsheet;
using Zapos.Common.DocumentModel;
using Zapos.Common.Printers;

namespace Zapos.Printers.Gembox
{
    public class XlsxPrinter : InternalPrinter, ITablePrinter
    {
        new public Stream Print(Table table)
        {
            var pdfTable = base.Print(table);

            var stream = new MemoryStream();
            pdfTable.Save(stream, SaveOptions.XlsxDefault);
            stream.Position = 0;
            return stream;
        }
    }
}