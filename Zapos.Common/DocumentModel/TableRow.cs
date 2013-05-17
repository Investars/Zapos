using System.Collections.Generic;
using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class TableRow
    {
        public virtual IEnumerable<TableCell> Cells
        {
            get;
            set;
        }

        public virtual BaseStyle? Style
        {
            get;
            set;
        }
    }
}