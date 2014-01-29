using System;
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
        protected static ExcelFile Print(IEnumerable<Table> tables)
        {
            var ef = new ExcelFile();

            foreach (var table in tables)
            {
                var worksheetName = table.Name;

                // Work sheet name can't be over 31 characters.
                if (worksheetName.Length > 31)
                {
                    worksheetName = worksheetName.Substring(0, 28) + "...";
                }

                var ws = ef.Worksheets.Add(worksheetName);

                if (table.Head != null)
                {
                    var rowsHeight = CalculateRowsHeight(table.Head.Rows, table.Body.Rows);
                    var columnsWidth = CalculateColumnsWidth(table.Head.Rows, table.Body.Rows);

                    for (var index = 0; index < columnsWidth.Length; index++)
                    {
                        var i = columnsWidth[index];
                        if (i != null)
                        {
                            ws.Columns[index].Width = i.Value * 256;
                        }
                    }

                    for (var index = 0; index < rowsHeight.Length; index++)
                    {
                        var row = rowsHeight[index];
                        if (row.HasValue)
                        {
                            ws.Rows[index].Height = row.Value * 20;
                        }
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
                        var column = columnsWidth[index];
                        if (column != null)
                        {
                            ws.Columns[index].Width = column.Value * 256;
                        }
                    }

                    for (var index = 0; index < rowsHeight.Length; index++)
                    {
                        var row = rowsHeight[index];
                        if (row.HasValue)
                        {
                            ws.Rows[index].Height = row.Value * 20;
                        }
                    }

                    PrintSection(ref ws, table.Body.Rows);
                }

                if (table.Images != null)
                {
                    foreach (var image in table.Images)
                    {
                        ws.Pictures.Add(
                            image.ImagePath,
                            new Rectangle(image.Left, image.Top, image.Width, image.Height));
                    }
                }
            }

            return ef;
        }

        // ReSharper disable once UnusedParameter.Global
        protected void Init(IDictionary<string, object> config)
        {
        }

        private static int?[] CalculateColumnsWidth(params TableRow[][] rowsCollections)
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
                                        var width = row.Cells[index].Style.Style.Width;
                                        if (!width.HasValue)
                                        {
                                            return null;
                                        }

                                        return width;
                                    }
                                    catch (IndexOutOfRangeException)
                                    {
                                        return null;
                                    }
                                }))
                            .ToArray();

            return result;
        }

        private static int?[] CalculateRowsHeight(params TableRow[][] rowsCollections)
        {
            var rows = rowsCollections.SelectMany(collection => collection).ToArray();
            var result = new List<int?>();

            foreach (var row in rows)
            {
                var cellsWithSetHeight = row.Cells.AsParallel().Where(cell => cell.Style.Style.Height.HasValue);
                if (cellsWithSetHeight.Any())
                {
                    result.Add(cellsWithSetHeight.Max(cell => (int)cell.Style.Style.Height));
                }
                else
                {
                    result.Add(null);
                }
            }

            return result.ToArray();
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

            if (style.Style.BorderTopColor.HasValue && style.Style.BorderTopStyle.HasValue)
            {
                resultStyle.Borders.SetBorders(
                    MultipleBorders.Top,
                    style.Style.BorderTopColor,
                    ConvertLineStyle(style.Style.BorderTopStyle));
            }

            if (style.Style.BorderRightColor.HasValue && style.Style.BorderRightStyle.HasValue)
            {
                resultStyle.Borders.SetBorders(
                    MultipleBorders.Right,
                    style.Style.BorderRightColor,
                    ConvertLineStyle(style.Style.BorderRightStyle));
            }

            if (style.Style.BorderBottomColor.HasValue && style.Style.BorderBottomStyle.HasValue)
            {
                resultStyle.Borders.SetBorders(
                    MultipleBorders.Bottom,
                    style.Style.BorderBottomColor,
                    ConvertLineStyle(style.Style.BorderBottomStyle));
            }

            if (style.Style.BorderLeftColor.HasValue && style.Style.BorderLeftStyle.HasValue)
            {
                resultStyle.Borders.SetBorders(
                    MultipleBorders.Left,
                    style.Style.BorderLeftColor,
                    ConvertLineStyle(style.Style.BorderLeftStyle));
            }

            SetValueHelper(style.Style.BackgroundColor, value =>
            {
                resultStyle.FillPattern.PatternStyle = FillPatternStyle.Solid;
                resultStyle.FillPattern.PatternForegroundColor = value;
            });

            SetValueHelper(style.Style.Color, value => { resultStyle.Font.Color = value; });
            SetValueHelper(style.Style.IsItalic, value => { resultStyle.Font.Italic = value; });
            SetValueHelper(style.Style.Font, value => { resultStyle.Font.Name = value; });
            SetValueHelper(style.Style.FontSize, value => { resultStyle.Font.Size = value * 20; });
            SetValueHelper(style.Style.IsLineThrough, value => { resultStyle.Font.Strikeout = value; });
            SetValueHelper(style.Style.IsUnderline, value => { resultStyle.Font.UnderlineStyle = value ? UnderlineStyle.Single : UnderlineStyle.None; });
            SetValueHelper(style.Style.IsBold, value => { resultStyle.Font.Weight = value ? ExcelFont.BoldWeight : ExcelFont.NormalWeight; });

            if (style.Style.HAlign.HasValue)
            {
                switch (style.Style.HAlign.Value)
                {
                    case HAlign.Center:
                    resultStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    break;

                    case HAlign.Justify:
                    resultStyle.HorizontalAlignment = HorizontalAlignmentStyle.Justify;
                    break;

                    case HAlign.Left:
                    resultStyle.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    break;

                    case HAlign.Right:
                    resultStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    break;
                }
            }

            if (style.Style.VAlign.HasValue)
            {
                switch (style.Style.VAlign.Value)
                {
                    case VAlign.Bottom:
                    resultStyle.VerticalAlignment = VerticalAlignmentStyle.Bottom;
                    break;

                    case VAlign.Middle:
                    resultStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                    break;

                    case VAlign.Top:
                    resultStyle.VerticalAlignment = VerticalAlignmentStyle.Top;
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(style.NumberFormat))
            {
                resultStyle.NumberFormat = style.NumberFormat;
            }

            resultStyle.WrapText = true;

            return resultStyle;
        }

        private static LineStyle ConvertLineStyle(BorderStyle borderStyle)
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

        private static void SetValueHelper<T>(InheritStyle<T> value, Action<T> action)
        {
            if (value.HasValue)
            {
                action(value.Value);
            }
        }
    }
}
