using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Utility
{
    internal class DialogService: IDialogService
    {
        public string OpenFileDialog(string defaultExt, string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = defaultExt;// ".csv";
            ofd.Filter = filter;// "Doc (.csv)|*.csv*";

            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }

        public string SaveFileDiaolog(string defaultExt, string filter)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = defaultExt;
            sfd.Filter = filter;

            if (sfd.ShowDialog() == true)
            {
                return sfd.FileName;
            }
            return string.Empty;
        }
    }
}
