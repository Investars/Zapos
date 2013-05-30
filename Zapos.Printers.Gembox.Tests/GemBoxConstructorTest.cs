using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using NUnit.Framework;

using Zapos.Common.DocumentModel;
using Zapos.Common.Styles;

namespace Zapos.Printers.Gembox.Tests
{
    [TestFixture]
    public class GemBoxConstructorTest
    {
        [Test]
        public void ConstructorTest()
        {

            var headBaseStyle = new BaseStyle()
            {
                BackgroundColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderBottomStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderLeftStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderTopStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderRightStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderBottomColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderLeftColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderTopColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderRightColor = new InheritStyle<Color>(Color.Black, Color.Black),
                Color = new InheritStyle<Color>(Color.White, Color.White),
                IsBold = new InheritStyle<bool>(true, true),
                HAlign = new InheritStyle<HAlign>(HAlign.Center, HAlign.Center)
            };

            var bodyBaseStyle = new BaseStyle()
            {
                BackgroundColor = new InheritStyle<Color>(Color.White, Color.White),
                BorderBottomStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderLeftStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderTopStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderRightStyle = new InheritStyle<BorderStyle>(BorderStyle.Solid, BorderStyle.Solid),
                BorderBottomColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderLeftColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderTopColor = new InheritStyle<Color>(Color.Black, Color.Black),
                BorderRightColor = new InheritStyle<Color>(Color.Black, Color.Black),
                Color = new InheritStyle<Color>(Color.Black, Color.Black),
                IsBold = new InheritStyle<bool>(true, true),
                HAlign = new InheritStyle<HAlign>(HAlign.Center, HAlign.Center)
            };

            var tableModel = new Table();
            tableModel.Images = new TableImage[0];
            tableModel.Head = new TableHead()
                {
                    Rows = new[]
                        {
                            new TableRow()
                                {
                                    Cells = new[]
                                        {
                                            new TableCell()
                                                {
                                                    Value = "HeadRow1Cell1",
                                                    Style = new CellStyle() {Style = headBaseStyle}
                                                },
                                            new TableCell()
                                                {
                                                    Value = "HeadRow1Cell2",
                                                    Style = new CellStyle() {Style = headBaseStyle}
                                                },
                                            new TableCell()
                                                {
                                                    Value = "HeadRow1Cell3",
                                                    Style = new CellStyle() {Style = headBaseStyle}
                                                }
                                        },
                                    Style = headBaseStyle
                                }
                        },
                    Style = headBaseStyle
                };
            tableModel.Body = new TableBody()
                {
                    Rows = new[]
                                {
                                    new TableRow()
                                        {
                                            Cells = new[]
                                                {
                                                    new TableCell() { Value = "BodyRow1Cell1", Style = new CellStyle() { Style = bodyBaseStyle } },
                                                    new TableCell() { Value = "BodyRow1Cell2", Style = new CellStyle() { Style = bodyBaseStyle } },
                                                    new TableCell() { Value = "BodyRow1Cell3", Style = new CellStyle() { Style = bodyBaseStyle } },
                                                },
                                                Style = bodyBaseStyle
                                        },
                                    new TableRow()
                                        {
                                            Cells = new[]
                                                {
                                                    new TableCell() { Value = "BodyRow2Cell1", Style = new CellStyle() { Style = bodyBaseStyle } },
                                                    new TableCell() { Value = "BodyRow2Cell2", Style = new CellStyle() { Style = bodyBaseStyle } },
                                                    new TableCell() { Value = "BodyRow2Cell3", Style = new CellStyle() { Style = bodyBaseStyle } },
                                                },
                                                Style = bodyBaseStyle
                                        }
                                },
                    Style = bodyBaseStyle
                };

            try
            {
                var printer = new XlsxPrinter();

                printer.Init(new Dictionary<string, object> { { "LICENSE_KEY", "FREE-LIMITED-KEY" } });

                using (Stream stream = new FileStream("test.xlsx", FileMode.Create))
                {
                    printer.Print(stream, tableModel);

                    Assert.AreNotEqual(stream.Length, 0);
                }

                Assert.Greater(File.ReadAllBytes("test.xlsx").Length, 10);
                File.Delete("test.xlsx");
            }
            finally
            {
                if (File.Exists("test.xlsx"))
                {
                    File.Delete("test.xlsx");
                }
            }
        }
    }
}
