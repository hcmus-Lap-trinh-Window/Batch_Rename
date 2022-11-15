using RuleWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CommonModel
{
    public class ConvertToLowerCaseRule : IRule
    {
        public string Name { get; set; }
        public bool IsInUse { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ConvertToLowerCaseRuleWindow ConfigurationUI { get; set; }

        public ConvertToLowerCaseRule()
        {
            Name = "Convert To Lower Case and Trim WhiteSpace";
            IsInUse = false;
            var instance = this;
            ConfigurationUI = new ConvertToLowerCaseRuleWindow(ref instance);
        }

        public string Apply(string originString, object parameters)
        {
            string result = string.Empty;
            try
            {
                if (!originString.IsNullOrWhiteSpace())
                {
                    throw new Exception("ConvertToLowerCase Failed. File name cannot be null!");
                }
                result = originString.ToLower().Trim();
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
                if (orginStringList != null && orginStringList.Count > 0)
                {
                    foreach (var origin in orginStringList)
                    {
                        result.Add(Apply(origin, parameters));
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
    }
}
