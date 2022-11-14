using RuleWindow;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CommonModel
{
    /// <summary>
    /// Thêm tiền tố cho tên file
    /// </summary>
    public class AddPrefixRule : IRule
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }
        public string Prefix { get; set; }
        public AddPrefixRuleWindow ConfigurationUI { get; set; }

        public AddPrefixRule()
        {
            this.Name = "Add Prefix";
            Prefix = String.Empty;
            var instance = this;
            ConfigurationUI = new AddPrefixRuleWindow(ref instance);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Apply(string originalValue, object parameters)
        {
            try
            {
                if(this.Prefix.IsNullOrWhiteSpace())
                {
                    throw new Exception($"Prefix value là null, rỗng hoặc khoảng trắng.");
                }
                var newValue = Prefix + originalValue;
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
                if(orginStringList == null || orginStringList.Count < 1)
                {
                    throw new Exception("Apply failed. originStringList is null or empty");
                }
                var resultStringList = new List<string>();
                foreach(var orginString in orginStringList)
                {
                    var resultString = this.Apply(orginString, parameters);
                    resultStringList.Add(resultString);
                }
                return orginStringList;
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message, ex.InnerException ?? ex);
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class AddPrefixRuleData
    {
        public string Prefix { get; set; }
    }
}
