using CommonModel;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace CommonModel
{
    public class ChangeExtensionRule : IRule, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public bool IsInUse { get; set; }
        public ChangeExtensionRuleWindow ConfigurationUI { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public ChangeExtensionRule()
        {
            this.Name = "Change Extension";
            this.Extension = "";
            var instance = this;
            this.ConfigurationUI = new ChangeExtensionRuleWindow(ref instance);
        }

        public string Apply(string originString, object parameters)
        {
            throw new NotImplementedException();
        }
        public List<string> Apply(List<string> orginStringList, object parameters)
        {
            List<string> result = new List<string>();
            try
            {
                if (parameters == null)
                {
                    throw new Exception($"ChangeExtensionRule failed. Parameters cannot be null");
                }
                if (!ExtensionValidation(this.Extension))
                {
                    throw new Exception($"ChangeExtensionRule failed. Extension not accepted");
                }
                if (orginStringList != null && orginStringList.Count > 0)
                {
                    foreach (var origin in orginStringList)
                    {
                        var outputFileName = origin.getFileName() + this.Extension.ToLower();
                        if (outputFileName.FileWithExtensionValidation(this.Extension))
                        {
                            result.Add(outputFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return result;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
        /// <summary>
        /// validate input extension
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        private bool ExtensionValidation(string ext)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(ext))
            {
                if (ext.All(char.IsLetter))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
