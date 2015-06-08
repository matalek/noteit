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

        private String title;

        public bool IsPdfPresent { get; set; }

        public void AddSlide(SavableSlide slide)
        {
            slidesList.Add(slide);
        }

        public String Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
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
