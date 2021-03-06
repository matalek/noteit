﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteIt
{
    /// <summary>
    /// Interaction logic for TitleBox.xaml
    /// </summary>
    public partial class TitleBox : TextBox
    {
        private Note note;
        
        public TitleBox(Note note)
        {
            InitializeComponent();
            this.note = note;
            this.TextChanged += (sender, e) => note.MarkAsChanged();
        }
    }
}
