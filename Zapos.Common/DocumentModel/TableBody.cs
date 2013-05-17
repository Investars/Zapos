using System.Collections.Generic;
using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class TableBody
    {
        public virtual IEnumerable<TableRow> Rows
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
