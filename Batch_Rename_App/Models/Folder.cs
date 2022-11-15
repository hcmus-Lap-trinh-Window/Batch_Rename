using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename_App.Models
{
    public class MyFolder : INotifyPropertyChanged
    {
        public string FolderName { get; set; }
        public string NewFolderName { get; set; }
        public string FolderPath { get; set; }  
        public string FolderStatus { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public MyFolder()
        {
            FolderName = "";
            NewFolderName = "";
            FolderPath = "";
            FolderStatus = "";
        }

        public MyFolder(string folderdir)
        {
            FolderName = folderdir.Substring(folderdir.LastIndexOf(@"\") + 1, folderdir.Length - (folderdir.LastIndexOf(@"\") + 1));
            NewFolderName = FolderName;
            FolderPath = folderdir;
            FolderStatus = "";
        }

        public bool checkExist()
        {
            if (!Directory.Exists(this.FolderPath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
