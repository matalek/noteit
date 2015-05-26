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
    public class Slide
    {

        private SlideText slideText;

        private Grid grid;

        private Bitmap image;

        System.Windows.Controls.Image imageControl;

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

        public string Text
        {
            get
            {
                return slideText.Text;
            }
            set
            {
                slideText.Text = value;
            }
        }

        public Bitmap Image
        {
            get
            {
                return image;
            }
        }

        public void AddPdfSlide(FileSource fs, int page)
        {

            image = BitmapHelper.ResizeBitmapToWidth(MuPdfWrapper.ExtractPage(fs, page + 1), 400);

            imageControl = new System.Windows.Controls.Image();

            MemoryStream Ms = new MemoryStream();
            System.Drawing.Bitmap ObjBitmap = image;
            ObjBitmap.Save(Ms, System.Drawing.Imaging.ImageFormat.Bmp);
            Ms.Position = 0;
            BitmapImage ObjBitmapImage = new BitmapImage();
            ObjBitmapImage.BeginInit();
            ObjBitmapImage.StreamSource = Ms;
            ObjBitmapImage.EndInit();
            imageControl.Source = ObjBitmapImage;

            Grid.SetColumn(imageControl, 0);
            grid.Children.Add(imageControl);
        }

        public void RemovePdfSlide()
        {
            grid.Children.Remove(imageControl);
            image = null;
        }

        // checks, wheter there is an assigned PDF image for this slide
        public bool IsPdfPresent()
        {
            return image != null;
        }
    }
}
