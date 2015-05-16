using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteIt
{
    public partial class UserControl2 : UserControl
    {
        public UserControl2(string filename)
        {
            InitializeComponent();

            this.axAcroPDF1.LoadFile(filename);
            this.axAcroPDF1.setCurrentPage(2);
        }
    }
}
