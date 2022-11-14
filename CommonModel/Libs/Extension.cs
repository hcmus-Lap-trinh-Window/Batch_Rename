using System.IO;
using System.Linq;
using System;

namespace CommonModel
{
    static public partial class Extension
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        /// <summary>
        /// Get the output file name and wanted extension.
        /// return true if file name is in the correct pattern and contains wanted extension
        /// else false
        /// </summary>
        /// <param name="outputName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static bool FileWithExtensionValidation(this string outputName, string ext)
        {
            bool result = false;
            if (outputName.Contains(ext, StringComparison.OrdinalIgnoreCase) && outputName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Get File name only, not include extension
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static string getFileName(this string fullFileName)
        {
            return Path.GetFileNameWithoutExtension(fullFileName);
        }
        /// <summary>
        /// Get Extension only, not include file name
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static string getExtension(this string fullFileName)
        {
            return Path.GetExtension(fullFileName);
        }
    }
}