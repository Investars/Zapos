using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class TableRow
    {
        public virtual TableCell[] Cells { get; set; }

        public virtual BaseStyle Style { get; set; }
    }
}