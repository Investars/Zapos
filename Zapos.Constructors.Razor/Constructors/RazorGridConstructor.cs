using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

using Zapos.Common.Constructors;
using Zapos.Common.DocumentModel;
using Zapos.Common.Styles;

using Zapos.Constructors.Razor.Generators;
using Zapos.Constructors.Razor.Parsers;

namespace Zapos.Constructors.Razor.Constructors
{
    using System.Globalization;

    public class RazorGridConstructor : IGridConstructor
    {
        private IDictionary<string, object> _config;

        private Func<string, string> _resolvePath = s => s;

        public void Init(IDictionary<string, object> config)
        {
            _config = config;

            _resolvePath = (Func<string, string>)_config["RESOLVE_PATH_ACTION"];
        }

        public IEnumerable<Table> CreateTables<TModel>(string filePath, TModel model)
        {
            var generator = new RazorTextGenerator();

            var content = generator.Generate(filePath, model);

            var document = XDocument.Parse(string.Concat("<body>", content, "</body>"));

            // ReSharper disable PossibleNullReferenceException

            var tableTags = document.Root.Elements("table");

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

            var tables = ParseTables(tableTags, classes).ToArray();

            var imagesTags = document.Root.Elements("img");

            var images = ParseImages(imagesTags);

            var table = tables.First();
            table.Images = images;

            return tables;
        }

        private static IEnumerable<BaseStyle> GetStylesByClasses(IEnumerable<KeyValuePair<string, BaseStyle>> classes, XAttribute attrClass)
        {
            var ownClasses = attrClass.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var classesInternal = classes.AsParallel();
            var result = new List<BaseStyle>();

            foreach (var ownClass in ownClasses)
            {
                KeyValuePair<string, BaseStyle>? @class;
                try
                {
                    var internalClass = ownClass;
                    @class = classesInternal.First(cl => string.Equals(cl.Key, internalClass, StringComparison.InvariantCultureIgnoreCase));
                }
                catch (InvalidOperationException)
                {
                    @class = null;
                }

                if (@class != null)
                {
                    result.Add(@class.Value.Value);
                }
            }

            return result;
        }

        private static IEnumerable<Table> ParseTables(IEnumerable<XElement> tableTags, IDictionary<string, BaseStyle> classes)
        {
            var tableIndex = 0;

            foreach (var tableTag in tableTags)
            {
                var tableStyle = StyleFactory.DefaultStyle;
                var attrClass = tableTag.Attributes("class").FirstOrDefault();
                if (attrClass != null)
                {
                    var tableClasses = GetStylesByClasses(classes, attrClass);
                    tableStyle = StyleFactory.MergeStyles(tableClasses);
                }

                var attrTitle = tableTag.Attributes("title").FirstOrDefault();
                var tableName = attrTitle != null ? attrTitle.Value : tableIndex.ToString(CultureInfo.InvariantCulture);

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

                        Style = tableStyle,

                        Name = tableName
                    };

                tableIndex++;

                yield return table;
            }
        }

        private static TableHead ParseTableHead(
            XElement tableHeadTag,
            BaseStyle tablestyle,
            IDictionary<string, BaseStyle> classes)
        {
            var attrClass = tableHeadTag.Attributes("class").FirstOrDefault();
            BaseStyle theadStyle;
            if (attrClass != null)
            {
                var theadClasses = GetStylesByClasses(classes, attrClass);
                theadStyle = StyleFactory.MergeStyles(tablestyle, theadClasses);
            }
            else
            {
                theadStyle = tablestyle;
            }

            var rows = tableHeadTag.Elements("tr").Select(tr => ParseHeadRow(tr, theadStyle, classes));

            var thead = new TableHead
                {
                    Style = theadStyle,
                    Rows = rows.ToArray()
                };

            return thead;
        }

        private static TableBody ParseTableBody(
            XElement tableBodyTag,
            BaseStyle tablestyle,
            IDictionary<string, BaseStyle> classes)
        {
            var attrClass = tableBodyTag.Attributes("class").FirstOrDefault();
            BaseStyle tbodyStyle;
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                tbodyStyle = StyleFactory.MergeStyles(tablestyle, tbodyClasses);
            }
            else
            {
                tbodyStyle = tablestyle;
            }

            var rows = tableBodyTag.Elements("tr").Select(tr => ParseBodyRow(tr, tbodyStyle, classes));

            var tbody = new TableBody
            {
                Style = tbodyStyle,
                Rows = rows.ToArray()
            };

