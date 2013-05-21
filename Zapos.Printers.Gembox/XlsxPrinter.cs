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
            var preFile = base.Print(table);
            var stream = new MemoryStream();
            preFile.Save(stream, SaveOptions.XlsxDefault);
            stream.Position = 0;
            return stream;
        }
    }
}