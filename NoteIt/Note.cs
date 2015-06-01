using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Diagnostics;
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoteIt
{
    public class Note
    {
        private List<Slide> slidesList;

        private StackPanel panel;

        private TextBox titleBox;

        private bool pdfPresent = false;

        string fileName; // where the note is saved

        bool isSaved; // true, if user hasn't made any changes since last saving

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
            titleBox = new TitleBox(this);
            panel.Children.Add(titleBox);

            slidesList = new List<Slide>();

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

            isSaved = true;
        }

        public Note(StackPanel panel, string fileName) : this(panel)
        {
            this.fileName = fileName;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            SavableNote savableNote = (SavableNote)formatter.Deserialize(stream);
            stream.Close();

            titleBox.Text = savableNote.Title;

            foreach (SavableSlide slide in savableNote.SlidesList)
                AddSlideOnEnd(slide);

            isSaved = true;
        }

        public void AddSlideOnEnd()
        {
            slidesList.Add(new Slide(slidesList.Count, this));
            panel.Children.Add(slidesList.Last().Grid);
        }

        public void AddSlideOnEnd(SavableSlide slide)
        {
            slidesList.Add(new Slide(slidesList.Count, this, slide));
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

        public bool FileAssigned()
        {
            return fileName != null;
        }

        public void Save() 
        {
            SavableNote savableNote = new SavableNote();
            savableNote.Title = titleBox.Text;
            foreach (Slide slide in slidesList)
                savableNote.AddSlide(new SavableSlide(slide));

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, savableNote);
            stream.Close();

            isSaved = true;
        }

        public void SaveAs(String fileName)
        {
            this.fileName = fileName;
            Save();
        }

        public void MarkAsChanged()
        {
            isSaved = false;
        }

        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
        }

        public string Title
        {
            get
            {
                return titleBox.Text;
            }
        }

        public List<Slide> SlidesList
        {
            get
            {
                return slidesList;
            }
        }
    }
}
