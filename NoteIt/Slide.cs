using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using System.IO;
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NoteIt
{
    class Slide
    {
        private SlideNote slideNote;

        private SlideText slideText;

        private Grid grid;
        private Grid rightGrid;
        private Button newSlideButton;

        private Bitmap image;

        public Slide()
        {
            grid = new Grid();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);
            grid.Height = 200;

            // grid displaying slideText and new slide button
            rightGrid = new Grid();
            RowDefinition gridRow1 = new RowDefinition();
            RowDefinition gridRow2 = new RowDefinition();
            gridRow2.Height = System.Windows.GridLength.Auto;
            rightGrid.RowDefinitions.Add(gridRow1);
            rightGrid.RowDefinitions.Add(gridRow2);

            // adding empty slide
            slideText = new SlideText();
            Grid.SetRow(slideText, 0);
            rightGrid.Children.Add(slideText);

            // adding new slide button under the slide
            newSlideButton = new Button();
            newSlideButton.Content = "New slide";
            newSlideButton.Height = 20;
            Grid.SetRow(newSlideButton, 1);
            rightGrid.Children.Add(newSlideButton);

            Grid.SetColumn(rightGrid, 1);
            grid.Children.Add(rightGrid);
        }

        public SlideNote SlideNote
        {
            get
            {
                return slideNote;
            }
            set
            {
                slideNote = value;
            }
        }

        public Grid Grid
        {
            get
            {
                return grid;
            }
        }

        public void AddPdfSlide(FileSource fs, int page)
        {

            image = BitmapHelper.ResizeBitmap(MuPdfWrapper.ExtractPage(fs, page + 1), 400, 400);

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            MemoryStream Ms = new MemoryStream();
            System.Drawing.Bitmap ObjBitmap = image;
            ObjBitmap.Save(Ms, System.Drawing.Imaging.ImageFormat.Bmp);
            Ms.Position = 0;
            BitmapImage ObjBitmapImage = new BitmapImage();
            ObjBitmapImage.BeginInit();
            ObjBitmapImage.StreamSource = Ms;
            ObjBitmapImage.EndInit();
            img.Source = ObjBitmapImage;

            Grid.SetColumn(img, 0);
            grid.Children.Add(img);
        }

        public void PrintSlide(PdfPTable table)
        {

            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(BitmapHelper.ResizeBitmap(image, 300, 200), System.Drawing.Imaging.ImageFormat.Bmp);

            PdfPCell cell = new PdfPCell(pdfImage);
            table.AddCell(cell);

            cell = new PdfPCell(new iTextSharp.text.Paragraph(slideText.Text));
            table.AddCell(cell);   
        }

    }
}
