using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Batch_Rename_App.Converters
{
    public class ImageExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Images/end-file-extensions/default-file.png";
            else if(value is string)
            {
                string imageExtensionUpper = value.ToString().ToUpper();
                string imageExtensionLower = value.ToString().ToLower().Substring(1);
                List<string> fileExtensionList = new List<string>()
                { ".PNG", ".DOC", ".DOCX", ".TXT", ".CSV", ".DLL", ".PDF", ".PPT", ".PPTX",
                  ".XLS", ".ZIP", ".JPEG", ".JPG", ".MP3", ".MP4", ".EXE", ".WMV", ".RAR", ".AI", ".BAT",
                  ".BIN", ".CHM", ".CSS", ".CUR", ".DAT", ".TIF", ".GHO", ".GIF", ".JAR", ".JAVA", ".INK",
                  ".MOV", ".MPG", ".ODT", ".PRG", ".RA", ".RAW", ".RSS", ".SCR", ".SQL", ".WAV", ".XVID",
                  ".ICO", ".OBJ", ".PHP", ".PS", ".XML"
                };
                string imageExtensionPath = "Images/end-file-extensions/default-file.png";

                if (fileExtensionList.Contains(imageExtensionUpper))
                {
                    imageExtensionPath = $"Images/end-file-extensions/{imageExtensionLower}-file.png";
                }

                return imageExtensionPath;
            }
            throw new Exception("Invalid Value");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
