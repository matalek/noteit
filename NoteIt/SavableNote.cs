using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace NoteIt
{
    [Serializable]
    class SavableNote
    {
        private List<SavableSlide> slidesList = new List<SavableSlide>();

        public String Title { get; set;  }

        public bool IsPdfPresent { get; set; }

        public void AddSlide(SavableSlide slide)
        {
            slidesList.Add(slide);
        }

        public List<SavableSlide> SlidesList
        {
            get
            {
                return slidesList;
            }
        }
    }
}
