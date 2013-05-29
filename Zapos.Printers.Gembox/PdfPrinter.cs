using System.Collections.Generic;
using System.IO;

using GemBox.Spreadsheet;

using Zapos.Common.DocumentModel;
using Zapos.Common.Printers;

namespace Zapos.Printers.Gembox
{
    public class PdfPrinter : GemboxUniversalPrinter, ITablePrinter
    {
        public void Print(Stream stream, Table table)
        {
            var preFile = Print(table);
            preFile.Save(stream, SaveOptions.PdfDefault);
        }

        public new void Init(IDictionary<string, object> config)
        {
            base.Init(config);
        }
    }
}