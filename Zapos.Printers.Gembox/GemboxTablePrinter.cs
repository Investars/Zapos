using System.Collections.Generic;
using System.IO;
using System.Linq;

using GemBox.Spreadsheet;
using Zapos.Common.DocumentModel;
using Zapos.Common.Printers;
using Zapos.Common.Styles;
using CellStyle = Zapos.Common.Styles.CellStyle;
using GemBoxCellStyle = GemBox.Spreadsheet.CellStyle;

namespace Zapos.Printers.Gembox
{
    public class GemboxTablePrinter : ITablePrinter
    {
        public Stream Print(Table table)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var ef = new ExcelFile();
            var ws = ef.Worksheets.Add("Report");

            //AutoFit(ref ws, 4);
            PrintHead(ref ws, table.Head.Rows.ToList());
            PrintBody(ref ws, table.Body.Rows.ToList(), table.Head.Rows.Count());

            var stream = new MemoryStream();
            ef.Save(stream, SaveOptions.PdfDefault);
            ef.Save("Export.xlsx");
            ef.Save("Export.pdf");
            stream.Position = 0;
            return stream;
        }

        private void AutoFit(ref ExcelWorksheet ws, int columnCount)
        {
            for (var i = 0; i < columnCount; i++)
            {
                ws.Columns[i].AutoFit();
            }
        }

        private void PrintBody(ref ExcelWorksheet ws, IEnumerable<TableRow> rows, int headRow)
        {
            PrintSection(ref ws, rows, headRow);
        }

        private void PrintHead(ref ExcelWorksheet ws, IEnumerable<TableRow> rows)
        {
            PrintSection(ref ws, rows, 0);
        }

        private void PrintSection(ref ExcelWorksheet ws, IEnumerable<TableRow> rows, int rowBegin)
        {
            var rowsInternal = rows.ToArray();

            var rowBeginPosition = rowBegin;
            var columnBeginPosition = 0;

            for (var rowIndex = 0; rowIndex < rowsInternal.Length; rowIndex++)
            {
                var row = rowsInternal[rowIndex];
                var cells = row.Cells.ToArray();

                for (var columnIndex = 0; columnIndex < cells.Length; columnIndex++)
                {
                    var cell = cells[columnIndex];

                    ws.Cells[rowBeginPosition + rowIndex, columnBeginPosition + columnIndex]
                      .Value = cell.Value;

                    ws.Cells[rowBeginPosition + rowIndex, columnBeginPosition + columnIndex]
                        .Style = ConvertStyle(cell.Style);
                }
            }
        }

        private static GemBoxCellStyle ConvertStyle(CellStyle style)
        {
            var resultStyle = new GemBoxCellStyle();
            resultStyle.Borders.SetBorders(MultipleBorders.Top, style.Style.BorderTopColor, ConvertLineStyle(style.Style.BorderTopStyle));
            resultStyle.Borders.SetBorders(MultipleBorders.Right, style.Style.BorderRightColor, ConvertLineStyle(style.Style.BorderRightStyle));
            resultStyle.Borders.SetBorders(MultipleBorders.Bottom, style.Style.BorderBottomColor, ConvertLineStyle(style.Style.BorderBottomStyle));
            resultStyle.Borders.SetBorders(MultipleBorders.Left, style.Style.BorderLeftColor, ConvertLineStyle(style.Style.BorderLeftStyle));

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
                case null:
                    return LineStyle.None;
                default:
                    return LineStyle.Medium;
            }
        }
    }
}
