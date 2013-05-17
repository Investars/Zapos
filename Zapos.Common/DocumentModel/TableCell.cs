using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class TableCell
    {
        public virtual string Value
        {
            get;
            set;
        }

        public virtual CellStyle Style
        {
            get;
            set;
        }
    }
}