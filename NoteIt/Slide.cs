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

        private Bitmap image;

        private int nr;

        public Slide(int nr, Note note)
        {
            this.nr = nr;

            grid = new Grid();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);
            grid.Height = 200;

            // adding empty slide
            slideText = new SlideText(nr, note);
            Grid.SetColumn(slideText, 1);
            grid.Children.Add(slideText);
        }

        public void Remove()
        {
            grid.Children.Remove(slideText);
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

        public SlideText SlideText
        {
            get
            {
                return slideText;
            }
            set
            {
                // right now we copy only text, due to child removal issues, as described in:
                // https://social.msdn.microsoft.com/Forums/vstudio/en-US/8a9fa5f1-5b55-45cc-8495-7a4527002568/disconnecting-children-from-a-logical-element?forum=wpf
                slideText.Text = value.Text;
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
            PdfPCell cell;
            if (image != null)
            {
                // PDF image is not set for this slide (PDF slides weren't imported or user decided to add additional slides)
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(BitmapHelper.ResizeBitmap(image, 300, 200), System.Drawing.Imaging.ImageFormat.Bmp);
                cell = new PdfPCell(pdfImage);
                table.AddCell(cell);
            }
            else
                table.AddCell(new PdfPCell());

            cell = new PdfPCell(new iTextSharp.text.Paragraph(slideText.Text));
            table.AddCell(cell);   
        }

    }
}
