using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIt
{
    class SlideNote
    {
        private String text;

        public SlideNote(String text)
        {
            this.text = text;
        }

        public String Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }

        }

    }
}
