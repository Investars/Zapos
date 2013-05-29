using System.Drawing;

namespace Zapos.Common.Styles
{
    public struct CellStyle
    {
        public CellStyle(BaseStyle cellStyle, string formula, string numberFormat, string title)
            : this()
        {
            Style = cellStyle;
            Formula = formula;
            NumberFormat = numberFormat;
            Title = title;
        }

        public BaseStyle Style { get; set; }

        public string Formula { get; set; }

        public string NumberFormat { get; set; }

        public string Title { get; set; }
    }
}