using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MoonPdfLib;
using System.Diagnostics;

namespace NoteIt
{
    public partial class UserControl3 : UserControl
    {
        public UserControl3(string filename)
        {
            InitializeComponent();
            panel.InitializeComponent();
            panel.ViewType = ViewType.SinglePage;

            panel.OpenFile(filename);
            panel.Width = 200;
            panel.Height = 200;

            Debug.WriteLine(panel.TotalPages);
        }

        private MoonPdfPanel panel = new MoonPdfPanel();
    }
}
