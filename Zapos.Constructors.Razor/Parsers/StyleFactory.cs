using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ExCSS.Model;

using Zapos.Common.Styles;

namespace Zapos.Constructors.Razor.Parsers
{
    public class StyleFactory
    {
        private readonly Dictionary<string, ConvertStyle> _styleConverters =
            new Dictionary<string, ConvertStyle>
            {
                { "color", StyleConverters.ColorConverter },
                { "text-align", StyleConverters.HAlignConverter },
                { "text-decoration", StyleConverters.TextDecorationConverter },
                { "vertical-align", StyleConverters.VAlignConverter },
                { "width", StyleConverters.WidthConverter },
                { "height", StyleConverters.HeightConverter },
                { "background-color", StyleConverters.BackgroundColorConverter },
                { "border-left", StyleConverters.BorderLeftConverter },
                { "border-right", StyleConverters.BorderRightConverter },
                { "border-top", StyleConverters.BorderTopConverter },
                { "border-bottom", StyleConverters.BorderBottomConverter },
                { "font-family", StyleConverters.FontFamilyConverter },
                { "font-size", StyleConverters.FontSizeConverter },
                { "font-style", StyleConverters.FontStyleConverter },
                { "font-weight", StyleConverters.FontWeightConverter }
            };

        public StyleFactory()
        {
            Style = DefaultStyle;
        }

        public StyleFactory(IEnumerable<Declaration> declarations)
            : this()
        {
            var copyOfStyle = Style;
            foreach (var declaration in declarations)
            {
                var action = _styleConverters[declaration.Name];
                action(ref copyOfStyle, declaration);
            }

            if (!DefaultStyle.Equals(Style))
            {
                copyOfStyle.Id = Guid.NewGuid();
            }

            Style = copyOfStyle;
        }

        private delegate void ConvertStyle(ref BaseStyle baseStyle, Declaration declaration);

        public static BaseStyle DefaultStyle
        {
            get
            {
                return new BaseStyle
                {
                    Id = Guid.Parse("220B1E42-DE05-463D-B322-23D9F26F16A3"),

                    Font = new InheritStyle<string>("Times New Roman"),
                    BackgroundColor = new InheritStyle<Color>(Color.White),
                    BorderBottomColor = new InheritStyle<Color>(Color.Black),
                    BorderBottomStyle = new InheritStyle<BorderStyle>(BorderStyle.None),
                    BorderBottomWidth = new InheritStyle<ushort>(1),
                    BorderLeftColor = new InheritStyle<Color>(Color.Black),
                    BorderLeftStyle = new InheritStyle<BorderStyle>(BorderStyle.None),
                    BorderLeftWidth = new InheritStyle<ushort>(1),
                    BorderRightColor = new InheritStyle<Color>(Color.Black),
                    BorderRightStyle = new InheritStyle<BorderStyle>(BorderStyle.None),
                    BorderRightWidth = new InheritStyle<ushort>(1),
                    BorderTopColor = new InheritStyle<Color>(Color.Black),
                    BorderTopStyle = new InheritStyle<BorderStyle>(BorderStyle.None),
                    BorderTopWidth = new InheritStyle<ushort>(1),
                    Color = new InheritStyle<Color>(Color.Black),
                    FontSize = new InheritStyle<ushort>(14),
                    HAlign = new InheritStyle<HAlign>(HAlign.Left),
                    Height = new InheritStyle<ushort>(10),
                    VAlign = new InheritStyle<VAlign>(VAlign.Bottom),
                    Width = new InheritStyle<ushort>(50),
                };
            }
        }

        public BaseStyle Style { get; private set; }

        public static BaseStyle MergeStyles(BaseStyle style, IEnumerable<BaseStyle> children)
        {
            return children.Aggregate(style, MergeStyles);
        }

        public static BaseStyle MergeStyles(IEnumerable<BaseStyle> children)
        {
            return MergeStyles(DefaultStyle, children);
        }

