using CommonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch_Rename_App.Models
{
    public class MyFile : INotifyPropertyChanged
    {
        
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        public string FileExtension { get; set; }
        public string FileImage { get; set; }
        public string FilePath { get; set; }
        public string FileStatus { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;


        public MyFile()
        {
            FileName = "";
            NewFileName = "";
            FileExtension = "";
            FileImage = "";
            FilePath = "";
            FileStatus = "";
        }

        public MyFile(string filedir)
        {
            FileInfo myfile = new FileInfo(filedir);
            FileName = myfile.Name; 
            NewFileName = this.FileName;
            FileExtension = myfile.Extension;
            FileImage = imageForExtension(FileExtension);
            FilePath = filedir;
            FileStatus = "";
        }

        private string imageForExtension(string fileExtension)
        {
            string imageExtension = fileExtension.ToUpper();
            string imageExtensionPath;

            switch (imageExtension)
            {
                case ".PNG":
                    imageExtensionPath = "Images/end-file-extensions/png-file.png";
                    break;
                case ".DOC":
                    imageExtensionPath = "Images/end-file-extensions/doc-file.png";
                    break;                
                case ".DOCX":
                    imageExtensionPath = "Images/end-file-extensions/docx-file.png";
                    break;
                case ".TXT":
                    imageExtensionPath = "Images/end-file-extensions/txt-file.png";
                    break;
                case ".CSV":
                    imageExtensionPath = "Images/end-file-extensions/csv-file.png";
                    break;
                case ".DLL":
                    imageExtensionPath = "Images/end-file-extensions/dll-file.png";
                    break;
                case ".PDF":
                    imageExtensionPath = "Images/end-file-extensions/pdf-file.png";
                    break;
                case ".PPT":
                case ".PPTX":
                    imageExtensionPath = "Images/end-file-extensions/ppt-file.png";
                    break;
                case ".XLS":
                    imageExtensionPath = "Images/end-file-extensions/xls-file.png";
                    break;
                case ".ZIP":
                    imageExtensionPath = "Images/end-file-extensions/zip-file.png";
                    break;
                case ".JPEG":
                    imageExtensionPath = "Images/end-file-extensions/jpeg-file.png";
                    break;
                case ".JPG":
                    imageExtensionPath = "Images/end-file-extensions/jpg-file.png";
                    break;
                case ".MP3":
                    imageExtensionPath = "Images/end-file-extensions/mp3-file.png";
                    break;
                case ".MP4":
                    imageExtensionPath = "Images/end-file-extensions/mp4-file.png";
                    break;
                case ".EXE":
                    imageExtensionPath = "Images/end-file-extensions/exe-file.png";
                    break;
                case ".WMV":
                    imageExtensionPath = "Images/end-file-extensions/wmv-file.png";
                    break;
                case ".RAR":
                    imageExtensionPath = "Images/end-file-extensions/rar-file.png";
                    break;
                case ".AI":
                    imageExtensionPath = "Images/end-file-extensions/ai-file.png";
                    break;
                case ".BAT":
                    imageExtensionPath = "Images/end-file-extensions/bat-file.png";
                    break;
                case ".BIN":
                    imageExtensionPath = "Images/end-file-extensions/bin-file.png";
                    break;
                case ".CHM":
                    imageExtensionPath = "Images/end-file-extensions/chm-file.png";
                    break;
                case ".CSS":
                    imageExtensionPath = "Images/end-file-extensions/css-file.png";
                    break;
                case ".CUR":
                    imageExtensionPath = "Images/end-file-extensions/cur-file.png";
                    break;
                case ".DAT":
                    imageExtensionPath = "Images/end-file-extensions/dat-file.png";
                    break;
                case ".TIF":
                    imageExtensionPath = "Images/end-file-extensions/tif-file.png";
                    break;
                case ".GHO":
                    imageExtensionPath = "Images/end-file-extensions/gho-file.png";
                    break;
                case ".GIF":
                    imageExtensionPath = "Images/end-file-extensions/gif-file.png";
                    break;
                case ".JAR":
                    imageExtensionPath = "Images/end-file-extensions/jar-file.png";
                    break;
                case ".JAVA":
                    imageExtensionPath = "Images/end-file-extensions/java-file.png";
                    break;
                case ".INK":
                    imageExtensionPath = "Images/end-file-extensions/ink-file.png";
                    break;
                case ".MOV":
                    imageExtensionPath = "Images/end-file-extensions/mov-file.png";
                    break;
                case ".MPG":
                    imageExtensionPath = "Images/end-file-extensions/mpg-file.png";
                    break;
                case ".ODT":
                    imageExtensionPath = "Images/end-file-extensions/odt-file.png";
                    break;
                case ".PRG":
                    imageExtensionPath = "Images/end-file-extensions/prg-file.png";
                    break;
                case ".RA":
                    imageExtensionPath = "Images/end-file-extensions/ra-file.png";
                    break;
                case ".RAW":
                    imageExtensionPath = "Images/end-file-extensions/raw-file.png";
                    break;
                case ".RSS":
                    imageExtensionPath = "Images/end-file-extensions/rss-file.png";
                    break;
                case ".SCR":
                    imageExtensionPath = "Images/end-file-extensions/scr-file.png";
                    break;
                case ".SQL":
                    imageExtensionPath = "Images/end-file-extensions/sql-file.png";
                    break;
                case ".WAV":
                    imageExtensionPath = "Images/end-file-extensions/wav-file.png";
                    break;
                case ".XVID":
                    imageExtensionPath = "Images/end-file-extensions/xvid-file.png";
                    break;
                case ".ICO":
                    imageExtensionPath = "Images/end-file-extensions/ico-file.png";
                    break;
                case ".OBJ":
                    imageExtensionPath = "Images/end-file-extensions/obj-file.png";
                    break;
                case ".PHP":
                    imageExtensionPath = "Images/end-file-extensions/php-file.png";
                    break;
                case ".PS":
                    imageExtensionPath = "Images/end-file-extensions/ps-file.png";
                    break;
                case ".XML":
                    imageExtensionPath = "Images/end-file-extensions/xml-file.png";
                    break;
                default:
                    imageExtensionPath = "Images/end-file-extensions/default-file.png";
                    break;
            }
            return imageExtensionPath;
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