            return tbody;
        }

        private static TableRow ParseHeadRow(
            XElement tableHeadRowTag,
            BaseStyle theadStyle,
            IDictionary<string, BaseStyle> classes)
        {
            BaseStyle rowStyle;
            var attrClass = tableHeadRowTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                rowStyle = StyleFactory.MergeStyles(theadStyle, tbodyClasses);
            }
            else
            {
                rowStyle = theadStyle;
            }

            var cells = tableHeadRowTag.Elements("th").Select(th => ParseCell(th, rowStyle, classes));

            var tableRow = new TableRow
            {
                Style = rowStyle,
                Cells = cells.ToArray()
            };

            return tableRow;
        }

        private static TableRow ParseBodyRow(
            XElement tableBodyRowTag,
            BaseStyle tbodyStyle,
            IDictionary<string, BaseStyle> classes)
        {
            var attrClass = tableBodyRowTag.Attributes("class").FirstOrDefault();
            BaseStyle rowStyle;
            if (attrClass != null)
            {
                var tbodyClasses = GetStylesByClasses(classes, attrClass);
                rowStyle = StyleFactory.MergeStyles(tbodyStyle, tbodyClasses);
            }
            else
            {
                rowStyle = tbodyStyle;
            }

            var cells = tableBodyRowTag.Elements("td").Select(td => ParseCell(td, rowStyle, classes));

            var tableRow = new TableRow
            {
                Style = rowStyle,
                Cells = cells.ToArray()
            };

            return tableRow;
        }

        private static TableCell ParseCell(
            XElement cellTag,
            BaseStyle rowStyle,
            IEnumerable<KeyValuePair<string, BaseStyle>> classes)
        {
            BaseStyle cellStyle;
            var attrClass = cellTag.Attributes("class").FirstOrDefault();
            if (attrClass != null)
            {
                var cellClasses = GetStylesByClasses(classes, attrClass);
                cellStyle = StyleFactory.MergeStyles(rowStyle, cellClasses);
            }
            else
            {
                cellStyle = rowStyle;
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

            var attrTitle = cellTag.Attributes("title").FirstOrDefault();
            if (attrTitle != null)
            {
                title = attrTitle.Value;
            }

            int colspan = 0;

            var attrColspan = cellTag.Attributes("colspan").FirstOrDefault();
            if (attrColspan != null)
            {
                int.TryParse(attrColspan.Value, out colspan);
            }

            int rowspan = 0;

            var attrRowspan = cellTag.Attributes("rowspan").FirstOrDefault();
            if (attrRowspan != null)
            {
                int.TryParse(attrRowspan.Value, out rowspan);
            }

            var cell = new TableCell
            {
                Style = new CellStyle(cellStyle, formula, numberFormat, title, colspan, rowspan),
                Value = ProcessString(cellTag)
            };

            return cell;
        }

        private static object ProcessString(XContainer element)
        {
            var value = element
                    .Nodes()
                    .Select(FilterValueNodes)
                    .Where(x => x != null)
                    .Select(str => str
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim(' ', '\t'))
                        .Aggregate(string.Empty, (result, next) => string.Concat(result, " ", next))
                        .Substring(1))
                    .Aggregate(string.Empty, string.Concat);

            var decodedValue = HttpUtility.HtmlDecode(value);

            double numberValue;
            if (double.TryParse(decodedValue, out numberValue))
            {
                return numberValue;
            }

            DateTime dateValue;
            if (DateTime.TryParse(decodedValue, out dateValue))
            {
                return dateValue;
            }

            return decodedValue;
        }

        private static string FilterValueNodes(XNode node)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Text:
                return node.ToString();
                case XmlNodeType.Element:
                return ((XElement)node).Name.LocalName.ToLower() == "br" ? "&#013;&#010;" : null;
                default:
                return null;
            }
        }

        private IEnumerable<TableImage> ParseImages(IEnumerable<XElement> imagesTags)
        {
            foreach (var imagesTag in imagesTags)
            {
                var tableImage = new TableImage();
                var styleAttr = imagesTag.Attribute("style").Value;
                if (!string.IsNullOrWhiteSpace(styleAttr))
                {
                    var styles = styleAttr
                                    .ToLower()
                                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(
                                        style => style
                                                     .Trim()
                                                     .Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(part => part.Trim()))
                                    .ToDictionary(item => item.ElementAt(0), item => item.ElementAt(1));

                    try
                    {
                        tableImage.Height = int.Parse(styles["height"].Replace("px", string.Empty));
                    }
                    catch (Exception exception)
                    {
                        throw new FormatException("Can't parse image height.", exception);
                    }

                    try
                    {
                        tableImage.Width = int.Parse(styles["width"].Replace("px", string.Empty));
                    }
                    catch (Exception exception)
                    {
                        throw new FormatException("Can't parse image width.", exception);
                    }

                    try
                    {
                        tableImage.Left = int.Parse(styles["left"].Replace("px", string.Empty));
                    }
                    catch (Exception exception)
                    {
                        throw new FormatException("Can't parse image left.", exception);
                    }

                    try
                    {
                        tableImage.Top = int.Parse(styles["top"].Replace("px", string.Empty));
                    }
                    catch (Exception exception)
                    {
                        throw new FormatException("Can't parse image top.", exception);
                    }
                }
                else
                {
                    throw new FormatException("Can't parse img style attribute.");
                }

                var src = imagesTag.Attribute("src").Value;
                if (!string.IsNullOrWhiteSpace(src))
                {
                    var path = _resolvePath(src);
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("Can't find image", path);
                    }

                    tableImage.ImagePath = path;
                }
                else
                {
                    throw new FormatException("Can't parse img src attribute.");
                }

                yield return tableImage;
            }
        }
    }
}