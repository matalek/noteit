using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace NoteIt
{
    [Serializable]
    public class SavableSlide
    {
        public SavableSlide(Slide slide)
        {
            text = slide.Text;
            image = slide.Image;
        }

        private String text;

        private Bitmap image;

        public String Text
        {
            get
            {
                return text;
            }
        }

        public Bitmap Image
        {
            get
            {
                return image;
            }
        }
    }
}
