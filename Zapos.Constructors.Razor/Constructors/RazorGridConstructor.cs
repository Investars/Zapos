using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Zapos.Common.Constructors;
using Zapos.Common.DocumentModel;
using Zapos.Common.Styles;

using Zapos.Constructors.Razor.Generators;
using Zapos.Constructors.Razor.Parsers;

namespace Zapos.Constructors.Razor.Constructors
{
    public class RazorGridConstructor : IGridConstructor
    {
        public Table CreateTable<TModel>(string filePath, TModel model)
        {
            var generator = new RazorTextGenerator();

            var content = generator.Generate(filePath, model);

            var document = XDocument.Parse("<body>" + content + "</body>");

            // ReSharper disable PossibleNullReferenceException

            var tableTag = document.Root.Element("table");

            var styleTag = document.Root.Elements("style").FirstOrDefault();

            // ReSharper restore PossibleNullReferenceException

            IDictionary<string, BaseStyle> classes;

            if (styleTag != null)
            {
                var parser = new CssParser();
                classes = parser.ParceCss(styleTag.Value);
            }
            else
            {
                classes = new Dictionary<string, BaseStyle>();
            }

            var table = ParseTable(tableTag, classes);

            return table;
        }

        private static IEnumerable<BaseStyle> GetStylesByClasses(IEnumerable<KeyValuePair<string, BaseStyle>> classes, XAttribute attrClass)
        {
            return attrClass
                .Value
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Join(
                    classes,
                    s => s,
                    s => s.Key,
                    (s, cssClass) => cssClass.Value);
        }

        private static Table ParseTable(XElement tableTag, IDictionary<string, BaseStyle> classes)
        {
            var tableStyle = StyleFactory.DefaultStyle;
            var attrClass = tableTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var tableClasses = GetStylesByClasses(classes, attrClass);
                tableStyle = StyleFactory.MergeStyles(tableClasses);
            }

            TableBody tableBody;
            var tableBodyTag = tableTag.Elements("tbody").FirstOrDefault();

            if (tableBodyTag != null)
            {
                tableBody = ParseTableBody(tableBodyTag, tableStyle, classes);
            }
            else
            {
                throw new FormatException("Can't find 'tbody' tag.");
            }

            var tableHeadTag = tableTag.Elements("thead").FirstOrDefault();

            var tableHead = tableHeadTag != null ? ParseTableHead(tableHeadTag, tableStyle, classes) : null;

            var table = new Table
                {
                    Body = tableBody,
                    Head = tableHead,

                    Style = tableStyle
                };

            return table;
        }

        private static TableHead ParseTableHead(
            XElement tableHeadTag,
            BaseStyle tablestyle,
            IDictionary<string, BaseStyle> classes)
        {
            BaseStyle theadStyle = StyleFactory.DefaultStyle;
            var attrClass = tableHeadTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var theadClasses = GetStylesByClasses(classes, attrClass);
                theadStyle = StyleFactory.MergeStyles(tablestyle, theadClasses);
            }

            var rows = tableHeadTag.Elements("tr").Select(tr => ParseHeadRow(tr, theadStyle, classes));

            var thead = new TableHead
                {
                    Style = theadStyle,
                    Rows = rows
                };

            return thead;
        }

        private static TableBody ParseTableBody(
            XElement tableBodyTag,
            BaseStyle tablestyle,
            IDictionary<string, BaseStyle> classes)
        {
            BaseStyle tbodyStyle = StyleFactory.DefaultStyle;
            var attrClass = tableBodyTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                tbodyStyle = StyleFactory.MergeStyles(tablestyle, tbodyClasses);
            }

            var rows = tableBodyTag.Elements("tr").Select(tr => ParseBodyRow(tr, tbodyStyle, classes));

            var tbody = new TableBody
            {
                Style = tbodyStyle,
                Rows = rows
            };

            return tbody;
        }

        private static TableRow ParseHeadRow(
            XElement tableHeadRowTag,
            BaseStyle theadStyle,
            IDictionary<string, BaseStyle> classes)
        {
            BaseStyle rowStyle = StyleFactory.DefaultStyle;
            var attrClass = tableHeadRowTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                rowStyle = StyleFactory.MergeStyles(theadStyle, tbodyClasses);
            }

            var cells = tableHeadRowTag.Elements("th").Select(th => ParseCell(th, rowStyle, classes));

            var tableRow = new TableRow
            {
                Style = rowStyle,
                Cells = cells
            };

            return tableRow;
        }

        private static TableRow ParseBodyRow(
            XElement tableBodyRowTag,
            BaseStyle tbodyStyle,
            IDictionary<string, BaseStyle> classes)
        {
            BaseStyle rowStyle = StyleFactory.DefaultStyle;
            var attrClass = tableBodyRowTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                rowStyle = StyleFactory.MergeStyles(tbodyStyle, tbodyClasses);
            }

            var cells = tableBodyRowTag.Elements("td").Select(td => ParseCell(td, rowStyle, classes));

            var tableRow = new TableRow
            {
                Style = rowStyle,
                Cells = cells
            };

            return tableRow;
        }

        private static TableCell ParseCell(
            XElement cellTag,
            BaseStyle rowStyle,
            IEnumerable<KeyValuePair<string, BaseStyle>> classes)
        {
            BaseStyle cellStyle = StyleFactory.DefaultStyle;
            var attrClass = cellTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                cellStyle = StyleFactory.MergeStyles(rowStyle, tbodyClasses);
            }

            string formula = null;

            var attrFormula = cellTag.Attributes("formula").FirstOrDefault();
            if (attrFormula != null)
            {
                formula = attrFormula.Value;
            }

            string numberFormat = null;

            var attrNumberFormat = cellTag.Attributes("number-format").FirstOrDefault();
            if (attrNumberFormat != null)
            {
                numberFormat = attrNumberFormat.Value;
            }

            string title = null;

            var attrTitle = cellTag.Attributes("number-format").FirstOrDefault();
            if (attrTitle != null)
            {
                title = attrTitle.Value;
            }

            var cell = new TableCell
            {
                Style = new CellStyle(cellStyle, formula, numberFormat, title),
                Value = cellTag.Value
            };

            return cell;
        }
    }
}