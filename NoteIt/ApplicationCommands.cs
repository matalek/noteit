using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace NoteIt.Commands
{
    public class ApplicationCommands
    {
        public static RoutedUICommand ImportCmd
            = new RoutedUICommand("Import PDF command", "ImportCmd", typeof(ApplicationCommands));

        public static RoutedUICommand ExportCmd
            = new RoutedUICommand("Export PDF command", "ExportCmd", typeof(ApplicationCommands));

        public static RoutedUICommand ExitCmd
            = new RoutedUICommand("Exit command", "ExitCmd", typeof(ApplicationCommands));
    }
}
