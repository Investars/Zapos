using System.Drawing;

namespace Zapos.Common.DocumentModel
{
    public class TableImage
    {
        public int Top { get; set; }

        public int Left { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Image Image { get; set; }
    }
}