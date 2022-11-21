using CommonModel.Model;
using RuleWindow;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;

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
        [JsonIgnore]
        public UserControl ConfigurationUI { get; set; }

        public AddPrefixRule()
        {
            this.Name = "Add Prefix";
            Prefix = String.Empty;
            //var instance = this;
            //ConfigurationUI = new AddPrefixRuleWindow(ref instance);
            ConfigurationUI = new AddPrefixRuleWindow(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Apply(string originalValue, object parameters)
        {
            try
            {
                if(this.Prefix.IsNullOrWhiteSpace())
                {
                    return originalValue;
                    //throw new Exception($"Prefix value là null, rỗng hoặc khoảng trắng.");
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
                return resultStringList;
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

        public IRule Clone(RuleJson ruleJson)
        {
            AddPrefixRule instance = null;
            try
            {
                instance = JsonSerializer.Deserialize<AddPrefixRule>(ruleJson.Json);
                if (instance != null)
                {
                    //instance.ConfigurationUI = new AddPrefixRuleWindow(ref instance);
                    instance.ConfigurationUI = new AddPrefixRuleWindow(this);
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
