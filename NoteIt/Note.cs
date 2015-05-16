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
    class Note
    {
        private List<Slide> slidesList;

        private StackPanel panel;

        // crates note with one empty slide
        public Note(StackPanel panel)
        {
            this.panel = panel;
            slidesList = new List<Slide>();
            slidesList.Add(new Slide());
            panel.Children.Add(slidesList.First().Grid);

        }

        public void AddSlide()
        {
            slidesList.Add(new Slide());
            panel.Children.Add(slidesList.Last().Grid);
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
                    slidesList.Add(new Slide());
                    slidesList.Last().AddPdfSlide(fs, i);
                    panel.Children.Add(slidesList.Last().Grid);
                }

                
            }

        }

        public void Save(String fileName) {
            StreamWriter sw1 = new StreamWriter(@"C:\Users\Aleksander\Desktop\result.txt");

            foreach (Slide slide in slidesList)
            {
                sw1.Write(slide.SlideNote.Text);
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
