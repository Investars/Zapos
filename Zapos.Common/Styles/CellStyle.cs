namespace Zapos.Common.Styles
{
    public struct CellStyle
    {
        public CellStyle(BaseStyle cellStyle, string formula, string numberFormat, string title, int colspan, int rowspan)
            : this()
        {
            Style = cellStyle;
            Formula = formula;
            NumberFormat = numberFormat;
            Title = title;
            Colspan = colspan;
            Rowspan = rowspan;
        }

        public BaseStyle Style { get; set; }

        public string Formula { get; set; }

        public string NumberFormat { get; set; }

        public string Title { get; set; }

        public int Colspan { get; set; }

        public int Rowspan { get; set; }

        //public Image Image { get; set; }
    }
}