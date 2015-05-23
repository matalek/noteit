﻿using System;
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
    public partial class NoteWindow : Window
    {

        private FileSource fs;

        private Note note;

        public NoteWindow()
        {
            InitializeComponent();
            note = new Note(slidesPanel);
            note.AddSlideOnEnd();
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
            if (note.IsPdfPresent)
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(
                    "You will loose currently imported PDF file from your note. Do you want to continue?", "Import PDF",
                    btnMessageBox, icnMessageBox);

                if (rsltMessageBox != MessageBoxResult.Yes)
                    return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                fs = new FileSource(dialog.FileName);
                note.AddPdf(fs);
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            // TO DO: check if saved
            slidesPanel.Children.Clear();
            note = new Note(slidesPanel);
            note.AddSlideOnEnd();
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (note.FileAssigned())
                note.Save();
            else
                SaveNoteAs_Click(sender, e);
        }

        private void SaveNoteAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "NoteIt Files (.note)|*.note";
            if (dialog.ShowDialog() == true)
                note.SaveAs(dialog.FileName);
        }
        

        private void OpenNote_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "NoteIt Files (.note)|*.note";
            if (dialog.ShowDialog() == true)
            {
                slidesPanel.Children.Clear();
                note = new Note(slidesPanel, dialog.FileName);
            }
        }

    }
}
