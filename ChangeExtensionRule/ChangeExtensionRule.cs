using CommonModel;
using CommonModel.Model;
using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CommonModel
{
    public class ChangeExtensionRule : IRule
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public bool IsInUse { get; set; }
        [JsonIgnore]
        public UserControl ConfigurationUI { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public ChangeExtensionRule()
        {
            Name = "Change Extension";
            Extension = "";
            ConfigurationUI = new ChangeExtensionRuleWindow(this);
        }

        public string Apply(string originString, object parameters)
        {
            var result = string.Empty;
            try
            {
                if (originString.IsNullOrWhiteSpace())
                {
                    throw new Exception("ChangeExtension failed! File name cannot be null!");
                }
                result = $"{originString.getFileName()}.{this.Extension}"; 
            
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return result;

        }
        public List<string> Apply(List<string> orginStringList, object parameters)
        {
            List<string> result = new List<string>();
            try
            {
                if (!ExtensionValidation(this.Extension))
                {
                    throw new Exception($"ChangeExtensionRule failed. Extension not accepted");
                }
                if (orginStringList != null && orginStringList.Count > 0)
                {
                    foreach (var origin in orginStringList)
                    {
                        var newFileName = Apply(origin, parameters);
                        result.Add(newFileName);
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

        public IRule Clone(RuleJson ruleJson)
        {
            ChangeExtensionRule instance = null;
            try
            {
                instance = JsonSerializer.Deserialize<ChangeExtensionRule>(ruleJson.Json);
                if (instance != null)
                {
                    instance.ConfigurationUI = new ChangeExtensionRuleWindow(instance);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
            return instance;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
