using System;
using System.Collections.Generic;
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

        private StyleFactory()
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
                var defaultStyle = new BaseStyle
                                       {
                                           Id = Guid.Parse("220B1E42-DE05-463D-B322-23D9F26F16A3"),

                                           //Font = new InheritStyle<string>(true, "Times New Roman"),
                                           //BackgroundColor = new InheritStyle<Color>(true, Color.White),
                                           //BorderBottomColor = new InheritStyle<Color>(true, Color.Black),
                                           //BorderBottomStyle = new InheritStyle<BorderStyle>(true, BorderStyle.Solid),
                                           //BorderBottomWidth = new InheritStyle<ushort>(true, 1),
                                           //BorderLeftColor = new InheritStyle<Color>(true, Color.Black),
                                           //BorderLeftStyle = new InheritStyle<BorderStyle>(true, BorderStyle.Solid),
                                           //BorderLeftWidth = new InheritStyle<ushort>(true, 1),
                                           //BorderRightColor = new InheritStyle<Color>(true, Color.Black),
                                           //BorderRightStyle = new InheritStyle<BorderStyle>(true, BorderStyle.Solid),
                                           //BorderRightWidth = new InheritStyle<ushort>(true, 1),
                                           //BorderTopColor = new InheritStyle<Color>(true, Color.Black),
                                           //BorderTopStyle = new InheritStyle<BorderStyle>(true, BorderStyle.Solid),
                                           //BorderTopWidth = new InheritStyle<ushort>(true, 1),
                                           //Color = new InheritStyle<Color>(true, Color.Black),
                                           //FontSize = new InheritStyle<ushort>(true, 14),
                                           //HAlign = new InheritStyle<HAlign>(true, HAlign.Left),
                                           //Height = new InheritStyle<ushort>(true, 20),
                                           //VAlign = new InheritStyle<VAlign>(true, VAlign.Bottom),
                                           //Width = new InheritStyle<ushort>(true, 50),
                                           //IsBold = new InheritStyle<bool>(true, false),
                                           //IsItalic = new InheritStyle<bool>(true, false),
                                           //IsLineThrough = new InheritStyle<bool>(true, false),
                                           //IsOverline = new InheritStyle<bool>(true, false),
                                           //IsUnderline = new InheritStyle<bool>(true, false)
                                       };

                return defaultStyle;
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

            if (mutations.Color.HasValue && baseStyle.Color != mutations.Color)
            {
                isNewId = true;
                baseStyle.Color = mutations.Color;
            }

            if (mutations.Width.HasValue && baseStyle.Width != mutations.Width)
            {
                isNewId = true;
                baseStyle.Width = mutations.Width;
            }

            if (mutations.Height.HasValue && baseStyle.Height != mutations.Height)
            {
                isNewId = true;
                baseStyle.Height = mutations.Height;
            }

            if (mutations.BackgroundColor.HasValue && baseStyle.BackgroundColor != mutations.BackgroundColor)
            {
                isNewId = true;
                baseStyle.BackgroundColor = mutations.BackgroundColor;
            }

            if (mutations.BorderLeftColor.HasValue && baseStyle.BorderLeftColor != mutations.BorderLeftColor)
            {
                isNewId = true;
                baseStyle.BorderLeftColor = mutations.BorderLeftColor;
            }

            if (mutations.BorderLeftWidth.HasValue && baseStyle.BorderLeftWidth != mutations.BorderLeftWidth)
            {
                isNewId = true;
                baseStyle.BorderLeftWidth = mutations.BorderLeftWidth;
            }

            if (mutations.BorderRightColor.HasValue && baseStyle.BorderRightColor != mutations.BorderRightColor)
            {
                isNewId = true;
                baseStyle.BorderRightColor = mutations.BorderRightColor;
            }

            if (mutations.BorderTopColor.HasValue && baseStyle.BorderTopColor != mutations.BorderTopColor)
            {
                isNewId = true;
                baseStyle.BorderTopColor = mutations.BorderTopColor;
            }

            if (mutations.BorderBottomColor.HasValue && baseStyle.BorderBottomColor != mutations.BorderBottomColor)
            {
                isNewId = true;
                baseStyle.BorderBottomColor = mutations.BorderBottomColor;
            }

            if (mutations.BorderRightWidth.HasValue && baseStyle.BorderRightWidth != mutations.BorderRightWidth)
            {
                isNewId = true;
                baseStyle.BorderRightWidth = mutations.BorderRightWidth;
            }

            if (mutations.BorderTopWidth.HasValue && baseStyle.BorderTopWidth != mutations.BorderTopWidth)
            {
                isNewId = true;
                baseStyle.BorderTopWidth = mutations.BorderTopWidth;
            }

            if (mutations.BorderBottomWidth.HasValue && baseStyle.BorderBottomWidth != mutations.BorderBottomWidth)
            {
                isNewId = true;
                baseStyle.BorderBottomWidth = mutations.BorderBottomWidth;
            }

            if (mutations.Font.HasValue && baseStyle.Font != mutations.Font)
            {
                isNewId = true;
                baseStyle.Font = mutations.Font;
            }

            if (mutations.FontSize.HasValue && baseStyle.FontSize != mutations.FontSize)
            {
                isNewId = true;
                baseStyle.FontSize = mutations.FontSize;
            }

            if (mutations.HAlign.HasValue && baseStyle.HAlign != mutations.HAlign)
            {
                isNewId = true;
                baseStyle.HAlign = mutations.HAlign;
            }

            if (mutations.VAlign.HasValue && baseStyle.VAlign != mutations.VAlign)
            {
                isNewId = true;
                baseStyle.VAlign = mutations.VAlign;
            }

            if (mutations.IsBold.HasValue && baseStyle.IsBold != mutations.IsBold)
            {
                isNewId = true;
                baseStyle.IsBold = mutations.IsBold;
            }

            if (mutations.IsItalic.HasValue && baseStyle.IsItalic != mutations.IsItalic)
            {
                isNewId = true;
                baseStyle.IsItalic = mutations.IsItalic;
            }

            if (mutations.IsLineThrough.HasValue && baseStyle.IsLineThrough != mutations.IsLineThrough)
            {
                isNewId = true;
                baseStyle.IsLineThrough = mutations.IsLineThrough;
            }

            if (mutations.IsOverline.HasValue && baseStyle.IsOverline != mutations.IsOverline)
            {
                isNewId = true;
                baseStyle.IsOverline = mutations.IsOverline;
            }

            if (mutations.IsUnderline.HasValue && baseStyle.IsUnderline != mutations.IsUnderline)
            {
                isNewId = true;
                baseStyle.IsUnderline = mutations.IsUnderline;
            }

            if (mutations.BorderBottomStyle.HasValue && baseStyle.BorderBottomStyle != mutations.BorderBottomStyle)
            {
                isNewId = true;
                baseStyle.BorderBottomStyle = mutations.BorderBottomStyle;
            }

            if (mutations.BorderLeftStyle.HasValue && baseStyle.BorderLeftStyle != mutations.BorderLeftStyle)
            {
                isNewId = true;
                baseStyle.BorderLeftStyle = mutations.BorderLeftStyle;
            }

            if (mutations.BorderRightStyle.HasValue && baseStyle.BorderRightStyle != mutations.BorderRightStyle)
            {
                isNewId = true;
                baseStyle.BorderRightStyle = mutations.BorderRightStyle;
            }

            if (mutations.BorderTopStyle.HasValue && baseStyle.BorderTopStyle != mutations.BorderTopStyle)
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
