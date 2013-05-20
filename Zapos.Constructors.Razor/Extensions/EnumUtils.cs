using System;

using Zapos.Common.Styles;

namespace Zapos.Constructors.Razor.Extensions
{
    internal static class EnumUtils
    {
        public static HAlign ParseHAlign(string value)
        {
            switch (value.ToLower())
            {
                case "center":
                    return HAlign.Center;
                case "left":
                    return HAlign.Left;
                case "right":
                    return HAlign.Right;
                case "justify":
                    return HAlign.Justify;
                default:
                    throw new ArgumentException("Incorrect format", "value");
            }
        }

        public static VAlign ParseVAlign(string value)
        {
            switch (value.ToLower())
            {
                case "bottom":
                    return VAlign.Bottom;
                case "middle":
                    return VAlign.Middle;
                case "top":
                    return VAlign.Top;
                default:
                    throw new ArgumentException("Incorrect format", "value");
            }
        }

        public static BorderStyle ParseBorderStyle(string value)
        {
            switch (value.ToLower())
            {
                case "none":
                    return BorderStyle.None;
                case "dotted":
                    return BorderStyle.Dotted;
                case "solid":
                    return BorderStyle.Solid;
                case "double":
                    return BorderStyle.Dotted;
                default:
                    throw new ArgumentException("Incorrect format", "value");
            }
        }
    }
}