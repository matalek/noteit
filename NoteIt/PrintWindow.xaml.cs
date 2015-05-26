using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.IO;

namespace NoteIt
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        private Note note;

        public PrintWindow(Note note)
        {
            InitializeComponent();
            this.note = note;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IPrintStrategy printStrategy = null;
            if (verticalRadioButton.IsChecked == true)
            {
                printStrategy = new VerticalPrintStrategy();
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF Files (.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                printStrategy.Print(note, new FileStream(dialog.FileName, FileMode.Create));
                Close();
            }

        }
    }
}
