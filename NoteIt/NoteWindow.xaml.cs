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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace NoteIt
{
    /// <summary>
    /// Interaction logic for NotePage.xaml
    /// </summary>
    public partial class NoteWindow : MetroWindow
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
            var printWindow = new PrintWindow(note);
            printWindow.ShowDialog();   
        }

        // we need to use this field in order to prevent window from closing when asynchronously displaying message
        private bool isClosingConfirmed = false;
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosingConfirmed)
                return; // will close

            CloseApp();
            e.Cancel = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            CloseApp();
        }

        private async void CloseApp()
        {
            bool shallContinue = await ContinueIfUnsaved();
            if (!shallContinue)
                return;

            isClosingConfirmed = true;
            Application.Current.Shutdown();
        }

        private async void ImportPdf_Click(object sender, RoutedEventArgs e)
        {
            if (note.IsPdfPresent)
            {
                MessageDialogResult result = await this.ShowMessageAsync(
                    "You have already imported a PDF file!", 
                    "You will loose currently imported PDF file from your note. Do you want to continue?",
                    MessageDialogStyle.AffirmativeAndNegative);

                if (result != MessageDialogResult.Affirmative)  
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

        // if note isn't saved, asks user, if he wants to continue and returns his answer
        private async Task<bool> ContinueIfUnsaved()
        {
            if (!note.IsSaved)
            {
                MessageDialogResult result = await this.ShowMessageAsync(
                    "Your note is not saved!", 
                    "You will loose all changes since previous save. Do you want to continue?",
                    MessageDialogStyle.AffirmativeAndNegative);

                return (result == MessageDialogResult.Affirmative);                
            }
            else
                return true;
        }

        private async void NewNote_Click(object sender, RoutedEventArgs e)
        {
            bool shallContinue = await ContinueIfUnsaved();
            if (!shallContinue)
                return;

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
        

        private async void OpenNote_Click(object sender, RoutedEventArgs e)
        {
            bool shallContinue = await ContinueIfUnsaved();
            if (!shallContinue)
                return;

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
