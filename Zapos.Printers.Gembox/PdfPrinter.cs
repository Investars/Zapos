using System.Collections.Generic;
using System.IO;
using GemBox.Spreadsheet;
using Zapos.Common.DocumentModel;
using Zapos.Common.Printers;

namespace Zapos.Printers.Gembox
{
    public class PdfPrinter : GemboxUniversalPrinter, ITablePrinter
    {
        new public Stream Print(Table table)
        {
            var preFile = base.Print(table);
            var stream = new MemoryStream();
            preFile.Save(stream, SaveOptions.PdfDefault);
            stream.Position = 0;
            return stream;
        }

        public new void Init(IDictionary<string, object> config)
        {
            base.Init(config);
        }
    }
}