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
        private Button addSlideButton;
        private Button deleteSlideButton;

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

            // grid displaying slideText and new slide button
            rightGrid = new Grid();
            RowDefinition gridRow1 = new RowDefinition();
            RowDefinition gridRow2 = new RowDefinition();
            RowDefinition gridRow3 = new RowDefinition();
            gridRow2.Height = System.Windows.GridLength.Auto;
            gridRow3.Height = System.Windows.GridLength.Auto;
            rightGrid.RowDefinitions.Add(gridRow1);
            rightGrid.RowDefinitions.Add(gridRow2);
            rightGrid.RowDefinitions.Add(gridRow3);

            // adding empty slide
            slideText = new SlideText(nr, note);

            slideText.MouseEnter += DisplayDeleteButton;


            Grid.SetRow(slideText, 0);
            rightGrid.Children.Add(slideText);

            // adding new slide button under the slide
            addSlideButton = new Button();
            // keep slide number in name in order to identify button in on click event
            addSlideButton.Name = "addSlideButton" + nr.ToString();
            addSlideButton.Content = "Add slide";
            addSlideButton.Height = 20;
            addSlideButton.Click += note.AddSlide_Click;
            Grid.SetRow(addSlideButton, 1);
            rightGrid.Children.Add(addSlideButton);

            // adding delete slide button
            deleteSlideButton = new Button();
            // keep slide number in name in order to identify button in on click event
            deleteSlideButton.Name = "deleteSlideButton" + nr.ToString();
            deleteSlideButton.Content = "Delete slide";
            deleteSlideButton.Height = 20;
            deleteSlideButton.Click += note.DeleteSlide_Click;
            Grid.SetRow(deleteSlideButton, 2);
            rightGrid.Children.Add(deleteSlideButton);




            Grid.SetColumn(rightGrid, 1);
            grid.Children.Add(rightGrid);
        }

        private void DisplayDeleteButton(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine("wyświetlam " + nr);
        }

        public void Remove()
        {
            grid.Children.Remove(rightGrid);
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
                Debug.WriteLine("ustawiam " + slideText.Text + " na " + value.Text + " " + rightGrid.Children[0]);
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
