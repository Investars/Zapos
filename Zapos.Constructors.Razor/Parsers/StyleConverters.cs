using System;
using System.Drawing;
using System.Linq;
using ExCSS.Model;
using Zapos.Common.Styles;
using Zapos.Constructors.Razor.Extensions;

namespace Zapos.Constructors.Razor.Parsers
{
    internal static class StyleConverters
    {
        public static void ColorConverter(ref BaseStyle style, Declaration declaration)
        {
            style.Color = declaration.Expression.Terms.First().ToColor();
        }

        public static void HAlignConverter(ref BaseStyle style, Declaration declaration)
        {
            style.HAlign = EnumUtils.ParseHAlign(declaration.Expression.Terms.First().Value);
        }

        public static void VAlignConverter(ref BaseStyle style, Declaration declaration)
        {
            style.VAlign = EnumUtils.ParseVAlign(declaration.Expression.Terms.First().Value);
        }

        public static void WidthConverter(ref BaseStyle style, Declaration declaration)
        {
            style.Width = ushort.Parse(declaration.Expression.Terms.First().Value);
        }

        public static void HeightConverter(ref BaseStyle style, Declaration declaration)
        {
            style.Height = ushort.Parse(declaration.Expression.Terms.First().Value);
        }

        public static void BackgroundColorConverter(ref BaseStyle style, Declaration declaration)
        {
            var backgroundColor = declaration.Expression.Terms.First().ToColor();
            style.BackgroundColor = backgroundColor;
        }

        public static void BorderLeftConverter(ref BaseStyle style, Declaration declaration)
        {
            var width = ushort.Parse(declaration.Expression.Terms.ElementAt(0).Value);
            var borderStyle = EnumUtils.ParseBorderStyle(declaration.Expression.Terms.ElementAt(1).Value);
            var color = declaration.Expression.Terms.ElementAt(2).ToColor();

            style.BorderLeftColor = color;
            style.BorderLeftStyle = borderStyle;
            style.BorderLeftWidth = width;
        }

        public static void BorderRightConverter(ref BaseStyle style, Declaration declaration)
        {
            var width = ushort.Parse(declaration.Expression.Terms.ElementAt(0).Value);
            var borderStyle = EnumUtils.ParseBorderStyle(declaration.Expression.Terms.ElementAt(1).Value);
            var color = declaration.Expression.Terms.ElementAt(2).ToColor();

            style.BorderRightColor = color;
            style.BorderRightStyle = borderStyle;
            style.BorderRightWidth = width;
        }

        public static void BorderTopConverter(ref BaseStyle style, Declaration declaration)
        {
            var width = ushort.Parse(declaration.Expression.Terms.ElementAt(0).Value);
            var borderStyle = EnumUtils.ParseBorderStyle(declaration.Expression.Terms.ElementAt(1).Value);
            var color = declaration.Expression.Terms.ElementAt(2).ToColor();

            style.BorderTopColor = color;
            style.BorderTopStyle = borderStyle;
            style.BorderTopWidth = width;
        }

        public static void BorderBottomConverter(ref BaseStyle style, Declaration declaration)
        {
            var width = ushort.Parse(declaration.Expression.Terms.ElementAt(0).Value);
            var borderStyle = EnumUtils.ParseBorderStyle(declaration.Expression.Terms.ElementAt(1).Value);
            var color = declaration.Expression.Terms.ElementAt(2).ToColor();

            style.BorderBottomColor = color;
            style.BorderBottomStyle = borderStyle;
            style.BorderBottomWidth = width;
        }

        public static void FontFamilyConverter(ref BaseStyle style, Declaration declaration)
        {
            var family = declaration.Expression.Terms.First().Value;

            new Font(family, 10);

            style.Font = family;
        }

        public static void FontSizeConverter(ref BaseStyle style, Declaration declaration)
        {
            var value = declaration.Expression.Terms.First().Value;
            style.FontSize = ushort.Parse(value);
        }

        public static void TextDecorationConverter(ref BaseStyle style, Declaration declaration)
        {
            style.IsLineThrough = false;
            style.IsOverline = false;
            style.IsUnderline = false;

            var flags = declaration.Expression.Terms.Select(flag => flag.Value).ToArray();

            if (!flags.Any())
            {
                throw new FormatException("Style 'text-decoration' has incorrect value. [text-decoration: [line-through | overline | underline] | none]");
            }

            if (flags.Contains("none"))
            {
                return;
            }

            if (flags.Contains("line-through"))
            {
                style.IsLineThrough = true;
            }

            if (flags.Contains("overline"))
            {
                style.IsOverline = true;
            }

            if (flags.Contains("underline"))
            {
                style.IsUnderline = true;
            }
        }

        public static void FontStyleConverter(ref BaseStyle style, Declaration declaration)
        {
            string flag;

            try
            {
                flag = declaration.Expression.Terms.First().Value;
            }
            catch (InvalidOperationException exception)
            {
                throw new FormatException("Style 'font-style' has incorrect value. [font-style: normal | italic]", exception);
            }
            catch (ArgumentNullException exception)
            {
                throw new FormatException("Style 'font-style' has incorrect value. [font-style: normal | italic]", exception);
            }
            catch (NullReferenceException exception)
            {
                throw new FormatException("Style 'font-style' has incorrect value. [font-style: normal | italic]", exception);
            }

            switch (flag)
            {
                case "normal":
                    style.IsItalic = false;
                    break;
                case "italic":
                    style.IsItalic = true;
                    break;
                default:
                    throw new FormatException("Style 'font-style' has incorrect value. [font-style: normal | italic]");
            }
        }

        public static void FontWeightConverter(ref BaseStyle style, Declaration declaration)
        {
            string flag;

            try
            {
                flag = declaration.Expression.Terms.First().Value;
            }
            catch (InvalidOperationException exception)
            {
                throw new FormatException("Style 'font-weight' has incorrect value. [font-weight: normal | bold]", exception);
            }
            catch (ArgumentNullException exception)
            {
                throw new FormatException("Style 'font-weight' has incorrect value. [font-weight: normal | bold]", exception);
            }
            catch (NullReferenceException exception)
            {
                throw new FormatException("Style 'font-weight' has incorrect value. [font-weight: normal | bold]", exception);
            }

            switch (flag)
            {
                case "normal":
                    style.IsBold = false;
                    break;
                case "bold":
                    style.IsBold = true;
                    break;
                default:
                    throw new FormatException("Style 'font-weight' has incorrect value. [font-weight: normal | bold]");
            }
        }
    }
}