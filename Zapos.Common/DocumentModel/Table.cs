using System.Collections.Generic;
using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class Table
    {
        public string Name { get; set; }

        public IEnumerable<TableImage> Images { get; set; }

        public virtual TableBody Body { get; set; }

        public virtual TableHead Head { get; set; }

        public virtual BaseStyle Style { get; set; }
    }
}