        private static BaseStyle MergeStyles(BaseStyle baseStyle, BaseStyle mutations)
        {
            var isNewId = false;

            if (!mutations.Color.IsBase && baseStyle.Color != mutations.Color)
            {
                isNewId = true;
                baseStyle.Color = mutations.Color;
            }

            if (!mutations.Width.IsBase && baseStyle.Width != mutations.Width)
            {
                isNewId = true;
                baseStyle.Width = mutations.Width;
            }

            if (!mutations.Height.IsBase && baseStyle.Height != mutations.Height)
            {
                isNewId = true;
                baseStyle.Height = mutations.Height;
            }

            if (!mutations.BackgroundColor.IsBase && baseStyle.BackgroundColor != mutations.BackgroundColor)
            {
                isNewId = true;
                baseStyle.BackgroundColor = mutations.BackgroundColor;
            }

            if (!mutations.BorderLeftColor.IsBase && baseStyle.BorderLeftColor != mutations.BorderLeftColor)
            {
                isNewId = true;
                baseStyle.BorderLeftColor = mutations.BorderLeftColor;
            }

            if (!mutations.BorderLeftWidth.IsBase && baseStyle.BorderLeftWidth != mutations.BorderLeftWidth)
            {
                isNewId = true;
                baseStyle.BorderLeftWidth = mutations.BorderLeftWidth;
            }

            if (!mutations.BorderRightColor.IsBase && baseStyle.BorderRightColor != mutations.BorderRightColor)
            {
                isNewId = true;
                baseStyle.BorderRightColor = mutations.BorderRightColor;
            }

            if (!mutations.BorderTopColor.IsBase && baseStyle.BorderTopColor != mutations.BorderTopColor)
            {
                isNewId = true;
                baseStyle.BorderTopColor = mutations.BorderTopColor;
            }

            if (!mutations.BorderBottomColor.IsBase && baseStyle.BorderBottomColor != mutations.BorderBottomColor)
            {
                isNewId = true;
                baseStyle.BorderBottomColor = mutations.BorderBottomColor;
            }

            if (!mutations.BorderRightWidth.IsBase && baseStyle.BorderRightWidth != mutations.BorderRightWidth)
            {
                isNewId = true;
                baseStyle.BorderRightWidth = mutations.BorderRightWidth;
            }

            if (!mutations.BorderTopWidth.IsBase && baseStyle.BorderTopWidth != mutations.BorderTopWidth)
            {
                isNewId = true;
                baseStyle.BorderTopWidth = mutations.BorderTopWidth;
            }

            if (!mutations.BorderBottomWidth.IsBase && baseStyle.BorderBottomWidth != mutations.BorderBottomWidth)
            {
                isNewId = true;
                baseStyle.BorderBottomWidth = mutations.BorderBottomWidth;
            }

            if (!mutations.Font.IsBase && baseStyle.Font != mutations.Font)
            {
                isNewId = true;
                baseStyle.Font = mutations.Font;
            }

            if (!mutations.FontSize.IsBase && baseStyle.FontSize != mutations.FontSize)
            {
                isNewId = true;
                baseStyle.FontSize = mutations.FontSize;
            }

            if (!mutations.HAlign.IsBase && baseStyle.HAlign != mutations.HAlign)
            {
                isNewId = true;
                baseStyle.HAlign = mutations.HAlign;
            }

            if (!mutations.VAlign.IsBase && baseStyle.VAlign != mutations.VAlign)
            {
                isNewId = true;
                baseStyle.VAlign = mutations.VAlign;
            }

            if (!mutations.IsBold.IsBase && baseStyle.IsBold != mutations.IsBold)
            {
                isNewId = true;
                baseStyle.IsBold = mutations.IsBold;
            }

            if (!mutations.IsItalic.IsBase && baseStyle.IsItalic != mutations.IsItalic)
            {
                isNewId = true;
                baseStyle.IsItalic = mutations.IsItalic;
            }

            if (!mutations.IsLineThrough.IsBase && baseStyle.IsLineThrough != mutations.IsLineThrough)
            {
                isNewId = true;
                baseStyle.IsLineThrough = mutations.IsLineThrough;
            }

            if (!mutations.IsOverline.IsBase && baseStyle.IsOverline != mutations.IsOverline)
            {
                isNewId = true;
                baseStyle.IsOverline = mutations.IsOverline;
            }

            if (!mutations.IsUnderline.IsBase && baseStyle.IsUnderline != mutations.IsUnderline)
            {
                isNewId = true;
                baseStyle.IsUnderline = mutations.IsUnderline;
            }

            if (!mutations.BorderBottomStyle.IsBase && baseStyle.BorderBottomStyle != mutations.BorderBottomStyle)
            {
                isNewId = true;
                baseStyle.BorderBottomStyle = mutations.BorderBottomStyle;
            }

            if (!mutations.BorderLeftStyle.IsBase && baseStyle.BorderLeftStyle != mutations.BorderLeftStyle)
            {
                isNewId = true;
                baseStyle.BorderLeftStyle = mutations.BorderLeftStyle;
            }

            if (!mutations.BorderRightStyle.IsBase && baseStyle.BorderRightStyle != mutations.BorderRightStyle)
            {
                isNewId = true;
                baseStyle.BorderRightStyle = mutations.BorderRightStyle;
            }

            if (!mutations.BorderTopStyle.IsBase && baseStyle.BorderTopStyle != mutations.BorderTopStyle)
            {
                isNewId = true;
                baseStyle.BorderTopStyle = mutations.BorderTopStyle;
            }

            if (isNewId)
            {
                baseStyle.Id = Guid.NewGuid();
            }

            return baseStyle;
        }
    }
}