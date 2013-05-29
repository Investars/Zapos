using System;
using System.Collections.Generic;
using System.IO;

using Zapos.Common.Constructors;
using Zapos.Common.Printers;

namespace Zapos.Common
{
    public class Report<TGridConstructor, TPrinter>
        where TGridConstructor : IGridConstructor, new()
        where TPrinter : ITablePrinter, new()
    {
        private readonly IDictionary<string, object> _constructorConfig;
        private readonly IDictionary<string, object> _printerConfig;

        public Report(
            IDictionary<string, object> constructorConfig,
            IDictionary<string, object> printerConfig)
        {
            _constructorConfig = constructorConfig;
            _printerConfig = printerConfig;
        }


        public Stream Create<T>(string filePath, T model)
        {
            var construnctor = new TGridConstructor();
            if (_constructorConfig != null)
            {
                construnctor.Init(_constructorConfig);
            }

            var table = construnctor.CreateTable(filePath, model);

            var printer = new TPrinter();

            if (_printerConfig != null)
            {
                printer.Init(_printerConfig);
            }
            var stream = printer.Print(table);

            return stream;
        }
    }
}