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
using System.Windows.Shapes;

using Microsoft.Win32;
using System.IO;

using MahApps.Metro.Controls;

namespace NoteIt
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : MetroWindow
    {
        private Note note;

        public PrintWindow(Note note)
        {
            InitializeComponent();
            this.note = note;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintStrategy printStrategy = null;
            if (verticalRadioButton.IsChecked == true)
            {
                printStrategy = new VerticalPrintStrategy();
            } 
            else
            {
                printStrategy = new HorizontalPrintStrategy();
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF Files (.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                printStrategy.Print(note, new FileStream(dialog.FileName, FileMode.Create), slideNumbersCheckBox.IsChecked.Value);
                Close();
            }

        }

        private void verticalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (previewImageVertical != null)
                previewImageVertical.Visibility = System.Windows.Visibility.Visible;
            if (previewImageHorizontal != null)
                previewImageHorizontal.Visibility = System.Windows.Visibility.Hidden;

            //previewImage.Source = new BitmapImage(new Uri("vertical.png", UriKind.Relative));
        }

        private void horizontalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (previewImageHorizontal != null)
                previewImageHorizontal.Visibility = System.Windows.Visibility.Visible;
            if (previewImageVertical != null)
                previewImageVertical.Visibility = System.Windows.Visibility.Hidden;
            //previewImage.Source = new BitmapImage(new Uri("horizontal.png", UriKind.Relative));
        }
    }
}
