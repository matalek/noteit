using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace NoteIt
{
    class BitmapHelper
    {
        public static Bitmap ResizeBitmapToWidth(Bitmap source, int width)
        {
            int height = (int)((double) width / (double) source.Width * (double) source.Height);
            Bitmap result = new Bitmap(width, height);
            Graphics.FromImage(result).DrawImage(source, 0, 0, width, height);
            return result;
        }
    }
}
