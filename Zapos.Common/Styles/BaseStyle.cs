using System;
using System.Drawing;

namespace Zapos.Common.Styles
{
    public struct BaseStyle
    {
        public Guid Id { get; set; }

        public InheritStyle<Color> Color { get; set; } //+

        public InheritStyle<ushort> Width { get; set; } //-

        public InheritStyle<ushort> Height { get; set; } //-

        public InheritStyle<Color> BackgroundColor { get; set; } //+

        public InheritStyle<Color> BorderLeftColor { get; set; } //+

        public InheritStyle<ushort> BorderLeftWidth { get; set; } //-

        public InheritStyle<Color> BorderRightColor { get; set; } //+

        public InheritStyle<Color> BorderTopColor { get; set; } //+

        public InheritStyle<Color> BorderBottomColor { get; set; } //+

        public InheritStyle<ushort> BorderRightWidth { get; set; } //-

        public InheritStyle<ushort> BorderTopWidth { get; set; } //-

        public InheritStyle<ushort> BorderBottomWidth { get; set; } //-

        public InheritStyle<string> Font { get; set; } //+

        public InheritStyle<ushort> FontSize { get; set; } //+

        public InheritStyle<HAlign> HAlign { get; set; } //+

        public InheritStyle<VAlign> VAlign { get; set; } //+

        public InheritStyle<bool> IsLineThrough { get; set; } //-

        public InheritStyle<bool> IsOverline { get; set; } //-

        public InheritStyle<bool> IsUnderline { get; set; } //+

        public InheritStyle<bool> IsItalic { get; set; } //+

        public InheritStyle<bool> IsBold { get; set; } //+

        public InheritStyle<BorderStyle?> BorderBottomStyle { get; set; } //+

        public InheritStyle<BorderStyle?> BorderLeftStyle { get; set; } //+

        public InheritStyle<BorderStyle?> BorderRightStyle { get; set; } //+

        public InheritStyle<BorderStyle?> BorderTopStyle { get; set; } //+

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Color.GetHashCode();
                hashCode = (hashCode * 397) ^ (Width.GetHashCode());
                hashCode = (hashCode * 397) ^ (Height.GetHashCode());
                hashCode = (hashCode * 397) ^ (BackgroundColor.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderLeftColor.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderLeftWidth.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderRightColor.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderTopColor.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderBottomColor.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderRightWidth.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderTopWidth.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderBottomWidth.GetHashCode());
                hashCode = (hashCode * 397) ^ (Font.GetHashCode());
                hashCode = (hashCode * 397) ^ (FontSize.GetHashCode());
                hashCode = (hashCode * 397) ^ (HAlign.GetHashCode());
                hashCode = (hashCode * 397) ^ (VAlign.GetHashCode());
                hashCode = (hashCode * 397) ^ (IsLineThrough.GetHashCode());
                hashCode = (hashCode * 397) ^ (IsOverline.GetHashCode());
                hashCode = (hashCode * 397) ^ (IsUnderline.GetHashCode());
                hashCode = (hashCode * 397) ^ (IsItalic.GetHashCode());
                hashCode = (hashCode * 397) ^ (IsBold.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderBottomStyle.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderLeftStyle.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderRightStyle.GetHashCode());
                hashCode = (hashCode * 397) ^ (BorderTopStyle.GetHashCode());
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals((BaseStyle)obj);
        }

        private bool Equals(BaseStyle other)
        {
            return (Color == other.Color) &&
                   (Width == other.Width) &&
                   (Height == other.Height) &&
                   (BackgroundColor == other.BackgroundColor) &&
                   (BorderLeftColor == other.BorderLeftColor) &&
                   (BorderLeftWidth == other.BorderLeftWidth) &&
                   (BorderRightColor == other.BorderRightColor) &&
                   (BorderTopColor == other.BorderTopColor) &&
                   (BorderBottomColor == other.BorderBottomColor) &&
                   (BorderRightWidth == other.BorderRightWidth) &&
                   (BorderTopWidth == other.BorderTopWidth) &&
                   (BorderBottomWidth == other.BorderBottomWidth) &&
                   (Font == other.Font) &&
                   (FontSize == other.FontSize) &&
                   (HAlign == other.HAlign) &&
                   (VAlign == other.VAlign) &&
                   (IsLineThrough == other.IsLineThrough) &&
                   (IsOverline == other.IsOverline) &&
                   (IsUnderline == other.IsUnderline) &&
                   (IsItalic == other.IsItalic) &&
                   (IsBold == other.IsBold) &&
                   (BorderBottomStyle == other.BorderBottomStyle) &&
                   (BorderLeftStyle == other.BorderLeftStyle) &&
                   (BorderRightStyle == other.BorderRightStyle) &&
                   (BorderTopStyle == other.BorderTopStyle);
        }

        public static bool operator ==(BaseStyle a, BaseStyle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BaseStyle a, BaseStyle b)
        {
            return !(a == b);
        }
    }
}

