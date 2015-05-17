using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

using Microsoft.Win32;
using System.Drawing;
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace NoteIt
{
    /// <summary>
    /// Interaction logic for NotePage.xaml
    /// </summary>
    public partial class NotePage : Page
    {

        private FileSource fs;

        //private int slidesNumber = 0;

        private Note note;
        private int slideNotesNumber = 1;

        public NotePage()
        {
            InitializeComponent();
            note = new Note(slidesPanel);

        }

        

        private void ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF Files (.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
                note.Print(new FileStream(dialog.FileName, FileMode.Create));
        }


        private void btnAddSlide_Click(object sender, RoutedEventArgs e)
        {
            note.AddSlideOnEnd();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ImportPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                fs = new FileSource(dialog.FileName);
                note.AddPdf(fs);
            }
        }

    }
}
