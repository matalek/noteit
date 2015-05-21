using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NoteIt
{
    public class Note
    {
        private List<Slide> slidesList;

        private StackPanel panel;

        private TextBox titleBox;

        private bool pdfPresent = false;

        public bool IsPdfPresent
        {
            get
            {
                return pdfPresent;
            }
        }

        // crates note with one empty slide
        public Note(StackPanel panel)
        {
            this.panel = panel;

            // adding note title
            titleBox = new TitleBox();
            panel.Children.Add(titleBox);

            // adding "Add slide" button at the beggining
            var grid = new Grid();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);
            grid.Height = 20;

            var addSlideButton = new Button();
            addSlideButton.Height = 20;
            addSlideButton.Width = 400;
            addSlideButton.Content = "Add slide";
            // use lambda expressions to pass slide number as argument to event handler
            // -1 means inserting slide at the beggining
            addSlideButton.Click += (sender, e) => AddSlide_Click(sender, e, -1);

            Grid.SetColumn(addSlideButton, 1);
            grid.Children.Add(addSlideButton);
            panel.Children.Add(grid);

            // adding first empty slide
            slidesList = new List<Slide>();
            slidesList.Add(new Slide(0, this));
            panel.Children.Add(slidesList.First().Grid);

        }

        public void AddSlideOnEnd()
        {
            slidesList.Add(new Slide(slidesList.Count, this));
            panel.Children.Add(slidesList.Last().Grid);
        }

        public void AddSlide_Click(object sender, System.Windows.RoutedEventArgs e, int nr)
        {
            AddSlideOnEnd();
            
            // from the one before last slide copy its content to next slide (till selected place)
            for (int i = slidesList.Count - 2; i >= nr + 1; i--)
            {
                Debug.Write("copy " + i + " to " + (i+1).ToString() + "\n" );
                slidesList[i + 1].SlideText = slidesList[i].SlideText;
            }
            // right now only text value, see note for Slide.SlideText
            slidesList[nr + 1].SlideText.Text = "";
        }

        private void RemoveLastSlide()
        {
            Slide last = slidesList.Last();
            last.Remove();
            panel.Children.Remove(last.Grid);
            slidesList.RemoveAt(slidesList.Count - 1);
        }

        public void DeleteSlide_Click(object sender, System.Windows.RoutedEventArgs e, int nr)
        {
            // for not empty slide, ask user before deleting it
            if (slidesList[nr].Text != "")
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(
                    "Do you want to delete a note for this slide?", "Delete slide note", btnMessageBox, icnMessageBox);

                if (rsltMessageBox != MessageBoxResult.Yes)
                    return;
            }


            // copy slide's content to previoues slide
            for (int i = nr; i < slidesList.Count - 1; i++)
            {
                Debug.WriteLine("przepisuję " + slidesList[i + 1].SlideText + " do " + slidesList[i].SlideText);
                slidesList[i].SlideText = slidesList[i + 1].SlideText;
                
            }

            Slide last = slidesList.Last();
            if (last.IsPdfPresent())
            {
                // if PDF slide is present, we just clear the content
                last.Text = "";
            }
            else
            {
                // if no PDF image is present, delete the last slide
                RemoveLastSlide();
            }
        }



        public void AddPdf(FileSource fs)
        {
            pdfPresent = true;

            var mainPanel = new MoonPdfPanel();
            mainPanel.InitializeComponent();
            mainPanel.Open(fs);

            var newSlidesNumber = MuPdfWrapper.CountPages(fs);

            for (int i = 0; i < newSlidesNumber; i++)
            {
                if (i < slidesList.Count)
                {
                    // the slide was already created
                    slidesList[i].AddPdfSlide(fs, i);
                }
                else
                {
                    // we have to create new, empty slide
                    slidesList.Add(new Slide(slidesList.Count, this));
                    slidesList.Last().AddPdfSlide(fs, i);
                    panel.Children.Add(slidesList.Last().Grid);
                }  
            }

            // we have to delete images from previous PDF and empty slides at the end of note
            bool stillDeleting = true; // have we met only empty slide
            for (int i = slidesList.Count - 1; i >= newSlidesNumber; i--)
            {
                // removing old PDF
                slidesList[i].RemovePdfSlide();
                if (slidesList[i].Text == "")
                {
                    if (stillDeleting)
                        // we can delete it from StackPanel
                        RemoveLastSlide();
                }
                else
                    stillDeleting = false;
            }

        }

        public void Save(String fileName) {
            StreamWriter sw1 = new StreamWriter(@"C:\Users\Aleksander\Desktop\result.txt");

            foreach (Slide slide in slidesList)
            {
                sw1.Write(slide.SlideText.Text);
            }

            sw1.Close();
        }

        public void Print(FileStream fs)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            // adding title
            var title = new iTextSharp.text.Paragraph(titleBox.Text);
            doc.Add(title);

            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            foreach (Slide slide in slidesList)
                slide.PrintSlide(table);
            
            doc.Add(table);
            doc.Close();
        }

    }
}
