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
            
            // keep slide number in name in order to identify button in on click event
            deleteSlideButton.Name = "deleteSlideButton" + nr.ToString();
            deleteSlideButton.Click += note.DeleteSlide_Click;

            addSlideButton.Name = "addSlideButton" + nr.ToString();
            addSlideButton.Click += note.AddSlide_Click;
            
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

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            deleteSlideButton.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            deleteSlideButton.Visibility = Visibility.Hidden;
        }
    }
}
