using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Utility
{
    internal interface IDialogService
    {
        string OpenFileDialog(string defaultExt, string filter);
        string SaveFileDiaolog(string defaultExt, string filter);
    }
}
