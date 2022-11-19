using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace CommonModel
{
    /// <summary>
    /// Thêm hậu tố cho tên file
    /// </summary>
    public class AddSuffixRule : IRule
    {
        public string Name { get ; set ; }
        public bool IsInUse { get ; set ; }
        public string Suffix { get; set; }
        public AddSuffixRuleWindow ConfigurationUI { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public AddSuffixRule()
        {
            this.Name = "Add Suffix";
            Suffix = String.Empty;
            var instance = this;
            ConfigurationUI = new AddSuffixRuleWindow(ref instance);
        }

        public string Apply(string originalValue, object parameters)
        {
            try
            {
                if (this.Suffix.IsNullOrWhiteSpace())
                {
                    throw new Exception($"Suffix value là null, rỗng hoặc khoảng trắng.");
                }
                var newValue = originalValue + Suffix;
                return newValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }

        public List<string> Apply(List<string> orginStringList, object parameters)
        {
            try
            {
                if (orginStringList == null || orginStringList.Count < 1)
                {
                    throw new Exception("Apply failed. originStringList is null or empty");
                }
                var resultStringList = new List<string>();
                foreach (var orginString in orginStringList)
                {
                    var resultString = this.Apply(orginString, parameters);
                    resultStringList.Add(resultString);
                }
                return resultStringList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException ?? ex);
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
