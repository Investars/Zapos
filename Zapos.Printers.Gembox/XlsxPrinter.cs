using System.Collections.Generic;
using System.IO;

using GemBox.Spreadsheet;

using Zapos.Common.DocumentModel;
using Zapos.Common.Printers;

namespace Zapos.Printers.Gembox
{
    public class XlsxPrinter : GemboxUniversalPrinter, ITablePrinter
    {
        public void Print(Stream stream1, Table table)
        {
            var preFile = Print(table);
            var stream = new MemoryStream();
            preFile.Save(stream, SaveOptions.XlsxDefault);
        }

        public new void Init(IDictionary<string, object> config)
        {
            base.Init(config);
        }
    }
}
