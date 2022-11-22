using CommonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Model
{
    public class MyFile : INotifyPropertyChanged
    {
        
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public string FileStatus { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;


        public MyFile()
        {
            FileName = "";
            NewFileName = "";
            FileExtension = "";
            FilePath = "";
            FileStatus = "";
        }

        public MyFile(string filedir)
        {
            FileInfo myfile = new FileInfo(filedir);
            FileName = myfile.Name; 
            NewFileName = this.FileName;
            FileExtension = myfile.Extension;
            FilePath = filedir;
            FileStatus = "";
        }    

        public bool CheckExist()
        {
            FileInfo file = new FileInfo(this.FilePath);
            if (!file.Exists)
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
