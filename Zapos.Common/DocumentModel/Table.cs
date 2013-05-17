using Zapos.Common.Styles;

namespace Zapos.Common.DocumentModel
{
    public class Table
    {
        public virtual TableBody Body { get; set; }

        public virtual TableHead Head { get; set; }

        public virtual BaseStyle Style { get; set; }
    }
}