using System;
using System.IO;

using Zapos.Common.Constructors;
using Zapos.Common.Printers;

namespace Zapos.Common
{
    public class Report<TGridConstructor, TPrinter>
        where TGridConstructor : IGridConstructor, new()
        where TPrinter : ITablePrinter, new()
    {
        public Stream Create<T>(string filePath, T model)
        {
            var construnctor = new TGridConstructor();
            var table = construnctor.CreateTable(filePath, model);
            var printer = new TPrinter();
            var stream = printer.Print(table);

            return stream;
        }
    }
}