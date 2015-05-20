using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace NoteIt
{
    /// <summary>
    /// Interaction logic for SlideText.xaml
    /// </summary>
    public partial class SlideText : UserControl
    {
        public SlideText(int nr, Note note)
        {
            InitializeComponent();
            deleteSlideButton.Name = "deleteSlideButton" + nr.ToString();
            deleteSlideButton.Click += note.DeleteSlide_Click;
            
        }

        public string Text
        {
            get
            {
                return textBox.Text;
            }

            set
            {
                textBox.Text = value;
            }
        }
    }
}
