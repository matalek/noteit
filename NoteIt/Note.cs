using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        // crates note with one empty slide
        public Note(StackPanel panel)
        {
            this.panel = panel;
            slidesList = new List<Slide>();
            slidesList.Add(new Slide(0, this));
            panel.Children.Add(slidesList.First().Grid);

        }

        public void AddSlideOnEnd()
        {
            slidesList.Add(new Slide(slidesList.Count, this));
            panel.Children.Add(slidesList.Last().Grid);
        }

        public void AddSlide_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
            // remove prefix addSlideButton from name
            name = name.Substring(14, name.Length - 14);
            int nr = Convert.ToInt32(name);
            Debug.Write(nr + "\n");

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

        public void DeleteSlide_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
            // remove prefix addSlideButton from name
            name = name.Substring(17, name.Length - 17);
            int nr = Convert.ToInt32(name);
            Debug.Write(nr + "\n");

            // copy slide's content to previoues slide
            for (int i = nr; i < slidesList.Count - 1; i++)
            {
                Debug.WriteLine("przepisuję " + slidesList[i + 1].SlideText + " do " + slidesList[i].SlideText);
                slidesList[i].SlideText = slidesList[i + 1].SlideText;
                
            }

            // delete the last slide
            Slide last = slidesList.Last();        
            last.Remove();
            // if no PDF image is present, we also delete it from stack panel
            panel.Children.Remove(last.Grid);
            slidesList.RemoveAt(slidesList.Count - 1);
            
        }



        public void AddPdf(FileSource fs)
        {
            var mainPanel = new MoonPdfPanel();
            mainPanel.InitializeComponent();
            mainPanel.Open(fs);

            for (int i = 0; i < MuPdfWrapper.CountPages(fs); i++)
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

            PdfPTable table = new PdfPTable(2);

            foreach (Slide slide in slidesList)
                slide.PrintSlide(table);
            
            doc.Add(table);
            doc.Close();
        }

    }
}
