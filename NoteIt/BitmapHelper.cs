using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace NoteIt
{
    class BitmapHelper
    {
        // source: http://www.deltasblog.co.uk/code-snippets/c-resizing-a-bitmap-image/
        public static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }
    }
}
