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

        private void DisplayPdfSlides()
        {
            var mainPanel = new MoonPdfPanel();
            mainPanel.InitializeComponent();
            mainPanel.Open(fs);

            List<MoonPdfPanel> slides = new List<MoonPdfPanel>();

            GridHelpers.SetRowCount(PdfSlides, MuPdfWrapper.CountPages(fs));

            for (int i = 0; i < MuPdfWrapper.CountPages(fs); i++)
            {
                var slide = new MoonPdfPanel();
                slide.InitializeComponent();
                slide.Open(fs);
                slide.GotoPage(i + 1);
                slide.Height = 200;
                ContentControl contentControl = new ContentControl();
                contentControl.SetValue(Grid.RowProperty, i);
 
                contentControl.Content = slide;

                Debug.WriteLine(Grid.GetRow(contentControl));

                PdfSlides.Children.Add(contentControl);
            }

            //slidesNumber = slides.Count;
            slides.Clear();
            
        }

        // source: http://www.deltasblog.co.uk/code-snippets/c-resizing-a-bitmap-image/
        private static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // PdfSlides.Children.Add(new Label { Content = "Label" });
            note.Print("test.pdf");
        }

        private void btnOpenPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                fs = new FileSource(openFileDialog.FileName);
            note.AddPdf(fs);
            //DisplayPdfSlides();
        }

        private void btnAddSlide_Click(object sender, RoutedEventArgs e)
        {
            note.AddSlide();
        }

    }
}
