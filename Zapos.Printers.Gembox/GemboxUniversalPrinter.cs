using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GemBox.Spreadsheet;
using Zapos.Common.DocumentModel;
using Zapos.Common.Styles;

using CellStyle = Zapos.Common.Styles.CellStyle;
using GemBoxCellStyle = GemBox.Spreadsheet.CellStyle;

namespace Zapos.Printers.Gembox
{
    public abstract class GemboxUniversalPrinter
    {
        private IDictionary<string, object> _config;

        private string _licenseKey;

        protected ExcelFile Print(Table table)
        {
            SpreadsheetInfo.SetLicense(_licenseKey);

            var ef = new ExcelFile();
            var ws = ef.Worksheets.Add("Report");

            if (table.Head != null)
            {
                var rowsHeight = CalculateRowsHeight(table.Head.Rows, table.Body.Rows);
                var columnsWidth = CalculateColumnsWidth(table.Head.Rows, table.Body.Rows);

                for (var index = 0; index < columnsWidth.Length; index++)
                {
                    ws.Columns[index].Width = columnsWidth[index] * 256;
                }

                for (var index = 0; index < rowsHeight.Length; index++)
                {
                    ws.Rows[index].Height = rowsHeight[index] * 20;
                }

                PrintSection(ref ws, table.Head.Rows);
                PrintSection(ref ws, table.Body.Rows, table.Head.Rows.Length);
            }
            else
            {
                var rowsHeight = CalculateRowsHeight(table.Body.Rows);
                var columnsWidth = CalculateColumnsWidth(table.Body.Rows);

                for (var index = 0; index < columnsWidth.Length; index++)
                {
                    ws.Columns[index].Width = columnsWidth[index] * 256;
                }

                for (var index = 0; index < rowsHeight.Length; index++)
                {
                    ws.Rows[index].Height = rowsHeight[index] * 20;
                }

                PrintSection(ref ws, table.Body.Rows);
            }

            foreach (var image in table.Images)
            {
                ws.Pictures.Add(image.ImagePath, new Rectangle(image.Left, image.Top, image.Width, image.Height));
            }

            return ef;
        }

        protected void Init(IDictionary<string, object> config)
        {
            _config = config;
            _licenseKey = (string)_config["LICENSE_KEY"];
        }

        private static int[] CalculateColumnsWidth(params TableRow[][] rowsCollections)
        {
            var rows = rowsCollections.SelectMany(collection => collection).ToArray();

            var columnCount = rows.Max(row => row.Cells.Length);

            var parallelRows = rows.AsParallel();

            var result = Enumerable
                            .Range(0, columnCount)
                            .Select(index => parallelRows.Max(row =>
                                {
                                    try
                                    {
                                        return row.Cells[index].Style.Style.Width;
                                    }
                                    catch (System.IndexOutOfRangeException)
                                    {
                                        return 0;
                                    }
                                }))
                            .ToArray();

            return result;
        }

        private static int[] CalculateRowsHeight(params TableRow[][] rowsCollections)
        {
            var rows = rowsCollections.SelectMany(collection => collection).ToArray();
            var result = rows.Select(row => row.Cells.AsParallel().Max(cell => (int)cell.Style.Style.Height)).ToArray();

            return result;
        }

        private static void PrintSection(ref ExcelWorksheet ws, IEnumerable<TableRow> rows, int rowBegin = 0)
        {
            var rowsInternal = rows.ToArray();
            var rowBeginPosition = rowBegin;

            for (var rowIndex = 0; rowIndex < rowsInternal.Length; rowIndex++)
            {
                var row = rowsInternal[rowIndex];
                var cells = row.Cells.ToArray();
                var columnIndex = 0;

                foreach (var cell in cells)
                {
                    if (ws.Cells[rowBeginPosition + rowIndex, columnIndex].MergedRange == null)
                    {
                        if (cell.Style.Colspan > 0 || cell.Style.Rowspan > 0)
                        {
                            var firstRow = rowBeginPosition + rowIndex;
                            var rowsRange = (cell.Style.Rowspan != 0) ? cell.Style.Rowspan - 1 : cell.Style.Rowspan;
                            var firstColumn = columnIndex;
                            var columnsRange = (cell.Style.Colspan != 0) ? cell.Style.Colspan - 1 : cell.Style.Colspan;

                            ws.Cells
                                .GetSubrangeAbsolute(firstRow, firstColumn, firstRow + rowsRange, firstColumn + columnsRange)
                                .Merged = true;
                        }
                    }
                    else
                    {
                        while (ws.Cells[rowBeginPosition + rowIndex, columnIndex].MergedRange != null)
                        {
                            columnIndex++;
                        }
                    }

                    ws.Cells[rowBeginPosition + rowIndex, columnIndex]
                        .Value = cell.Value;

                    ws.Cells[rowBeginPosition + rowIndex, columnIndex]
                        .Style = ConvertStyle(cell.Style);

                    ws.Cells[rowBeginPosition + rowIndex, columnIndex]
                        .Style.WrapText = true;

                    columnIndex++;
                }
            }
        }

        private static GemBoxCellStyle ConvertStyle(CellStyle style)
        {
            var resultStyle = new GemBoxCellStyle();
            resultStyle.Borders.SetBorders(
                MultipleBorders.Top,
                style.Style.BorderTopColor,
                ConvertLineStyle(style.Style.BorderTopStyle));

            resultStyle.Borders.SetBorders(
                MultipleBorders.Right,
                style.Style.BorderRightColor,
                ConvertLineStyle(style.Style.BorderRightStyle));

            resultStyle.Borders.SetBorders(
                MultipleBorders.Bottom,
                style.Style.BorderBottomColor,
                ConvertLineStyle(style.Style.BorderBottomStyle));

            resultStyle.Borders.SetBorders(
                MultipleBorders.Left,
                style.Style.BorderLeftColor,
                ConvertLineStyle(style.Style.BorderLeftStyle));

            resultStyle.FillPattern.PatternStyle = FillPatternStyle.Solid;
            resultStyle.FillPattern.PatternForegroundColor = style.Style.BackgroundColor;

            resultStyle.Font.Color = style.Style.Color.Value;
            resultStyle.Font.Italic = style.Style.IsItalic;
            resultStyle.Font.Name = style.Style.Font;
            resultStyle.Font.Size = style.Style.FontSize * 20;
            resultStyle.Font.Strikeout = style.Style.IsLineThrough;
            resultStyle.Font.UnderlineStyle = style.Style.IsUnderline ? UnderlineStyle.Single : UnderlineStyle.None;
            resultStyle.Font.Weight = style.Style.IsBold ? ExcelFont.BoldWeight : ExcelFont.NormalWeight;

            resultStyle.HorizontalAlignment = (HorizontalAlignmentStyle)style.Style.HAlign.Value;
            resultStyle.VerticalAlignment = (VerticalAlignmentStyle)style.Style.VAlign.Value;

            resultStyle.NumberFormat = style.NumberFormat;
            resultStyle.WrapText = true;

            return resultStyle;
        }

        private static LineStyle ConvertLineStyle(BorderStyle? borderStyle)
        {
            switch (borderStyle)
            {
                case BorderStyle.Dashed:
                    return LineStyle.Dashed;
                case BorderStyle.Dotted:
                    return LineStyle.Dotted;
                case BorderStyle.Double:
                    return LineStyle.Double;
                case BorderStyle.Solid:
                    return LineStyle.Thin;
                default:
                    return LineStyle.None;
            }
        }
    }
}
