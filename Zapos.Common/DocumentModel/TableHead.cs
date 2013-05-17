using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class TableHead
    {
        public virtual TableRow[] Rows { get; set; }

        public virtual BaseStyle Style { get; set; }
    }
}